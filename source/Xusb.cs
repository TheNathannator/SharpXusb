using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharpXusb
{
    public static class Xusb
    {
        public static int GetInformation(byte busIndex, out XusbBusInfo busInfo)
        {
            var bus = XusbList.GetBus(busIndex);
            if (bus != null)
            {
                return XusbCore.Bus_GetInformation(bus.Handle, out busInfo);
            }
            else
            {
                busInfo = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        public static int GetInformationEx(byte busIndex, out XusbBusInfoEx busInfo,
            XusbBusInformationExType type = XusbBusInformationExType.Basic)
        {
            var bus = XusbList.GetBus(busIndex);
            if (bus != null)
            {
                return XusbCore.Bus_GetInformationEx(bus.Handle, bus.Version, type, out busInfo);
            }
            else
            {
                busInfo = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        public static int GetInputState(byte userIndex, out XusbInputState state)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_GetInputState(device.AssociatedBus.Handle, device.Version, userIndex, out state);
            }
            else
            {
                state = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        public static int SetState(byte userIndex, XusbLedSetting ledState, XusbVibration vibration)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_SetState(device.AssociatedBus.Handle, userIndex, ledState, vibration);
            }
            else
            {
                return Win32Error.DeviceNotConnected;
            }
        }

        public static int SetState(byte userIndex, XusbLedSetting ledState, XusbVibration vibration, XusbSetStateFlags flags)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_SetState(device.AssociatedBus.Handle, userIndex, ledState, vibration, flags);
            }
            else
            {
                return Win32Error.DeviceNotConnected;
            }
        }

        public static int SetState(byte userIndex, XusbVibration vibration)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_SetState(device.AssociatedBus.Handle, userIndex, vibration);
            }
            else
            {
                return Win32Error.DeviceNotConnected;
            }
        }

        public static int SetState(byte userIndex, XusbLedSetting ledState)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_SetState(device.AssociatedBus.Handle, userIndex, ledState);
            }
            else
            {
                return Win32Error.DeviceNotConnected;
            }
        }

        public static int GetLedState(byte userIndex, out XusbLedState ledState)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_GetLedState(device.AssociatedBus.Handle, device.Version, userIndex, out ledState);
            }
            else
            {
                ledState = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        public static int GetCapabilities(byte userIndex, out XusbCapabilities capabilities)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_GetCapabilities(device.AssociatedBus.Handle, device.Version, userIndex, out capabilities);
            }
            else
            {
                capabilities = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        public static int GetBatteryInformation(byte userIndex, out XusbBatteryInformation batteryInfo,
            XusbSubDevice subDevice = XusbSubDevice.Gamepad)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_GetBatteryInformation(device.AssociatedBus.Handle, device.Version, userIndex,
                    out batteryInfo, subDevice);
            }
            else
            {
                batteryInfo = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        public static int GetAudioDeviceInformation(byte userIndex, out XusbAudioDeviceInformation audioInfo)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_GetAudioDeviceInformation(device.AssociatedBus.Handle, device.Version, userIndex,
                    out audioInfo);
            }
            else
            {
                audioInfo = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        public static int WaitForGuideButton(byte userIndex, out XusbInputState inputState)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_WaitForGuideButton(device.AssociatedBus.AsyncHandle,
                    userIndex, out inputState);
            }
            else
            {
                inputState = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        public static async Task<(int, XusbInputState)> WaitForGuideButtonAsync(byte userIndex)
        {
            return await Task.Run(() => {
                int result = WaitForGuideButton(userIndex, out var inputState);
                return (result, inputState);
            });
        }

        public static int WaitForInput(byte userIndex, out XusbInputState inputState)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_WaitForInput(device.AssociatedBus.AsyncHandle,
                    userIndex, out inputState);
            }
            else
            {
                inputState = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        public static async Task<(int, XusbInputState)> WaitForInputAsync(byte userIndex)
        {
            return await Task.Run(() => {
                int result = WaitForInput(userIndex, out var inputState);
                return (result, inputState);
            });
        }

        public static void CancelWait(byte userIndex)
        {
            XusbCore.Device_CancelWait(userIndex);
        }

        public static int PowerOffDevice(byte userIndex)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                int result = XusbCore.Device_PowerOff(device.AssociatedBus.Handle, device.Version, userIndex);
                if (result == Win32Error.DeviceNotConnected)
                {
                    // The device is already disconnected, so in a way this function was successful
                    return Win32Error.Success;
                }
                else
                {
                    return result;
                }
            }
            else
            {
                // The device is already disconnected, so in a way this function was successful
                return Win32Error.Success;
            }
        }
    }
}