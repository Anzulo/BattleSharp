using UnityEngine;

namespace BattleSharpControllerGenericNamespace
{
    public class ZoomOut : MonoBehaviour
    {
        Controller.GameStart GameStart;
        float defaultMaxZoom = 17.5f;
        float maxZoom = 95;
        public void Start()
        {
            GameStart = new Controller.GameStart(MaximizeZoom);
        }
        public void OnEnable()
        {
            Loader.Controller.OnMatchStart += MaximizeZoom;
            MaximizeZoom();
        }
        public void OnDisable()
        {
            Loader.Controller.OnMatchStart -= MaximizeZoom;
            ChangeMaxZoom(defaultMaxZoom);
        }
        public void MaximizeZoom()
        {
            ChangeMaxZoom(maxZoom);
        }
        public void ChangeMaxZoom(float zoomVal)
        {
            Loader.Controller.SetGameState<float>(Loader.Controller.ActiveCamera.ObjectId, "MaxZoom", zoomVal);
        }
        public void Update()
        {
            if ((float)Loader.Controller.GetGameState(Loader.Controller.ActiveCamera.ObjectId, "MaxZoom") != maxZoom)
                Loader.Controller.SetGameState<float>(Loader.Controller.ActiveCamera.ObjectId, "MaxZoom", maxZoom);
        }
#if DEBUG
        public void OnGUI()
        {
            GUI.Label(new Rect(0, 79, 800, 25), "zoom : " + Loader.Controller.GetGameState(Loader.Controller.ActiveCamera.ObjectId, "MaxZoom"));
        }
#endif
    }
}
