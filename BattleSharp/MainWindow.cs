using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace BattleSharp
{
    public partial class MainWindow : Form
    {
        readonly UnityBootstrapper.Interface bootstrap = new UnityBootstrapper.Interface();

        static string steamPath = Microsoft.Win32.Registry.CurrentUser.OpenSubKey(@"Software\Valve\Steam").GetValue("SteamPath").ToString();
        static string gamePath = steamPath + @"\steamapps\common\Battlerite\Battlerite_Data\Managed\";

        public MainWindow()
        {
            InitializeComponent();
            var fow1 = DllPatched(gamePath + @"UnityContent.dll", "CharacterView", "UpdateModel", new List<Tuple<OpCode, string>>()
            {
                Tuple.Create(OpCodes.Ldc_I4_1,""),
                Tuple.Create(OpCodes.Starg_S,"Parameter2"),
            });
            var fow2 = DllPatched(gamePath + @"MergedUnity.dll", "FogOfWarEffect", "UpdateFogOfWar", new List<Tuple<OpCode, string>>()
            {
                Tuple.Create(OpCodes.Ret,""),
            });
            var fow = fow1 && fow2;
            var zoom = DllPatched(gamePath + @"MergedShared.dll", "MathHelper", "Clamp", new List<Tuple<OpCode, string>>()
            {
                Tuple.Create(OpCodes.Ldarg_2,""),
                Tuple.Create(OpCodes.Ldc_R4,"float17.5"),
                Tuple.Create(OpCodes.Bne_Un_S,"Instruction0"),
                Tuple.Create(OpCodes.Ldc_R4,"float50.0"),
                Tuple.Create(OpCodes.Starg_S,"Parameter2"),
            });
            dllMods.SetItemChecked(0, zoom);
            dllMods.SetItemChecked(1, fow);
        }

        private void injectButton_Click(object sender, EventArgs e)
        {
            UnityBootstrapper.Interface.Inject(Process.GetProcessesByName("Battlerite")[0].Id, bootstrap);
        }

        private bool DllPatched(String dllPath, String typeName, String methodName, List<Tuple<OpCode, String>> instructions)
        {
            bool patched = true;
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(dllPath);
            TypeDefinition typeDef = assembly.MainModule.Types.First(a => a.Name == typeName);
            MethodDefinition methodDef = typeDef.Methods.First(method => method.Name == methodName);
            ILProcessor worker = methodDef.Body.GetILProcessor();
            if (methodDef.Body.Instructions[0].OpCode != instructions[0].Item1)
                patched = false;
            assembly.Dispose();
            return patched;
        }
        private void PatchDll(String dllPath, String typeName, String methodName, List<Tuple<OpCode, String>> instructions)
        {
            if (!File.Exists(dllPath + ".orig"))
                File.Copy(dllPath, dllPath + ".orig");
            if (!File.Exists(dllPath + ".working"))
                File.Copy(dllPath, dllPath + ".working");
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(dllPath + ".working");
            TypeDefinition typeDef = assembly.MainModule.Types.First(a => a.Name == typeName);
            MethodDefinition methodDef = typeDef.Methods.First(method => method.Name == methodName);
            ILProcessor worker = methodDef.Body.GetILProcessor();
            var opCodeParams = new List<Instruction>();
            foreach (var instruction in instructions)
            {
                if (instruction.Item2 == "")
                    opCodeParams.Add(worker.Create(instruction.Item1));
                else if (instruction.Item2.StartsWith("Instruction"))
                    opCodeParams.Add(worker.Create(instruction.Item1, methodDef.Body.Instructions[UInt16.Parse(instruction.Item2.Replace("Instruction", ""))]));
                else if (instruction.Item2.StartsWith("Parameter"))
                    opCodeParams.Add(worker.Create(instruction.Item1, methodDef.Parameters[UInt16.Parse(instruction.Item2.Replace("Parameter", ""))]));
                else if (instruction.Item2.StartsWith("float"))
                    opCodeParams.Add(worker.Create(instruction.Item1, Single.Parse(instruction.Item2.Replace("float", ""))));
            }
            if (methodDef.Body.Instructions[0].OpCode != opCodeParams[0].OpCode)
            {
                opCodeParams.Reverse();
                foreach (var instruction in opCodeParams)
                    worker.InsertBefore(methodDef.Body.Instructions[0], instruction);
                assembly.Write(dllPath);
            }
            assembly.Dispose();
            File.Delete(dllPath + ".working");
        }
        private void UnpatchDll(String dllPath)
        {
            if (File.Exists(dllPath) && File.Exists(dllPath + ".orig"))
            {
                File.Delete(dllPath);
                File.Copy(dllPath + ".orig", dllPath);
            }
        }
        private void fogHackButton_Click(object sender, EventArgs e)
        {

        }

        private void dllMods_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (dllMods.SelectedIndex == 0)
            {
                if (e.NewValue == CheckState.Checked)
                {
                    PatchDll(gamePath + @"MergedShared.dll", "MathHelper", "Clamp", new List<Tuple<OpCode, string>>()
                    {
                        Tuple.Create(OpCodes.Ldarg_2,""),
                        Tuple.Create(OpCodes.Ldc_R4,"float17.5"),
                        Tuple.Create(OpCodes.Bne_Un_S,"Instruction0"),
                        Tuple.Create(OpCodes.Ldc_R4,"float50.0"),
                        Tuple.Create(OpCodes.Starg_S,"Parameter2"),
                    });
                }
                else
                {
                    UnpatchDll(gamePath + @"MergedShared.dll");
                }
            }
            else
            {
                if (e.NewValue == CheckState.Checked)
                {
                    PatchDll(gamePath + @"UnityContent.dll", "CharacterView", "UpdateModel", new List<Tuple<OpCode, string>>()
                    {
                        Tuple.Create(OpCodes.Ldc_I4_1,""),
                        Tuple.Create(OpCodes.Starg_S,"Parameter2"),
                    });
                    PatchDll(gamePath + @"MergedUnity.dll", "FogOfWarEffect", "UpdateFogOfWar", new List<Tuple<OpCode, string>>()
                    {
                        Tuple.Create(OpCodes.Ret,""),
                    });
                }
                else
                {
                    UnpatchDll(gamePath + @"UnityContent.dll");
                    UnpatchDll(gamePath + @"MergedUnity.dll");
                }
            }
        }
    }
}
