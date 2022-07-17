namespace SharpXusb
{
    /// <summary>
    /// Referenced Windows error codes.
    /// </summary>
    public static class Win32Error
    {
        /// <summary>
        /// The operation completed successfully.
        /// </summary>
        public const int Success = 0; // ERROR_SUCCESS

        /// <summary>
        /// The device is not connected.
        /// </summary>
        public const int DeviceNotConnected = 1167; // ERROR_DEVICE_NOT_CONNECTED

        /// <summary>
        /// The operation was canceled by the user.
        /// </summary>
        public const int Cancelled = 1223; // ERROR_CANCELLED
    }

    /// <summary>
    /// Referenced Windows HRESULT codes.
    /// </summary>
    public static class HResult
    {
        /// <summary>
        /// The data necessary to complete this operation is not yet available.
        /// </summary>
        public const int E_Pending = unchecked((int)0x8000000A);
    }
}
