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
        /// An operation is currently in progress with the device.
        /// </summary>
        public const int OperationInProgress = 329; // ERROR_OPERATION_IN_PROGRESS

        /// <summary>
        /// Overlapped I/O operation is in progress.
        /// </summary>
        public const int IoPending = 997;

        /// <summary>
        /// The device is not connected.
        /// </summary>
        public const int DeviceNotConnected = 1167; // ERROR_DEVICE_NOT_CONNECTED

        /// <summary>
        /// The operation was canceled by the user.
        /// </summary>
        public const int Cancelled = 1223; // ERROR_CANCELLED
    }
}
