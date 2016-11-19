using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Mono.Cecil;
using Mono.Cecil.Cil;

namespace BattleriteFogHack
{
    class Program
    {
        static void Main(string[] args)
        {
            MethodInfo OriginalUpdateModel = typeof(CharacterView).GetMethod("UpdateModel", BindingFlags.Public | BindingFlags.Instance);
            string dllPath = @"C:\Program Files (x86)\Steam\steamapps\common\Battlerite\Battlerite_Data\Managed\UnityContent.dll";
            if (!File.Exists(dllPath + ".orig"))
                File.Copy(dllPath, dllPath + ".orig");
            if (!File.Exists(dllPath + ".working"))
                File.Copy(dllPath, dllPath + ".working");
            AssemblyDefinition assembly = AssemblyDefinition.ReadAssembly(dllPath + ".working");
            TypeDefinition CharacterView = assembly.MainModule.Types.First(a => a.Name == "CharacterView");
            MethodDefinition UpdateModel = CharacterView.Methods.First(method => method.Name == "UpdateModel");
            if (UpdateModel.Body.Instructions[0].OpCode == OpCodes.Ldc_I4_1)
            {
                Console.WriteLine("Undo'ing Patch");
                File.Delete(dllPath);
                File.Move(dllPath + ".orig", dllPath);
            }
            else
            {
                Console.WriteLine("Patching DLL");
                ILProcessor worker = UpdateModel.Body.GetILProcessor();
                MethodReference UpdateModelReference = assembly.MainModule.ImportReference(OriginalUpdateModel);
                Instruction pushTrueToStrack = worker.Create(OpCodes.Ldc_I4_1);
                Instruction setIsModelVisibleToTry = worker.Create(OpCodes.Starg_S, UpdateModel.Parameters[2]);
                UpdateModel.Body.GetILProcessor().InsertBefore(UpdateModel.Body.Instructions[0], setIsModelVisibleToTry);
                UpdateModel.Body.GetILProcessor().InsertBefore(UpdateModel.Body.Instructions[0], pushTrueToStrack);
                assembly.Write(dllPath);
            }
            assembly.Dispose();
            File.Delete(dllPath + ".working");
            Console.WriteLine("Press enter to exit!");
            Console.ReadLine();
        }
    }
}
