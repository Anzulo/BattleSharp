using System.Reflection;
using UnityEngine;
using BloodGUI_Binding.Base;

namespace BattleSharpController
{
    public static class Loader
    {
        public static BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;
        public static GameObject obj;
        public static void Load()
        {
            Controller.uiMainBinding = UI_MainBinding.Instance;
            Controller.uiBloodgateBase = (UI_BloodgateBase)Controller.uiMainBinding.GetType().GetField("_BloodgateBase", flags).GetValue(Controller.uiMainBinding);
            Controller.hudBase = (UI_HUDBase)Controller.uiMainBinding.GetType().GetField("_HUDBase", flags).GetValue(Controller.uiMainBinding);
            Controller.bloodgateSceneManager = (BloodgateSceneManager)Controller.uiBloodgateBase.GetType().GetField("_BloodgateSceneManager", flags).GetValue(Controller.uiBloodgateBase);
            //Controller.currentModels = (List<object>)Controller.bloodgateSceneManager.GetType().GetField("_CurrentModels", flags).GetValue(Controller.bloodgateSceneManager);
            Controller.bloodgateModelPool = BloodgateModelPool.Instance;
            Controller.unityMain = GameObject.Find("UnityMain(Clone)").GetComponent<UnityMain.Main>();
            Controller.GetActiveEntityData = Controller.unityMain.GetType().GetMethod("GetActiveEntityData", flags);
            Controller.core = GameObject.Find("Core").GetComponent<UnityCore.Core>();
        
            /*var objs = GameObject.FindObjectsOfType<GameObject>();
            foreach (var obj in objs)
                if (obj.transform.parent == null)
                    if (obj.name == "Effects")
                        Controller.effects = obj;*/

            var old = GameObject.Find("Controller");
            if (old != null)
                GameObject.Destroy(old);
            obj = new GameObject();
            obj.name = "Controller";
            obj.AddComponent<Controller>();
            GameObject.DontDestroyOnLoad(obj);
        }
    }
}
