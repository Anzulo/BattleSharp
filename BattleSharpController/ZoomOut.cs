using UnityEngine;

namespace BattleSharpControllerGenericNamespace
{
    public class ZoomOut : MonoBehaviour
    {
        public void Start()
        {
            Loader.Controller.OnMatchStart += MaximizeZoom;
            Menu.RootItems.Add(new MenuItem() { Text = "Zoom" });
        }
        public void OnDestroy()
        {
            Loader.Controller.OnMatchStart -= MaximizeZoom;
            ChangeMaxZoom(17.5f);
        }
        public void MaximizeZoom()
        {
            ChangeMaxZoom(100.0f);
        }
        public void ChangeMaxZoom(float zoomVal)
        {
            foreach (var current in Loader.Controller.unityMain.GetViewState().ActiveObjects)
            {
                if (current.TypeId == Loader.Controller.unityMain.GetViewState().ActiveCameraModePresetType)
                {
                    Loader.Controller.SetState.MakeGenericMethod(typeof(float)).Invoke(Loader.Controller.game, new object[] { current.ObjectId, "MaxZoom", zoomVal });
                    break;
                }
            }
        }
    }
}

