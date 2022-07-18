using System.Runtime.InteropServices;
using System.Threading;
using PInvoke;

namespace SharpXusb
{
    using static Kernel32;

    internal static class XusbCore
    {
        public static unsafe int Bus_GetInformation(SafeObjectHandle busHandle, out XusbBusInfo data)
        {
            fixed (XusbBusInfo* outBuffer = &data)
            {
                return Ioctl.Receive(busHandle, XusbIoctl.Bus_GetInformation, outBuffer, XusbBusInfo.Size, out _);
            }
        }

        public static unsafe int Bus_GetInformationEx(SafeObjectHandle busHandle, XusbDeviceVersion version,
            XusbBusInformationExType type, out XusbBusInfoEx data
        )
        {
            var inData = new XusbBuffer_GetBusInformationEx()
            {
                Version = (ushort)XusbDeviceVersion.v1_4,
                RequestType = (byte)type
            };

            fixed (XusbBusInfoEx* outBuffer = &data)
            {
                return Ioctl.SendReceive(busHandle, XusbIoctl.Bus_GetInformationEx, &inData,
                    XusbBuffer_GetBusInformationEx.Size, outBuffer, XusbBusInfoEx.Size, out _);
            }
        }

        public static unsafe int Device_GetLedState(SafeObjectHandle busHandle, XusbDeviceVersion version,
            byte deviceIndex, out XusbLedState data)
        {
#if !SHARPXUSB_NO_VERSION_GUARDS
            switch (version)
            {
                case XusbDeviceVersion.v1_0:
                case XusbDeviceVersion.v1_1:
                    break;

                default:
                    // Not supported, but don't want to throw an exception as a result
                    data = default;
                    return Win32Error.Success;
            }
#endif

            var inBuffer = new XusbBuffer_Common()
            {
                Version = (ushort)XusbDeviceVersion.v1_1,
                DeviceIndex = deviceIndex
            };

            fixed (XusbLedState* outBuffer = &data)
            {
                return Ioctl.SendReceive(busHandle, XusbIoctl.Device_GetLedState, &inBuffer,
                    XusbBuffer_Common.Size, outBuffer, XusbLedState.Size, out _);
            }
        }

        public static unsafe int Device_GetInputState(SafeObjectHandle busHandle, XusbDeviceVersion version,
            byte deviceIndex, out XusbInputState data)
        {
            data = new XusbInputState();
            switch (version)
            {
#if !SHARPXUSB_NO_VERSION_GUARDS
                case XusbDeviceVersion.v1_0:
                {
                    data.Version = (ushort)XusbDeviceVersion.v1_0;
                    fixed (XusbInputState_0100* outBuffer = &data.State_0100)
                    {
                        return Ioctl.SendReceive(busHandle, XusbIoctl.Device_GetInput, &deviceIndex,
                            sizeof(byte), outBuffer, XusbInputState_0100.Size, out _);
                    }
                }
#endif

                default:
                {
                    data.Version = (ushort)XusbDeviceVersion.v1_1;
                    var inData = new XusbBuffer_Common()
                    {
                        Version = (ushort)XusbDeviceVersion.v1_1,
                        DeviceIndex = deviceIndex
                    };

                    fixed (XusbInputState_0101* outBuffer = &data.State_0101)
                    {
                        return Ioctl.SendReceive(busHandle, XusbIoctl.Device_GetInput, &inData,
                            XusbBuffer_Common.Size, outBuffer, XusbInputState_0101.Size, out _);
                    }
                }
            }
        }

        public static unsafe int Device_SetState(SafeObjectHandle busHandle, byte deviceIndex, XusbLedSetting ledState)
        {
            var inBuffer = new XusbBuffer_SetState()
            {
                DeviceIndex = deviceIndex,
                LedState = (byte)ledState,
                Flags = (byte)XusbSetStateFlags.Led
            };

            return Ioctl.Send(busHandle, XusbIoctl.Device_SetState, &inBuffer, XusbBuffer_SetState.Size);
        }

