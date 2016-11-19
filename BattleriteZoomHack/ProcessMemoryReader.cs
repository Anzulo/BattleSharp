using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
namespace BattleriteZoomhack
{
    class ProcessMemoryReader
    {
        [Flags]
        public enum ProcessAccessType
        {
            PROCESS_TERMINATE = (0x0001),
            PROCESS_CREATE_THREAD = (0x0002),
            PROCESS_SET_SESSIONID = (0x0004),
            PROCESS_VM_OPERATION = (0x0008),
            PROCESS_VM_READ = (0x0010),
            PROCESS_VM_WRITE = (0x0020),
            PROCESS_DUP_HANDLE = (0x0040),
            PROCESS_CREATE_PROCESS = (0x0080),
            PROCESS_SET_QUOTA = (0x0100),
            PROCESS_SET_INFORMATION = (0x0200),
            PROCESS_QUERY_INFORMATION = (0x0400)
        }
        [StructLayout(LayoutKind.Sequential)]
        public struct MEMORY_BASIC_INFORMATION
        {
            public IntPtr BaseAddress;
            public IntPtr AllocationBase;
            public uint AllocationProtect;
            public uint RegionSize;
            public uint State;
            public uint Protect;
            public uint Type;
        }
        [DllImport("kernel32.dll")]
        public static extern IntPtr OpenProcess(UInt32 dwDesiredAccess, Int32 bInheritHandle, UInt32 dwProcessId);
        [DllImport("kernel32.dll")]
        public static extern Int32 CloseHandle(IntPtr hObject);
        [DllImport("kernel32.dll")]
        public static extern Int32 ReadProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesRead);
        [DllImport("kernel32.dll")]
        public static extern Int32 WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, [In, Out] byte[] buffer, UInt32 size, out IntPtr lpNumberOfBytesWritten);
        [DllImport("kernel32.dll")]
        public static extern int VirtualQueryEx(IntPtr hProcess, IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, int dwLength);

        public ProcessMemoryReader(Process process)
        {
            ReadProcess = process;
            OpenProcess();
        }
        ~ProcessMemoryReader()
        {
            CloseHandle();
        }
        public Process ReadProcess = null;
        private IntPtr ProcessHandle = IntPtr.Zero;

        public void OpenProcess()
        {
            ProcessAccessType access = ProcessAccessType.PROCESS_VM_READ | ProcessAccessType.PROCESS_VM_WRITE | ProcessAccessType.PROCESS_VM_OPERATION;
            ProcessHandle = OpenProcess((uint)access, 1, (uint)ReadProcess.Id);
        }
        public void CloseHandle()
        {
            CloseHandle(ProcessHandle);
        }
        public byte ReadProcessMemory(int MemoryAddress)
        {
            return ReadProcessMemory(new IntPtr(MemoryAddress), 1)[0];
        }
        public byte[] ReadProcessMemory(IntPtr MemoryAddress, uint bytesToRead)
        {
            byte[] buffer = new byte[bytesToRead];
            IntPtr ptrBytesRead;
            ReadProcessMemory(ProcessHandle, MemoryAddress, buffer, bytesToRead, out ptrBytesRead);
            return buffer;
        }
        public int WriteProcessMemory(IntPtr MemoryAddress, byte[] bytesToWrite)
        {
            IntPtr ptrBytesWritten;
            int bytesWritten;
            WriteProcessMemory(ProcessHandle, MemoryAddress, bytesToWrite, (uint)bytesToWrite.Length, out ptrBytesWritten);
            bytesWritten = ptrBytesWritten.ToInt32();
            return bytesWritten;
        }
        public int VirtualQueryEx(IntPtr lpAddress, out MEMORY_BASIC_INFORMATION lpBuffer, int dwLength)
        {
            return VirtualQueryEx(ProcessHandle, lpAddress, out lpBuffer, dwLength);
        }
    }
}