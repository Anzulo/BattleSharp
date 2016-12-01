using UnityEngine;

namespace BattleSharpControllerGenericNamespace
{
    public static class Loader
    {
        public static GameObject BaseObject;
        public static Controller Controller;
        public static void Load()
        {
            GameObject oldBaseObject;
            while (oldBaseObject = GameObject.Find("BattleSharp"))
                GameObject.DestroyImmediate(oldBaseObject);
            BaseObject = new GameObject()
            {
                name = "BattleSharp"
            };
            BaseObject.AddComponent<Menu>();
            Controller = BaseObject.AddComponent<Controller>();
            BaseObject.AddComponent<DisableFog>();
            BaseObject.AddComponent<ZoomOut>();
            BaseObject.AddComponent<Timers>();
            BaseObject.AddComponent<TargetLock>();
            GameObject.DontDestroyOnLoad(BaseObject);
        }
    }
}