using UnityEngine;

namespace BattleSharpControllerGenericNamespace
{
    public class SpeedUp : MonoBehaviour
    {
        float defaultSpeedVal = 3.4f;
        float speedVal = 9.4f;
        public void Update()
        {
            if (Loader.Controller.GetGameState(Loader.Controller.LocalPlayer.ID.ToGame(), "MovementSpeed") != speedVal)
                Loader.Controller.SetGameState<float>(Loader.Controller.LocalPlayer.ID.ToGame(), "MovementSpeed", speedVal);
        }
        public void OnDisable()
        {
            Loader.Controller.SetGameState<float>(Loader.Controller.LocalPlayer.ID.ToGame(), "MovementSpeed", defaultSpeedVal);
        }

#if DEBUG
        public void OnGUI()
        {
            GUI.Label(new Rect(0, 25, 800, 25), "speed : " + Loader.Controller.GetGameState(Loader.Controller.LocalPlayer.ID.ToGame(), "MovementSpeed"));
        }
#endif
    }
}
