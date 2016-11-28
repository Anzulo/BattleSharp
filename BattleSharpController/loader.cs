using System.Reflection;
using UnityEngine;
using BloodGUI_Binding.Base;

namespace BattleSharpController
{
    public static class Loader
    {
        public static GameObject obj;
        public static void Load()
        {
            //UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            obj = new GameObject();
            obj.name = "BattleSharp";
            obj.AddComponent<Menu>();
            obj.AddComponent<Controller>();
            GameObject.DontDestroyOnLoad(obj);
        }
    }
}
