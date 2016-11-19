using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
namespace BattleriteZoomhack
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                System.Diagnostics.Process battlerite = System.Diagnostics.Process.GetProcessesByName("Battlerite").FirstOrDefault();
                if (battlerite == null)
                {
                    Console.WriteLine("Waiting 5s for Battlerite...");
                    Thread.Sleep(5000);
                    continue;
                }
                ProcessMemoryReader pmr = new ProcessMemoryReader(battlerite);
                while (true)
                {
                    Console.WriteLine("Searching for zoom");
                    Int64 zoomAddr = 0;

                    var memInfos = new List<ProcessMemoryReader.MEMORY_BASIC_INFORMATION>();
                    IntPtr moduleAddress = new IntPtr();
                    while (true)
                    {
                        var memInfo = new ProcessMemoryReader.MEMORY_BASIC_INFORMATION();
                        int memQuery = pmr.VirtualQueryEx(moduleAddress, out memInfo, Marshal.SizeOf(memInfo));
                        if (memQuery == 0) break;
                        if ((memInfo.State & 0x1000) != 0 && (memInfo.Protect & 0x100) == 0)
                            memInfos.Add(memInfo);
                        moduleAddress = new IntPtr(memInfo.BaseAddress.ToInt32() + (int)memInfo.RegionSize);
                    }
                    for (int i = 0; i < memInfos.Count; i++)
                    {
                        Byte[] dumpedMemory = pmr.ReadProcessMemory(memInfos[i].BaseAddress, memInfos[i].RegionSize);
                        for (int j = 0x20; j < memInfos[i].RegionSize - 0x20; j += 8)
                        {
                            if (dumpedMemory[j + 2] == 0x8c && dumpedMemory[j + 3] == 0x41 && dumpedMemory[j - 0x16] == 0xf0 && dumpedMemory[j - 0x15] == 0x40
                                && dumpedMemory[j] == 0 && dumpedMemory[j + 1] == 0 && dumpedMemory[j - 0x17] == 0 && dumpedMemory[j - 0x18] == 0)
                            {
                                Byte[] timer = pmr.ReadProcessMemory(new IntPtr(memInfos[i].BaseAddress.ToInt32() + j + 0x1c8), 4);
                                Thread.Sleep(50);
                                Byte[] newTimer = pmr.ReadProcessMemory(new IntPtr(memInfos[i].BaseAddress.ToInt32() + j + 0x1c8), 4);
                                if (!timer.SequenceEqual(newTimer))
                                {
                                    Console.WriteLine("Found zoom @ " + (memInfos[i].BaseAddress.ToInt32() + j).ToString("X"));
                                    zoomAddr = memInfos[i].BaseAddress.ToInt32() + j;
                                    break;
                                }
                            }
                        }
                        if (zoomAddr != 0)
                            break;
                    }
                    if (zoomAddr == 0)
                    {
                        Thread.Sleep(5000);
                        break;
                    }
                    Console.WriteLine("Zoom hack activated!");
                    pmr.WriteProcessMemory(new IntPtr(zoomAddr), new Byte[4] { 0, 0, 0x48, 0x42 });
                    while (true)
                    {
                        Console.WriteLine("Waiting for new game");
                        Byte[] timer = pmr.ReadProcessMemory(new IntPtr(zoomAddr + 0x1c8), 4);
                        Thread.Sleep(50);
                        Byte[] newTimer = pmr.ReadProcessMemory(new IntPtr(zoomAddr + 0x1c8), 4);
                        if (timer.SequenceEqual(newTimer))
                        {
                            zoomAddr = 0;
                            break;
                        }
                        Thread.Sleep(5000);
                    }
                }
            }
        }
    }
}
