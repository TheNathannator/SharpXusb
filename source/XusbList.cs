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
                var bus = new XusbBus(path);
                XusbBusInfo busInfo;
                try
                {
                    busInfo = bus.GetInformation();
                    m_busList.Add((byte)instance, bus);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"Error creating XusbBus with index {instance}:");
                    Debug.WriteLine(ex);
                    continue;
                }

                for (byte userIndex = 0; userIndex < busInfo.MaxIndex; userIndex++)
                {
                    try
                    {
                        bus.GetDeviceInputState(userIndex);
                        m_deviceList.Add(userIndex, new XusbDevice(bus.Version, userIndex));
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine($"Error creating XusbDevice with index {userIndex}:");
                        Debug.WriteLine(ex);
                    }
                }
            }
        }
    }
}
