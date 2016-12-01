using System;
using System.IO;
using System.Reflection;
using EasyHook;

namespace UnityBootstrapper
{
    public class EntryPoint : IEntryPoint
    {
        readonly Interface _interface;

        public EntryPoint(RemoteHooking.IContext InContext, String InChannelName, String dll, String namespaceclassmethod)
        {
            _interface = RemoteHooking.IpcConnectClient<Interface>(InChannelName);
        }
        public void Run(RemoteHooking.IContext InContext, String InChannelName, String dll, String namespaceclassmethod)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => GetType().Assembly.FullName == args.Name ? GetType().Assembly : null;
            var monorootdomain = NativeMethods.mono_get_root_domain();
            NativeMethods.mono_thread_attach(monorootdomain);
            NativeMethods.mono_security_set_mode(0);
            var monodomain = NativeMethods.mono_domain_get();
            var monoassembly = NativeMethods.mono_domain_assembly_open(monorootdomain, dll);
            var monoassemblygetimage = NativeMethods.mono_assembly_get_image(monoassembly);
            var monoassemblydesc = NativeMethods.mono_method_desc_new(namespaceclassmethod, false);
            var monomethodcall = NativeMethods.mono_method_desc_search_in_image(monoassemblydesc, monoassemblygetimage);
            var monoinvoke = NativeMethods.mono_runtime_invoke(monomethodcall, IntPtr.Zero, IntPtr.Zero, IntPtr.Zero);
        }
    }
}
