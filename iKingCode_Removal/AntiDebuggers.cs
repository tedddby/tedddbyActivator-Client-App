using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Threading;

namespace iKingCode_BypassHello
{

    internal sealed class AntiDebuggers
    {
        [DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
        private static extern bool CheckRemoteDebuggerPresent(IntPtr ptr, ref bool b);

        internal void RegisteredWaitHandleAssemblyProductAttribute()
        {
            new Thread(new ParameterizedThreadStart(SizedArrayAssemblyCopyrightAttribute)).Start(Thread.CurrentThread);
        }

        internal void SizedArrayAssemblyCopyrightAttribute(object th)
        {

            string[] array = new string[]
            {
                "codecracker",
                "x32dbg",
                "x64dbg",
                "charles",
                "dnspy",
                "simpleassembly",
                "peek",
                "httpanalyzer",
                "wireshark",
                "devirt",
                "logger",
                "usbtrace",
                "usbmonitor",
                "serialmonitor",
                "ilspy",
                "charlesproxy",
                "fiddler",
                "extremedumper",
                "megadumper",
                "x64netdumper",
                "dumper",
                "dump",
                "ollydbg ",
                "softice",
                "dotpeek",
                //"visual studio",
                "cheat engine",
                "cheatengine",
                "scylla",
                "scylla_x64",
                "scylla_x86",
                "protection_id",
                "reshacker",
                "ImportREC",
                "de4dot",
                "disassembly",
                "Import reconstructor",
                "debug",
                "debugger",
                "httpdebug",
                "httpdebug",
                "immunitydebugger",
                "immunity",
                "debug",
                "petool",
                "petools",
                "PE Tools",
                "ida",
                "ida64",
                "idag",
                "idag64",
                "idaw",
                "idaw64",
                "idaq",
                "idaq64",
                "idau",
                "idau64",
                "idag",
                "idaq",
                "windbg",
                "[CPU",
                "simpleassembly",
                "postman",
                "softice",
                "jetbrains",
                "Resource Monitor",
                "Resource",
                "Resource and Performancer Monitor",
                "Suspend Process",
                "processhacker",
                "Process Hacker",
                "perfmon",
                "valgrind",
                "SIMMON",
                "Rational Purify",
                "Memcheck",
                "Disassembler",
                "parasoft",
                "Dr. Memory",
                "WinHex",
                "Analyze It",
                "Hook Analyzer",
                "Process Explorer",
                "procmon64",
                "scylla",
                "de4dotmodded",
                "protection_id",
                "x96dbg",
                "process hacker",
                "process monitor",
                "qt5core",
                "dbgclr",
                "hxd",
                "import reconstructor",
                "Trw2000",
                "Winpdb",
                "procdump"
            };

            while (true)
            {
                try
                {

                    if (iOSDevice.debugMode == false)
                    {
                        if (System.Diagnostics.Debugger.IsAttached)
                        {
                            //Debugger.IsAttached
                            Process.GetCurrentProcess().Kill();
                        }

                        bool isDebuggerPresent = false;
                        CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);

                        if (isDebuggerPresent)
                        {
                            //isDebuggerPresent
                            Process.GetCurrentProcess().Kill();
                        }
                    }

                    foreach (Process process in Process.GetProcesses())
                    {

                        if (process != Process.GetCurrentProcess())
                        {
                            //Hackers tools detector

                            for (int j = 0; j < array.Length; j++)
                            {
                                int id = Process.GetCurrentProcess().Id;
                                if (process.ProcessName.ToLower().Contains(array[j]))
                                {
                                    if (iOSDevice.debugMode == false)
                                    {
                                        Process.GetCurrentProcess().Kill();
                                    }
                                }

                                if (process.MainWindowTitle.ToLower().Contains(array[j]))
                                {
                                    if (iOSDevice.debugMode == false)
                                    {
                                        Process.GetCurrentProcess().Kill();
                                    }
                                }
                            }

                        }

                    }
                }
                catch (Exception ex)
                {
                }

                Thread.Sleep(1000);
            }

        }

    }
}