namespace SharpXusb
{
    using static Ioctl;
    /// <summary>
    /// IOCTL codes for the XUSB interface.
    /// </summary>
    internal static class XusbIoctl
    {
        private const int FILE_DEVICE_XUSB = TYPE_FLAG_VENDOR;

        /// <summary>
        /// Gets information about a bus.
        /// </summary>
        public static readonly int Bus_GetInformation               = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x000, METHOD_BUFFERED, ACCESS_READ);
        /// <summary>
        /// Gets a device's capabilities.
        /// </summary>
        public static readonly int Device_GetCapabilities           = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x001, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE);
        /// <summary>
        /// Gets a device's LED state.
        /// </summary>
        public static readonly int Device_GetLedState               = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x002, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE);
        /// <summary>
        /// Gets a device's input state.
        /// </summary>
        public static readonly int Device_GetInput                  = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x003, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE);
        /// <summary>
        /// Sets a device's LED and/or vibration state.
        /// </summary>
        public static readonly int Device_SetState                  = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x004, METHOD_BUFFERED, ACCESS_WRITE);
        /// <summary>
        /// Waits for an input state from a device that has the guide button pressed.
        /// </summary>
        public static readonly int Device_WaitForGuide              = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x005, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE);
        /// <summary>
        /// Gets a device's battery information.
        /// </summary>
        public static readonly int Device_GetBatteryInformation     = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x006, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE);
        /// <summary>
        /// Powers off a device.
        /// </summary>
        public static readonly int Device_PowerOff                  = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x007, METHOD_BUFFERED, ACCESS_WRITE);
        /// <summary>
        /// Gets a device's audio device information.
        /// </summary>
        public static readonly int Device_GetAudioDeviceInformation = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x008, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE);
        /// <summary>
        /// Waits for input from a device.
        /// </summary>
        /// <remarks>
        /// TODO: Does not seem to work currently, further investigation required.
        /// </remarks>
        public static readonly int Device_WaitForInput              = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x0EB, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE);
        /// <summary>
        /// Gets extended information about a bus.
        /// </summary>
        public static readonly int Bus_GetInformationEx             = Ioctl.Create(FILE_DEVICE_XUSB, FUNCTION_FLAG_CUSTOM | 0x0FF, METHOD_BUFFERED, ACCESS_READ | ACCESS_WRITE);
    }
}