        public static unsafe int Device_SetState(SafeObjectHandle busHandle, byte deviceIndex, XusbVibration vibration)
        {
            var inBuffer = new XusbBuffer_SetState()
            {
                DeviceIndex = deviceIndex,
                Vibration = vibration,
                Flags = (byte)XusbSetStateFlags.Vibration
            };

            return Ioctl.Send(busHandle, XusbIoctl.Device_SetState, &inBuffer, XusbBuffer_SetState.Size);
        }

        public static unsafe int Device_SetState(SafeObjectHandle busHandle, byte deviceIndex, XusbLedSetting ledState,
            XusbVibration vibration)
        {
            var inBuffer = new XusbBuffer_SetState()
            {
                DeviceIndex = deviceIndex,
                LedState = (byte)ledState,
                Vibration = vibration,
                Flags = (byte)XusbSetStateFlags.Led | (byte)XusbSetStateFlags.Vibration
            };

            return Ioctl.Send(busHandle, XusbIoctl.Device_SetState, &inBuffer, XusbBuffer_SetState.Size);
        }

        public static unsafe int Device_GetCapabilities(SafeObjectHandle busHandle, XusbDeviceVersion version,
            byte deviceIndex, out XusbCapabilities data)
        {
            data = default;
            switch (version)
            {
#if !SHARPXUSB_NO_VERSION_GUARDS
                case XusbDeviceVersion.v1_0:
                {
                    // Not supported, but don't want to throw an exception as a result
                    return Win32Error.Success;
                }

                case XusbDeviceVersion.v1_1:
                {
                    var inData = new XusbBuffer_Common()
                    {
                        Version = (ushort)XusbDeviceVersion.v1_1,
                        DeviceIndex = deviceIndex
                    };

                    fixed (XusbCapabilities_0101* outBuffer = &data.Capabilities_0101)
                    {
                        return Ioctl.SendReceive(busHandle, XusbIoctl.Device_GetCapabilities, &deviceIndex,
                            sizeof(byte), outBuffer, XusbCapabilities_0101.Size, out _);
                    }
                }
#endif

                default:
                {
                    var inData = new XusbBuffer_Common()
                    {
                        Version = (ushort)XusbDeviceVersion.v1_2,
                        DeviceIndex = deviceIndex
                    };

                    fixed (XusbCapabilities_0102* outBuffer = &data.Capabilities_0102)
                    {
                        return Ioctl.SendReceive(busHandle, XusbIoctl.Device_GetCapabilities, &deviceIndex,
                            sizeof(byte), outBuffer, XusbCapabilities_0102.Size, out _);
                    }
                }
            }
        }

        public static unsafe int Device_GetBatteryInformation(SafeObjectHandle busHandle, XusbDeviceVersion version,
            byte deviceIndex, out XusbBatteryInformation data, XusbSubDevice subDevice = XusbSubDevice.Gamepad)
        {
            data = default;
            switch (version)
            {
#if !SHARPXUSB_NO_VERSION_GUARDS
                case XusbDeviceVersion.v1_0:
                case XusbDeviceVersion.v1_1:
                {
                    // Not supported, but don't want to throw an exception as a result
                    return Win32Error.Success;
                }
#endif

                default:
                {
                    var inData = new XusbBuffer_GetBatteryInformation()
                    {
                        Version = (ushort)XusbDeviceVersion.v1_2,
                        DeviceIndex = deviceIndex,
                        SubDevice = (byte)subDevice
                    };

                    fixed (XusbBatteryInformation* outBuffer = &data)
                    {
                        return Ioctl.SendReceive(busHandle, XusbIoctl.Device_GetCapabilities, &deviceIndex,
                            sizeof(byte), outBuffer, XusbCapabilities_0102.Size, out _);
                    }
                }
            }
        }

