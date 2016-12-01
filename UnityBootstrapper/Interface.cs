using System;
using System.Collections.Generic;
using System.Runtime.Remoting;
using EasyHook;

namespace UnityBootstrapper
{
    public class Interface : MarshalByRefObject
    {
        public static void Inject(int ProcessId, Interface Interface, String dll, String namespaceclassmethod)
        {
            String channel = null;
            RemoteHooking.IpcCreateServer(ref channel, WellKnownObjectMode.Singleton, Interface);
            var injectionLibraryPath = typeof(Interface).Assembly.Location;
            RemoteHooking.Inject(ProcessId, injectionLibraryPath, injectionLibraryPath, new object[] { channel, dll, namespaceclassmethod });
        }
    }
}