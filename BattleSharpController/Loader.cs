using System.Collections.Generic;
using UnityEngine;

namespace BattleSharpControllerGenericNamespace
{
    public static class Loader
    {
        public static GameObject BaseObject;
        public static Controller Controller;
        public static List<string> Addons = new List<string>();
        public static void Load()
        {
            GameObject oldBaseObject;
            while (oldBaseObject = GameObject.Find("BattleSharp"))
                GameObject.DestroyImmediate(oldBaseObject);
            BaseObject = new GameObject() { name = "BattleSharp" };

            Controller = BaseObject.AddComponent<Controller>();

            BaseObject.AddComponent<DisableFog>();
            BaseObject.AddComponent<SpeedUp>();
            BaseObject.AddComponent<TargetLock>();
            BaseObject.AddComponent<Timers>();
            BaseObject.AddComponent<ZoomOut>();

            BaseObject.AddComponent<Menu>();
            GameObject.DontDestroyOnLoad(BaseObject);
        }
    }
}
