using System.Runtime.InteropServices;
using System.Threading;
using PInvoke;

namespace SharpXusb
{
    using static Kernel32;

    internal static class Ioctl
    {
        public const int
        TYPE_FLAG_VENDOR = 0x8000,

        FUNCTION_FLAG_CUSTOM = 0x800,

        METHOD_BUFFERED = 0,
        METHOD_IN_DIRECT = 1,
        METHOD_OUT_DIRECT = 2,
        METHOD_NEITHER = 3,

        ACCESS_ANY = 0x00,
        ACCESS_SPECIAL = ACCESS_ANY,
        ACCESS_READ = 0x01,
        ACCESS_WRITE = 0x02;

        public static int Create(int deviceType, int function, int method, int access)
        {
            return ((deviceType & 0xFFFF) << 16) | ((access & 3) << 14) | ((function & 0xFFF) << 2) | (method & 3);
        }

        public static unsafe int Send(SafeObjectHandle Device, int IoctlCode, void* InBuffer, int InBufferSize,
            NativeOverlapped* Overlapped = null
        )
        {
            return SendReceive(Device, IoctlCode, InBuffer, InBufferSize, null, 0, out _, Overlapped);
        }

        public static unsafe int Receive(SafeObjectHandle Device, int IoctlCode, void* OutBuffer, int OutBufferSize,
            out int BytesReturned, NativeOverlapped* Overlapped = null
        )
        {
            return SendReceive(Device, IoctlCode, null, 0, OutBuffer, OutBufferSize, out BytesReturned, Overlapped);
        }

        public static unsafe int SendReceive(SafeObjectHandle Device, int IoctlCode, void* InBuffer, int InBufferSize,
            void* OutBuffer, int OutBufferSize, out int BytesReturned, NativeOverlapped* Overlapped = null
        )
        {
            bool result = Kernel32.DeviceIoControl(Device, IoctlCode, InBuffer, InBufferSize, OutBuffer, OutBufferSize,
                out BytesReturned, Overlapped);
            return result ? Marshal.GetLastWin32Error() : Win32Error.Success;
        }
    }
}
