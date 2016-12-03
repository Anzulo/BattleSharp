using UnityEngine;

namespace BattleSharpControllerGenericNamespace
{
    public class DisableFog : MonoBehaviour
    {
        Controller.GameStart GameStart;
        public void Start()
        {
            GameStart = new Controller.GameStart(DisableFogOfWar);
        }
        public void OnEnable()
        {
            Loader.Controller.OnMatchStart += GameStart;
            DisableFogOfWar(true);
        }
        public void OnDisable()
        {
            Loader.Controller.OnMatchStart -= GameStart;
            DisableFogOfWar(false);
        }
        public void DisableFogOfWar()
        {
            DisableFogOfWar(true);
        }
        public void DisableFogOfWar(bool enable)
        {
            Loader.Controller.SetGameState<bool>(Loader.Controller.ActiveCamera.ObjectId, "DisableFogOfWar", enable);
        }
        public void Update()
        {
            if (Loader.Controller.GetGameState(Loader.Controller.ActiveCamera.ObjectId, "DisableFogOfWar") == false)
                Loader.Controller.SetGameState<bool>(Loader.Controller.ActiveCamera.ObjectId, "DisableFogOfWar", true);
        }
#if DEBUG
        public void OnGUI()
        {
            GUI.Label(new Rect(0, 61, 800, 25), "fog : " + Loader.Controller.GetGameState(Loader.Controller.ActiveCamera.ObjectId, "DisableFogOfWar"));
        }
#endif
    }
}
