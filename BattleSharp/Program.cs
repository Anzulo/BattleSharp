using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Threading;
using Microsoft.CSharp;

namespace BattleSharp
{
    class Program
    {
        static string steamPath = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam").GetValue("SteamPath").ToString();
        static string gamePath = steamPath + @"\steamapps\common\Battlerite\Battlerite_Data\Managed\";
        static string exePath = Path.GetDirectoryName(Assembly.GetAssembly(typeof(Program)).Location);

        static void Main(string[] args)
        {
            RemoveBreadcrumbs();
            var dll = CompileDll();
            if (dll.Errors.Count > 0)
            {
                Console.WriteLine("Errors in Compiling!");
            }
            else
            {
                var battlerite = Process.GetProcessesByName("Battlerite");
                if (battlerite.Length == 0)
                    Console.WriteLine("Waiting for Battlerite to open...");
                while (battlerite.Length == 0)
                {
                    Thread.Sleep(5000);
                    battlerite = Process.GetProcessesByName("Battlerite");
                }
                Console.WriteLine("Injecting into Battlerite");
                UnityBootstrapper.Interface.Inject(battlerite.First().Id, new UnityBootstrapper.Interface(), exePath + @"\" + dll.PathToAssembly, Path.GetFileNameWithoutExtension(dll.PathToAssembly) + ".Loader:Load()");
            }
        }
        private static void RemoveBreadcrumbs()
        {
            var oldDlls = Directory.GetFiles(@".\", "aa*.dll", SearchOption.AllDirectories);
            var oldPdbs = Directory.GetFiles(@".\", "aa*.pdb", SearchOption.AllDirectories);
            var filesToRemove = oldDlls.Concat(oldPdbs);
            if (filesToRemove.Count() > 0)
                Console.WriteLine("Removing old artifacts");
            foreach (var file in filesToRemove)
            {
                try
                {
                    File.Delete(file);
                }
                catch { }
            }
        }
        private static CompilerResults CompileDll()
        {
            Console.WriteLine("Generating new DLL");
            var csprojPath = @"..\BattleSharpController\";
            if (!File.Exists(csprojPath + "BattleSharpController.csproj"))
            {
                Console.WriteLine("Getting latest version info from Github");
                System.Net.WebClient wc = new System.Net.WebClient();
                string hash = wc.DownloadString("https://github.com/shalzuth/BattleSharp");
                string hashSearch = "\"commit-tease-sha\" href=\"/shalzuth/BattleSharp/commit/";
                hash = hash.Substring(hash.IndexOf(hashSearch) + hashSearch.Length, 7);
                string hashFile = @".\BattleSharp-master\hash.txt";
                csprojPath = @".\BattleSharp-master\BattleSharpController";
                if (File.Exists(hashFile))
                {
                    if (hash != File.ReadAllText(hashFile))
                    {
                        Console.WriteLine("Later version exists, removing existing version");
                        Directory.Delete(@".\BattleSharp-master", true);
                    }
                }
                if (!File.Exists(hashFile))
                {
                    Console.WriteLine("Downloading latest version");
                    wc.DownloadFile("https://github.com/shalzuth/BattleSharp/archive/master.zip", "BattleSharpSource.zip");
                    using (var archive = ZipFile.OpenRead("BattleSharpSource.zip"))
                    {
                        archive.ExtractToDirectory(@".\");
                    }
                    File.WriteAllText(hashFile, hash);
                }
            }
            var randString = "aa" + Guid.NewGuid().ToString().Substring(0, 8);

            var codeProvider = new CSharpCodeProvider();
            var compilerParameters = new CompilerParameters
            {
                GenerateExecutable = false,
#if DEBUG
                IncludeDebugInformation = true,
#endif
                GenerateInMemory = false,
                OutputAssembly = randString + ".dll"
            };
            compilerParameters.ReferencedAssemblies.Add("mscorlib.dll");
            compilerParameters.ReferencedAssemblies.Add(gamePath + "System.dll");
            compilerParameters.ReferencedAssemblies.Add(gamePath + "System.Core.dll"); // not sure which to use... prefer Linq...
            compilerParameters.ReferencedAssemblies.Add("System.Drawing.dll");
            compilerParameters.ReferencedAssemblies.Add("System.Windows.Forms.dll");
            compilerParameters.ReferencedAssemblies.Add(gamePath + "UnityEngine.dll");
            compilerParameters.ReferencedAssemblies.Add(gamePath + "UnityEngine.UI.dll");
            compilerParameters.ReferencedAssemblies.Add(gamePath + "UnityContent.dll");
            compilerParameters.ReferencedAssemblies.Add(gamePath + "MergedShared.dll");
            compilerParameters.ReferencedAssemblies.Add(gamePath + "MergedUnity.dll");

            var sourceFiles = Directory.GetFiles(csprojPath, "*.cs", SearchOption.AllDirectories);
            var sources = new List<String>();
            foreach (var sourceFile in sourceFiles)
            {
                var source = File.ReadAllText(sourceFile);
                source = source.Replace("BattleSharpControllerGenericNamespace", randString);
                sources.Add(source);
            }
            Console.WriteLine("Compiling DLL");
            var result = codeProvider.CompileAssemblyFromSource(compilerParameters, sources.ToArray());
            return result;
        }
    }
}
