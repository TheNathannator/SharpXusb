using System;
using System.Collections.Generic;
using System.Diagnostics;
using Nefarius.Utilities.DeviceManagement.PnP;

namespace SharpXusb
{
    public static class XusbList
    {
        private static readonly object m_listLock = new object();
        private static readonly SortedDictionary<byte, XusbDevice> m_deviceList = new SortedDictionary<byte, XusbDevice>();
        private static readonly SortedDictionary<byte, XusbBus> m_busList = new SortedDictionary<byte, XusbBus>();
        private static readonly Dictionary<byte, byte> m_ledStateToIndex = new Dictionary<byte, byte>()
        {
            {(byte)XusbLedSetting.Player1_Blink, 0},
            {(byte)XusbLedSetting.Player2_Blink, 1},
            {(byte)XusbLedSetting.Player3_Blink, 2},
            {(byte)XusbLedSetting.Player4_Blink, 3},
            {(byte)XusbLedSetting.Player1, 0},
            {(byte)XusbLedSetting.Player2, 1},
            {(byte)XusbLedSetting.Player3, 2},
            {(byte)XusbLedSetting.Player4, 3}
        };

        public static SortedDictionary<byte, XusbDevice> DeviceList
        {
            get
            {
                lock (m_listLock)
                {
                    _Refresh();
                    return m_deviceList;
                }
            }
        }

        public static SortedDictionary<byte, XusbBus> BusList
        {
            get
            {
                lock (m_listLock)
                {
                    _Refresh();
                    return m_busList;
                }
            }
        }

        public static XusbDevice GetDevice(byte userIndex)
        {
            return ListHelper(m_deviceList, userIndex);
        }

        public static XusbBus GetBus(byte busIndex)
        {
            return ListHelper(m_busList, busIndex);
        }

        private static T ListHelper<T>(SortedDictionary<byte, T> list, byte index) where T : class
        {
            lock (m_listLock)
            {
                if (list.TryGetValue(index, out var item))
                {
                    return item;
                }
                else
                {
                    // Refresh list and try again
                    _Refresh();
                    if (list.TryGetValue(index, out item))
                    {
                        return item;
                    }
                    else
                    {
                        return null;
                    }
                }
            }
        }

        public static void Refresh()
        {
            lock (m_listLock)
            {
                _Refresh();
            }
        }

