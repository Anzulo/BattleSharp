using System;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;
using EasyHook;

namespace UnityBootstrapper
{
    public class EntryPoint : IEntryPoint
    {
        public static string dll = Path.GetDirectoryName(Assembly.GetAssembly(typeof(EntryPoint)).Location) + @"\BattleSharpController.dll";
        public static string namespaceclassmethod = "BattleSharpController.Loader:Load()";
        readonly Interface _interface;

        public EntryPoint(RemoteHooking.IContext InContext, string InChannelName)
        {
            _interface = RemoteHooking.IpcConnectClient<Interface>(InChannelName);
        }
        public void Run(RemoteHooking.IContext InContext, String InChannelName)
        {
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) => GetType().Assembly.FullName == args.Name ? GetType().Assembly : null;
            var monorootdomain = mono_get_root_domain();
            mono_thread_attach(monorootdomain);
            mono_security_set_mode(0);
            var monodomain = mono_domain_get();
            var monoassembly = mono_domain_assembly_open(monorootdomain, dll);
            var monoassemblygetimage = mono_assembly_get_image(monoassembly);
            var monoassemblydesc = mono_method_desc_new(namespaceclassmethod, false);
            var monomethodcall = mono_method_desc_search_in_image(monoassemblydesc, monoassemblygetimage);
            var monoinvoke = mono_runtime_invoke(monomethodcall, 0, 0, 0);
        }

        private const string monoDll = "mono.dll";
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr mono_get_root_domain();
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr mono_thread_attach(IntPtr monoRootDomain);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void mono_security_set_mode(Int32 monoRootDomain);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr mono_domain_get();
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr mono_domain_assembly_open(IntPtr monoDomain, String assembly);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr mono_assembly_get_image(IntPtr monoDomainAssembly);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr mono_class_from_name(IntPtr monoAssemblyImage, String nameSpaceLoad, String classLoad);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr mono_class_get_method_from_name(IntPtr monoClassName, String methodCall, String level);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr mono_runtime_invoke(IntPtr monoMethod, Int32 level, Int32 unk, Int32 unk2);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr mono_method_desc_new(String assemblyMethod, Boolean val);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern IntPtr mono_method_desc_search_in_image(IntPtr description, IntPtr image);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern void mono_jit_parse_options(Int32 args, String[] assembly);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        private static extern void mono_debug_init(Int32 format);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void mono_trace_set_level_string(String type);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        private static extern void mono_trace_set_mask_string(String type);
    }
}
