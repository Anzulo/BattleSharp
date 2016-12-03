using System.Reflection;
using UnityEngine;

namespace BattleSharpControllerGenericNamespace
{
    public class Timers : MonoBehaviour
    {
        public FieldInfo effectStateField;
        public void Start()
        {
            effectStateField = typeof(Gameplay.View.EffectState).GetField("#a", Loader.Controller.flags);
        }
        public void OnGUI()
        {
            foreach (var current in Loader.Controller.unityMain.GetViewState().ActiveObjects)
            {
                if (current.TypeId.ToString() == "RiteOfBonesSpawner" || current.TypeId.ToString() == "HealthShardSpawner" || current.TypeId.ToString() == "EnergyShardSpawner")
                {
                    var age = Loader.Controller.GetGameState(current.ObjectId, "Age");
                    var effectState = (Gameplay.View.EffectState)Loader.Controller.GetGameState(current.ObjectId, "EffectState");
                    var effectStateValue = (StunShared.Optional<Gameplay.View.EffectInstanceId>)effectStateField.GetValue(effectState);
                    foreach (var prefabState in Loader.Controller.prefabStates)
                    {
                        if (prefabState.Data.CreatorID == effectStateValue.Value)
                        {
                            var screenPos = Camera.main.WorldToScreenPoint(prefabState.Instance.transform.position);
                            var respawnTime = 25.0f;
                            if (current.TypeId.ToString() == "RiteOfBonesSpawner")
                                respawnTime = 20.0f;
                            GUI.Label(new Rect(screenPos.x, Screen.height - screenPos.y, 30, 30), (respawnTime - age).ToString("0.0"));
                        }
                    }
                }
            }
        }
    }
}