        private static void _Refresh()
        {
            m_busList.Clear();
            m_deviceList.Clear();
    
            for (int instance = 0;
                Devcon.FindByInterfaceGuid(DeviceInterfaceIds.XUsbDevice, out string path, out _, instance);
                instance++
            )
            {
                Debug.WriteLine($"Found bus: {path}");
                XusbBus bus;
                try
                {
                    bus = new XusbBus(path);
                    Debug.WriteLine("Created XusbBus for bus.");
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Couldn't create XusbBus for bus:");
                    Debug.WriteLine(ex);
                    continue;
                }

                if (!bus.TryGetInformation(out var busInfo))
                {
                    Debug.WriteLine($"Couldn't get bus info, skipping.");
                    continue;
                }

                Debug.WriteLine("Bus info:");
                Debug.Indent();
                Debug.WriteLine($"Version: 0x{busInfo.Version:X4}");
                Debug.WriteLine($"Max device count: {busInfo.MaxCount}");
                Debug.WriteLine($"Connected device count: {busInfo.DeviceCount}");
                Debug.WriteLine($"Status: {busInfo.Status}");
                Debug.WriteLine($"unk1: 0x{busInfo.unk1:X2}");
                Debug.WriteLine($"unk2: 0x{busInfo.unk2:X4}");
                Debug.WriteLine($"Vendor ID: 0x{busInfo.VendorId:X4}");
                Debug.WriteLine($"Product ID: 0x{busInfo.ProductId:X4}");
                Debug.Unindent();

                try
                {
                    m_busList.Add((byte)instance, bus);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("Couldn't add bus to list:");
                    Debug.WriteLine(ex);
                    Debug.WriteLine($"Attempted to add to index {instance}.");
                    Debug.WriteLine("Current list state:");
                    foreach (byte index in m_busList.Keys)
                    {
                        Debug.WriteLine($"{index} - {m_busList[index].DevicePath}");
                    }
                    continue;
                }

                if ((busInfo.Status & 0x80) != 0) // TODO: Figure out what this means, this value isn't named in OpenXInput
                {
                    Debug.WriteLine("Bus has 0x80 bit set, skipping.");
                    continue;
                }

                for (byte indexOnBus = 0; indexOnBus < busInfo.MaxCount; indexOnBus++)
                {
                    byte userIndex = 0xFF;
                    Debug.WriteLine($"Checking for device at index {indexOnBus}");
                    if (!bus.TryGetDeviceInputState(indexOnBus, out var inputState))
                    {
                        Debug.WriteLine("Couldn't get input state, skipping.");
                        continue;
                    }
                    Debug.WriteLine("Found device.");

                    // Attempt to use device's LED state to determine the user index
                    if (bus.TryGetDeviceLedState(indexOnBus, out var ledState))
                    {
                        if (!m_ledStateToIndex.TryGetValue(ledState.LEDState, out userIndex))
                        {
                            Debug.WriteLine($"Could not get user index for LED state {(XusbLedSetting)ledState.LEDState} ({ledState.LEDState}). Will insert device into next available index.");
                            userIndex = 0xFF;
                        }
                    }

                    // If user index couldn't be determined, use next-available user index
                    if (userIndex == 0xFF)
                    {
                        for (byte i = 0; i < 0xFF; i++)
                        {
                            if (!m_deviceList.ContainsKey(i))
                            {
                                userIndex = i;
                                break;
                            }
                        }

                        if (userIndex == 0xFF)
                        {
                            Debug.WriteLine("Maximum device count reached! Cannot add this device.");
                            break;
                        }
                    }

                    XusbDevice device;
                    try
                    {
                        device = new XusbDevice(bus, userIndex, indexOnBus);
                        Debug.WriteLine("Created XusbDevice for device.");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Couldn't create XusbDevice for device:");
                        Debug.WriteLine(ex);
                        continue;
                    }

#if DEBUG
                    Debug.WriteLine("Device info:");
                    Debug.Indent();
                    try
                    {
                        var capabilities = device.GetCapabilities();

                        if (capabilities.Version == (ushort)XusbDeviceVersion.ProcNotSupported)
                        {
                            Debug.WriteLine("This device does not support capability querying.");
                        }
                        else if (capabilities.Version == (ushort)XusbDeviceVersion.v1_1)
                        {
                            var caps_v1 = capabilities.Capabilities_v1;
                            Debug.WriteLine($"Type:     0x{caps_v1.Type:X2} ({(XusbControllerType)caps_v1.Type})");
                            Debug.WriteLine($"SubType:  0x{caps_v1.SubType:X2} ({(XusbControllerSubType)caps_v1.SubType})");
                        }
                        else
                        {
                            var caps_v2 = capabilities.Capabilities_v2;
                            Debug.WriteLine($"Type:       0x{caps_v2.Type:X2} ({(XusbControllerType)caps_v2.Type})");
                            Debug.WriteLine($"SubType:    0x{caps_v2.SubType:X2} ({(XusbControllerSubType)caps_v2.SubType})");
                            Debug.WriteLine($"Flags:      0x{caps_v2.Flags:X4} ({(XusbCapabilityFlags)caps_v2.Flags})");
                            Debug.WriteLine($"VendorId:   0x{caps_v2.VendorId:X4}");
                            Debug.WriteLine($"ProductId:  0x{caps_v2.ProductId:X4}");
                            Debug.WriteLine($"Revision:   0x{caps_v2.Revision:X4}");
                            Debug.WriteLine($"XusbId:     0x{caps_v2.XusbId:X8}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Could not retrieve device capabilities.");
                        Debug.WriteLine(ex);
                    }
                    Debug.Unindent();
#endif

                    try
                    {
                        m_deviceList.Add(userIndex, device);
                        Debug.WriteLine($"Added device as user index {userIndex}.");
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Couldn't add device to list:");
                        Debug.WriteLine(ex);
                        Debug.WriteLine($"Attempted to add to index {userIndex}.");
                        Debug.WriteLine("Current list state:");
                        foreach (byte index in m_deviceList.Keys)
                        {
                            Debug.WriteLine($"{index} - {m_deviceList[index].AssociatedBus.DevicePath}");
                        }
                        continue;
                    }
                }
            }
        }
    }
}
