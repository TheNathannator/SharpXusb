using System;
using System.Collections.Generic;
using System.Diagnostics;
using Nefarius.Utilities.DeviceManagement.PnP;

namespace SharpXusb
{
    public static class XusbList
    {
        private static readonly object m_listLock = new object();
        private static readonly Dictionary<byte, XusbDevice> m_deviceList = new Dictionary<byte, XusbDevice>();
        private static readonly Dictionary<byte, XusbBus> m_busList = new Dictionary<byte, XusbBus>();
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

        public static Dictionary<byte, XusbDevice> DeviceList
        {
            get
            {
                Refresh();
                return m_deviceList;
            }
        }

        public static Dictionary<byte, XusbBus> BusList
        {
            get
            {
                Refresh();
                return m_busList;
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

        private static T ListHelper<T>(Dictionary<byte, T> list, byte index) where T : class
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
                    Refresh();
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

        private static void Refresh()
        {
            m_busList.Clear();
            m_deviceList.Clear();
    
            for (int instance = 0;
                Devcon.FindByInterfaceGuid(DeviceInterfaceIds.XUsbDevice, out string path, out _, instance);
                instance++
            )
            {
                Debug.WriteLine($"Found bus: {path}");
                var bus = new XusbBus(path);

                if (!bus.TryGetInformation(out var busInfo))
                {
                    Debug.WriteLine($"Couldn't get bus info, skipping.");
                    continue;
                }

                try
                {
                    m_busList.Add((byte)instance, bus);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Couldn't add bus to list:");
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
                    Debug.WriteLine($"Bus has 0x80 bit set, skipping.");
                    continue;
                }

                Debug.WriteLine($"Max device count: {busInfo.MaxCount}");
                Debug.WriteLine($"Connected device count: {busInfo.DeviceCount}");
                for (byte indexOnBus = 0; indexOnBus < busInfo.MaxCount; indexOnBus++)
                {
                    byte userIndex = 0xFF;
                    Debug.WriteLine($"Checking for device at index {indexOnBus}");
                    if (!bus.TryGetDeviceInputState(indexOnBus, out var inputState))
                    {
                        Debug.WriteLine($"Couldn't get input state, skipping.");
                        continue;
                    }

                    Debug.Assert(inputState.Version != (ushort)XusbDeviceVersion.ProcNotSupported, "Invalid device version detected, check for bugs!");

                    // Attempt to use device's LED state to determine the user index
                    if (bus.TryGetDeviceLedState(indexOnBus, out var ledState))
                    {
                        if (!m_ledStateToIndex.TryGetValue(ledState.LEDState, out userIndex))
                        {
                            Debug.WriteLine($"Could not get user index for LED state {(XusbLedSetting)ledState.LEDState} ({ledState.LEDState}).");
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

                    try
                    {
                        var device = new XusbDevice(bus, userIndex, indexOnBus);
                        m_deviceList.Add(userIndex, device);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Couldn't add device to list:");
                        Debug.WriteLine(ex);
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
