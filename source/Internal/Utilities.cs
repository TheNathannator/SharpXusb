using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using PInvoke;

namespace SharpXusb
{
    using static Kernel32;

    internal static class Utilities
    {
        /// <summary>
        /// Creates a file handle with the given path and, optionally, file creation flags.
        /// </summary>
        public static SafeObjectHandle CreateFile(string path, CreateFileFlags flags = CreateFileFlags.FILE_ATTRIBUTE_NORMAL)
        {
            var handle = Kernel32.CreateFile(
                path,
                ACCESS_MASK.GenericRight.GENERIC_READ | ACCESS_MASK.GenericRight.GENERIC_WRITE,
                FileShare.FILE_SHARE_READ | FileShare.FILE_SHARE_WRITE,
                IntPtr.Zero,
                CreationDisposition.OPEN_EXISTING,
                flags,
                SafeObjectHandle.Null
            );

            if (handle == null || handle.IsInvalid)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }

            return handle;
        }

        /// <summary>
        /// Throws an exception on a failure result.
        /// </summary>
        public static void ThrowOnError(int result)
        {
            if (result != Win32Error.Success)
            {
                throw new System.ComponentModel.Win32Exception(result);
            }
        }

        /// <summary>
        /// Throws an exception on a failure result.
        /// </summary>
        public static void ThrowOnError(bool result)
        {
            if (!result)
            {
                throw new System.ComponentModel.Win32Exception(Marshal.GetLastWin32Error());
            }
        }
    }
}
