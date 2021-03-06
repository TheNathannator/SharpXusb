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
                for (byte userIndex = 0; userIndex < busInfo.MaxCount; userIndex++)
                {
                    Debug.WriteLine($"Checking for device at index {userIndex}");
                    var device = new XusbDevice(bus, userIndex);

                    if (!device.TryGetInputState(out var inputState))
                    {
                        Debug.WriteLine($"Couldn't get input state, skipping.");
                        continue;
                    }

                    Debug.Assert(inputState.Version != (ushort)XusbDeviceVersion.ProcNotSupported, "Invalid device version detected, check for bugs!");

                    try
                    {
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
