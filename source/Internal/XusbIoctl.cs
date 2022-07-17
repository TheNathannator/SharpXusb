namespace SharpXusb
{
    using static Ioctl;
    internal static class XusbIoctl
    {
        private const int FILE_DEVICE_XUSB = TYPE_FLAG_VENDOR;

        public static readonly int
        Bus_GetInformation               = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x000, METHOD_BUFFERED, ACCESS_READ),
        Device_GetCapabilities           = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x001, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE),
        Device_GetLedState               = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x002, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE),
        Device_GetInput                  = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x003, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE),
        Device_SetState                  = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x004, METHOD_BUFFERED, ACCESS_WRITE),
        Device_WaitForGuide              = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x005, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE),
        Device_GetBatteryInformation     = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x006, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE),
        Device_PowerOff                  = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x007, METHOD_BUFFERED, ACCESS_WRITE),
        Device_GetAudioDeviceInformation = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x008, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE),
        Device_WaitForInput              = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x0EB, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE),
        Bus_GetInformationEx             = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x0FF, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE);
    }
}