        public static unsafe int Device_GetAudioDeviceInformation(SafeObjectHandle busHandle, XusbDeviceVersion version,
            byte deviceIndex, out XusbAudioDeviceInformation data)
        {
            data = default;
            switch (version)
            {
#if !SHARPXUSB_NO_VERSION_GUARDS
                case XusbDeviceVersion.v1_0:
                case XusbDeviceVersion.v1_1:
                {
                    // Not supported, but don't want to throw an exception as a result
                    return Win32Error.Success;
                }
#endif

                default:
                {
                    var inData = new XusbBuffer_Common()
                    {
                        Version = (ushort)XusbDeviceVersion.v1_2,
                        DeviceIndex = deviceIndex
                    };

                    fixed (XusbAudioDeviceInformation* outBuffer = &data)
                    {
                        return Ioctl.SendReceive(busHandle, XusbIoctl.Device_GetCapabilities, &deviceIndex,
                            sizeof(byte), outBuffer, XusbCapabilities_0102.Size, out _);
                    }
                }
            }
        }

        public static unsafe int Device_WaitForGuideButton(SafeObjectHandle busHandle_Async, EventWaitHandle waitHandle,
            XusbDeviceVersion version, byte deviceIndex, out XusbInputWaitState state)
        {
            var inData = new XusbBuffer_Common()
            {
                Version = (ushort)XusbDeviceVersion.v1_2,
                DeviceIndex = deviceIndex
            };

            return Device_WaitCommon(busHandle_Async, waitHandle, XusbIoctl.Device_WaitForGuide, &inData,
                XusbBuffer_Common.Size, out state);
        }

        public static unsafe int Device_WaitForInput(SafeObjectHandle busHandle_Async, EventWaitHandle waitHandle,
            XusbDeviceVersion version, byte deviceIndex, out XusbInputWaitState state)
        {
            var inData = new XusbBuffer_WaitForInput()
            {
                Version = (ushort)XusbDeviceVersion.v1_2,
                DeviceIndex = deviceIndex,
                unk = 3
            };

            return Device_WaitCommon(busHandle_Async, waitHandle, XusbIoctl.Device_WaitForInput, &inData,
                XusbBuffer_WaitForInput.Size, out state);
        }

        private static unsafe int Device_WaitCommon(SafeObjectHandle busHandle_Async, EventWaitHandle waitHandle,
            int ioctl, void* inBuffer, int inSize, out XusbInputWaitState state)
        {
            var overlapped = new NativeOverlapped()
            {
                EventHandle = waitHandle.SafeWaitHandle.DangerousGetHandle()
            };

            int result;
            fixed (XusbInputWaitState* outBuffer = &state)
            {
                result = Ioctl.SendReceive(busHandle_Async, ioctl, inBuffer, inSize, outBuffer,
                    XusbInputWaitState.Size, out _, &overlapped);
                if (result == HResult.E_Pending)
                {
                    bool bResult = Kernel32.GetOverlappedResult(busHandle_Async, &overlapped, out int bytes, true);
                    if (!bResult)
                    {
                        result = Marshal.GetLastWin32Error();
                    }
                    else if (bytes != XusbInputWaitState.Size)
                    {
                        result = Win32Error.Cancelled;
                    }
                    else if (state.Status == 0)
                    {
                        result = Win32Error.DeviceNotConnected;
                    }
                    else
                    {
                        result = Win32Error.Success;
                    }
                }
            }

            return result;
        }

        public static unsafe int Device_PowerOff(SafeObjectHandle busHandle, XusbDeviceVersion version, byte deviceIndex)
        {
#if !SHARPXUSB_NO_VERSION_GUARDS
            // Exclude unsupported versions
            switch (version)
            {
                case XusbDeviceVersion.v1_0:
                case XusbDeviceVersion.v1_1:
                // Not supported, but don't want to throw an exception as a result
                    return Win32Error.Success;
            }
#endif

            var buffer = new XusbBuffer_Common()
            {
                Version = (ushort)version,
                DeviceIndex = deviceIndex
            };

            return Ioctl.Send(busHandle, XusbIoctl.Device_PowerOff, &buffer, XusbBuffer_Common.Size);
        }
    }
}
