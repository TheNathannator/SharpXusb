using System.Runtime.InteropServices;
using System.Threading;
using PInvoke;

namespace SharpXusb
{
    using static Kernel32;

    /// <summary>
    /// Wrapper for IOCTL operations.
    /// </summary>
    internal static class Ioctl
    {
        /// <summary>
        /// Flag for vendor-defined IOCTL device types.
        /// </summary>
        public const int TYPE_FLAG_VENDOR = 0x8000;
        /// <summary>
        /// Flag for vendor-defined IOCTL functions.
        /// </summary>
        public const int FUNCTION_FLAG_CUSTOM = 0x800;
        /// <summary>
        /// Sets an IOCTL code as buffered.
        /// </summary>
        public const int METHOD_BUFFERED = 0;
        /// <summary>
        /// Sets read access for an IOCTL code.
        /// </summary>
        public const int ACCESS_READ = 0x01;
        /// <summary>
        /// Sets write access for an IOCTL code.
        /// </summary>
        public const int ACCESS_WRITE = 0x02;

        /// <summary>
        /// Creates an IOCTL code.
        /// </summary>
        public static int Create(int deviceType, int function, int method, int access)
        {
            return ((deviceType & 0xFFFF) << 16) | ((access & 3) << 14) | ((function & 0xFFF) << 2) | (method & 3);
        }

        /// <summary>
        /// Sends an IOCTL transfer.
        /// </summary>
        public static unsafe int Send(SafeObjectHandle Device, int IoctlCode, void* InBuffer, int InBufferSize,
            NativeOverlapped* Overlapped = null
        )
        {
            return SendReceive(Device, IoctlCode, InBuffer, InBufferSize, null, 0, out _, Overlapped);
        }

        /// <summary>
        /// Receives an IOCTL transfer.
        /// </summary>
        public static unsafe int Receive(SafeObjectHandle Device, int IoctlCode, void* OutBuffer, int OutBufferSize,
            out int BytesReturned, NativeOverlapped* Overlapped = null
        )
        {
            return SendReceive(Device, IoctlCode, null, 0, OutBuffer, OutBufferSize, out BytesReturned, Overlapped);
        }

        /// <summary>
        /// Sends and receives an IOCTL transfer.
        /// </summary>
        public static unsafe int SendReceive(SafeObjectHandle Device, int IoctlCode, void* InBuffer, int InBufferSize,
            void* OutBuffer, int OutBufferSize, out int BytesReturned, NativeOverlapped* Overlapped = null
        )
        {
            bool result = Kernel32.DeviceIoControl(Device, IoctlCode, InBuffer, InBufferSize, OutBuffer, OutBufferSize,
                out BytesReturned, Overlapped);
            return result ? Win32Error.Success : Marshal.GetLastWin32Error();
        }
    }
}
