using System;
using System.Runtime.InteropServices;

namespace UnityBootstrapper
{
    class NativeMethods
    {
        private const string monoDll = "mono.dll";
#pragma warning disable IDE1006 // Mono uses lower case function names
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr mono_get_root_domain();
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr mono_thread_attach(IntPtr monoRootDomain);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        internal static extern void mono_security_set_mode(Int32 securityMode);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr mono_domain_get();
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr mono_domain_assembly_open(IntPtr monoRootDomain, String assemblyPath);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr mono_assembly_get_image(IntPtr assembly);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr mono_runtime_invoke(IntPtr method, IntPtr obj, IntPtr paramsPtr, IntPtr excPtr);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr mono_method_desc_new(String assemblyMethod, Boolean includeNamespace);
        [DllImport(monoDll, CallingConvention = CallingConvention.Cdecl)]
        internal static extern IntPtr mono_method_desc_search_in_image(IntPtr description, IntPtr image);
#pragma warning restore IDE1006 // Naming Styles
    }
}
