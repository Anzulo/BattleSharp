using UnityEngine;

namespace BattleSharpControllerGenericNamespace
{
    public class DisableFog : MonoBehaviour
    {
        public void Start()
        {
            Loader.Controller.OnMatchStart += DisableFogOfWar;
            Menu.RootItems.Add(new MenuItem() { Text = "Fog" });
        }
        public void OnDestroy()
        {
            Loader.Controller.OnMatchStart -= DisableFogOfWar;
            DisableFogOfWar(false);
        }
        public void DisableFogOfWar()
        {
            DisableFogOfWar(true);
        }
        public void DisableFogOfWar(bool enable)
        {
            foreach (var current in Loader.Controller.unityMain.GetViewState().ActiveObjects)
            {
                if (current.TypeId == Loader.Controller.unityMain.GetViewState().ActiveCameraModePresetType)
                {
                    Loader.Controller.SetState.MakeGenericMethod(typeof(bool)).Invoke(Loader.Controller.game, new object[] { current.ObjectId, "DisableFogOfWar", enable });
                    break;
                }
            }
        }
    }
}