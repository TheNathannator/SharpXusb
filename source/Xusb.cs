using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace SharpXusb
{
    /// <summary>
    /// An XInput-style interface for the XUSB interface.
    /// </summary>
    public static class Xusb
    {
        /// <inheritdoc cref="XusbCore.Bus_GetInformation(PInvoke.Kernel32.SafeObjectHandle, out XusbBusInfo)"/>
        /// <param name="busIndex">
        /// The index of the bus to query.
        /// </param>
        /// <param name="busInfo">
        /// The bus information.
        /// </param>
        /// <returns>
        /// <see cref="Win32Error.Success"/> if successful, <see cref="Win32Error.DeviceNotConnected"/> if device
        /// isn't present. Other codes may be returned by the system.
        /// </returns>
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

        /// <inheritdoc cref="XusbCore.Bus_GetInformationEx(PInvoke.Kernel32.SafeObjectHandle, XusbDeviceVersion,
        ///     XusbBusInformationExType, out XusbBusInfoEx)"/>
        /// <param name="busIndex">
        /// The index of the bus to query.
        /// </param>
        /// <param name="busInfo">
        /// The bus information.
        /// </param>
        /// <param name="type">
        /// The type of extended bus information to request.
        /// </param>
        /// <returns>
        /// <see cref="Win32Error.Success"/> if successful, <see cref="Win32Error.DeviceNotConnected"/> if device
        /// isn't present. Other codes may be returned by the system.
        /// </returns>
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

        /// <inheritdoc cref="XusbCore.Device_GetInputState(PInvoke.Kernel32.SafeObjectHandle, XusbDeviceVersion,
        ///     byte, out XusbInputState)"/>
        /// <param name="userIndex">
        /// The index of the device to query.
        /// </param>
        /// <param name="state">
        /// The input state of the device.
        /// </param>
        /// <returns>
        /// <see cref="Win32Error.Success"/> if successful, <see cref="Win32Error.DeviceNotConnected"/> if device
        /// isn't present. Other codes may be returned by the system.
        /// </returns>
        public static int GetInputState(byte userIndex, out XusbInputState state)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_GetInputState(device.AssociatedBus.Handle, device.Version, device.IndexOnBus, out state);
            }
            else
            {
                state = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        /// <inheritdoc cref="XusbCore.Device_SetState(PInvoke.Kernel32.SafeObjectHandle, byte, XusbLedSetting, XusbVibration)"/>
        /// <param name="userIndex">
        /// The index of the device to query.
        /// </param>
        /// <param name="ledState">
        /// The LED state to set.
        /// </param>
        /// <param name="vibration">
        /// The vibration strength to set.
        /// </param>
        /// <returns>
        /// <see cref="Win32Error.Success"/> if successful, <see cref="Win32Error.DeviceNotConnected"/> if device
        /// isn't present. Other codes may be returned by the system.
        /// </returns>
        public static int SetState(byte userIndex, XusbLedSetting ledState, XusbVibration vibration)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_SetState(device.AssociatedBus.Handle, device.IndexOnBus, ledState, vibration);
            }
            else
            {
                return Win32Error.DeviceNotConnected;
            }
        }

        /// <inheritdoc cref="SetState(byte, XusbLedSetting, XusbVibration)"/>
        /// <param name="flags">
        /// Flags for which parts of the state to set.
        /// </param>
        /// <exception cref="System.ArgumentException">
        /// <paramref name="flags"/> is an invalid value.
        /// </exception>
        public static int SetState(byte userIndex, XusbLedSetting ledState, XusbVibration vibration, XusbSetStateFlags flags)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_SetState(device.AssociatedBus.Handle, device.IndexOnBus, ledState, vibration, flags);
            }
            else
            {
                return Win32Error.DeviceNotConnected;
            }
        }

        /// <inheritdoc cref="SetState(byte, XusbLedSetting, XusbVibration)"/>
        public static int SetState(byte userIndex, XusbVibration vibration)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_SetState(device.AssociatedBus.Handle, device.IndexOnBus, vibration);
            }
            else
            {
                return Win32Error.DeviceNotConnected;
            }
        }

        /// <inheritdoc cref="SetState(byte, XusbLedSetting, XusbVibration)"/>
        public static int SetState(byte userIndex, XusbLedSetting ledState)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_SetState(device.AssociatedBus.Handle, device.IndexOnBus, ledState);
            }
            else
            {
                return Win32Error.DeviceNotConnected;
            }
        }

        /// <inheritdoc cref="XusbCore.Device_GetLedState(PInvoke.Kernel32.SafeObjectHandle, XusbDeviceVersion, byte, out XusbLedState)"/>
        /// <param name="userIndex">
        /// The index of the device to query.
        /// </param>
        /// <param name="ledState">
        /// The LED state of the device.
        /// </param>
        /// <returns>
        /// <see cref="Win32Error.Success"/> if successful, <see cref="Win32Error.DeviceNotConnected"/> if device
        /// isn't present. Other codes may be returned by the system.
        /// </returns>
        public static int GetLedState(byte userIndex, out XusbLedState ledState)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_GetLedState(device.AssociatedBus.Handle, device.Version, device.IndexOnBus, out ledState);
            }
            else
            {
                ledState = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        /// <inheritdoc cref="XusbCore.Device_GetCapabilities(PInvoke.Kernel32.SafeObjectHandle, XusbDeviceVersion, byte,
        ///     out XusbCapabilities)"/>
        /// <param name="userIndex">
        /// The index of the device to query.
        /// </param>
        /// <param name="capabilities">
        /// The capabilities of the device.
        /// </param>
        /// <returns>
        /// <see cref="Win32Error.Success"/> if successful, <see cref="Win32Error.DeviceNotConnected"/> if device
        /// isn't present. Other codes may be returned by the system.
        /// </returns>
        public static int GetCapabilities(byte userIndex, out XusbCapabilities capabilities)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_GetCapabilities(device.AssociatedBus.Handle, device.Version, device.IndexOnBus, out capabilities);
            }
            else
            {
                capabilities = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        /// <inheritdoc cref="XusbCore.Device_GetBatteryInformation(PInvoke.Kernel32.SafeObjectHandle, XusbDeviceVersion,
        ///     byte, out XusbBatteryInformation, XusbSubDevice)"/>
        /// <param name="userIndex">
        /// The index of the device to query.
        /// </param>
        /// <param name="batteryInfo">
        /// The battery information of the device.
        /// </param>
        /// <param name="subDevice">
        /// The sub-device to query.
        /// </param>
        /// <returns>
        /// <see cref="Win32Error.Success"/> if successful, <see cref="Win32Error.DeviceNotConnected"/> if device
        /// isn't present. Other codes may be returned by the system.
        /// </returns>
        public static int GetBatteryInformation(byte userIndex, out XusbBatteryInformation batteryInfo,
            XusbSubDevice subDevice = XusbSubDevice.Gamepad)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_GetBatteryInformation(device.AssociatedBus.Handle, device.Version, device.IndexOnBus,
                    out batteryInfo, subDevice);
            }
            else
            {
                batteryInfo = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        /// <inheritdoc cref="XusbCore.Device_GetAudioDeviceInformation(PInvoke.Kernel32.SafeObjectHandle, XusbDeviceVersion,
        ///     byte, out XusbAudioDeviceInformation)"/>
        /// <param name="userIndex">
        /// The index of the device to query.
        /// </param>
        /// <param name="audioInfo">
        /// The audio device information of the device.
        /// </param>
        /// <returns>
        /// <see cref="Win32Error.Success"/> if successful, <see cref="Win32Error.DeviceNotConnected"/> if device
        /// isn't present. Other codes may be returned by the system.
        /// </returns>
        public static int GetAudioDeviceInformation(byte userIndex, out XusbAudioDeviceInformation audioInfo)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_GetAudioDeviceInformation(device.AssociatedBus.Handle, device.Version, device.IndexOnBus,
                    out audioInfo);
            }
            else
            {
                audioInfo = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        /// <inheritdoc cref="XusbCore.Device_WaitForGuideButton(PInvoke.Kernel32.SafeObjectHandle, byte, byte, out XusbInputState)"/>
        /// <param name="userIndex">
        /// The index of the device to start a wait for.
        /// </param>
        /// <param name="inputState">
        /// The input state that ended the wait.
        /// </param>
        /// <returns>
        /// <see cref="Win32Error.Success"/> if successful, <see cref="Win32Error.DeviceNotConnected"/> if device
        /// isn't present, <see cref="Win32Error.OperationInProgress"/> if a wait operation is already taking place,
        /// <see cref="Win32Error.Cancelled"/> if the wait was cancelled. Other codes may be returned by the system.
        /// </returns>
        public static int WaitForGuideButton(byte userIndex, out XusbInputState inputState)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_WaitForGuideButton(device.AssociatedBus.AsyncHandle,
                    device.IndexOnBus, userIndex, out inputState);
            }
            else
            {
                inputState = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        /// <summary>
        /// Waits asynchronously for an input state where the guide button is active.
        /// </summary>
        /// <param name="userIndex">
        /// The index of the device to query.
        /// </param>
        /// <returns>
        /// A task that returns a tuple containing a Win32 error code and the input state that ended the wait.
        /// See <see cref="WaitForGuideButton(byte, out XusbInputState)"/> for specific error codes.
        /// </returns>
        public static Task<(int, XusbInputState)> WaitForGuideButtonAsync(byte userIndex)
        {
            return Task.Run(() => {
                int result = WaitForGuideButton(userIndex, out var inputState);
                return (result, inputState);
            });
        }

        /// <inheritdoc cref="XusbCore.Device_WaitForInput(PInvoke.Kernel32.SafeObjectHandle, byte, byte, out XusbInputState)"/>
        /// <param name="userIndex">
        /// The index of the device to query.
        /// </param>
        /// <param name="inputState">
        /// The input state that ended the wait.
        /// </param>
        /// <returns>
        /// <see cref="Win32Error.Success"/> if successful, <see cref="Win32Error.DeviceNotConnected"/> if device
        /// isn't present, <see cref="Win32Error.OperationInProgress"/> if a wait operation is already taking place,
        /// <see cref="Win32Error.Cancelled"/> if the wait was cancelled. Other codes may be returned by the system.
        /// </returns>
        public static int WaitForInput(byte userIndex, out XusbInputState inputState)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_WaitForInput(device.AssociatedBus.Handle,
                    device.IndexOnBus, userIndex, out inputState);
            }
            else
            {
                inputState = default;
                return Win32Error.DeviceNotConnected;
            }
        }

        /// <summary>
        /// Waits asynchronously for a new input state.
        /// </summary>
        /// <remarks>
        /// NOTE: This requires an active non-console window in order to work.
        /// This is a limitation imposed by the driver itself.
        /// </remarks>
        /// <param name="userIndex">
        /// The index of the device to query.
        /// </param>
        /// <returns>
        /// A task that returns a tuple containing a Win32 error code and the input state that ended the wait.
        /// See <see cref="WaitForInput(byte, out XusbInputState)"/> for specific error codes.
        /// </returns>
        public static Task<(int, XusbInputState)> WaitForInputAsync(byte userIndex)
        {
            return Task.Run(() => {
                int result = WaitForInput(userIndex, out var inputState);
                return (result, inputState);
            });
        }

        /// <inheritdoc cref="XusbCore.Device_CancelWait(byte)"/>
        /// <param name="userIndex">
        /// The index of the device to cancel the wait of.
        /// </param>
        public static void CancelWait(byte userIndex)
        {
            XusbCore.Device_CancelWait(userIndex);
        }

        /// <inheritdoc cref="XusbCore.Device_PowerOff(PInvoke.Kernel32.SafeObjectHandle, XusbDeviceVersion, byte)"/>
        /// <param name="userIndex">
        /// The index of the device to turn off.
        /// </param>
        /// <returns>
        /// <see cref="Win32Error.Success"/> if successful, <see cref="Win32Error.DeviceNotConnected"/> if device
        /// isn't present. Other codes may be returned by the system.
        /// </returns>
        public static int PowerOffDevice(byte userIndex)
        {
            var device = XusbList.GetDevice(userIndex);
            if (device != null)
            {
                return XusbCore.Device_PowerOff(device.AssociatedBus.Handle, device.Version, device.IndexOnBus);
            }
            else
            {
                // The device is already disconnected, so in a way this function was successful
                return Win32Error.Success;
            }
        }
    }
}