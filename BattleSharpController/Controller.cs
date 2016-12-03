using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Reflection;
using Forms = System.Windows.Forms;
using UnityEngine;
using UnityMain;
using BloodGUI_Binding.Base;
using Gameplay;
using Gameplay.GameObjects;
using Gameplay.View;

namespace BattleSharpControllerGenericNamespace
{
    public class Controller : MonoBehaviour
    {
        public UI_MainBinding uiMainBinding;
        public UI_BloodgateBase uiBloodgateBase;
        public BloodgateSceneManager bloodgateSceneManager;
        public BloodgateModelPool bloodgateModelPool;
        public List<object> currentModels;
        public GameObject effects;
        public Main unityMain;
        public UI_HUDBase hudBase;
        public UnityCore.Core core;
        public ViewState viewState;
        public EffectSystem.PrefabInstanceSystem prefabInstance;
        public List<EffectSystem.PrefabInstanceSystem.PrefabInstanceState> prefabStates;
        public BloodGUI.Bloodgate.UI_BloodgateChatBindings chat;
        public IBloodgate server;

        public object game;
        public MethodInfo GetState = null;
        public MethodInfo SetState = null;

        public BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        public delegate void GameStart();
        public GameStart OnMatchStart;
        public Boolean InGame = false;

        public BloodGUI_Binding.HUD.UI_PlayersInfoBinding _PlayersInfoBinding;
        public List<BloodGUI.Data_PlayerInfo> _EnemyTeamData;
        public List<BloodGUI.Data_PlayerInfo> _LocalTeamData;
        public BloodGUI.Data_PlayerInfo LocalPlayer
        {
            get { return _LocalTeamData.Find(p => p.LocalPlayer == true); }
        }
        public ActiveObject ActiveCamera
        {
            get { return unityMain.GetViewState().ActiveObjects.Find(c => c.TypeId == unityMain.GetViewState().ActiveCameraModePresetType); }
        }
        public void GameStartInit()
        {
            core = GameObject.Find("Core").GetComponent<UnityCore.Core>();
            uiMainBinding = UI_MainBinding.Instance;
            uiBloodgateBase = (UI_BloodgateBase)uiMainBinding.GetType().GetField("_BloodgateBase", flags).GetValue(uiMainBinding);
            hudBase = (UI_HUDBase)uiMainBinding.GetType().GetField("_HUDBase", flags).GetValue(uiMainBinding);
            //bloodgateSceneManager = (BloodgateSceneManager)uiBloodgateBase.GetType().GetField("_BloodgateSceneManager", flags).GetValue(uiBloodgateBase);
            //server = (IBloodgate)uiBloodgateBase.GetType().GetField("_Bloodgate", flags).GetValue(uiBloodgateBase);
            //var bottomBar = (UI_BloodgateBottomBar)uiBloodgateBase.GetType().GetField("_BottomBar", flags).GetValue(uiBloodgateBase);
            //chat = (BloodGUI.Bloodgate.UI_BloodgateChatBindings)bottomBar.GetType().GetField("_ChatBindings", flags).GetValue(bottomBar);

            _PlayersInfoBinding = (BloodGUI_Binding.HUD.UI_PlayersInfoBinding)hudBase.GetType().GetField("_PlayersInfoBinding", flags).GetValue(hudBase);
            _EnemyTeamData = (List<BloodGUI.Data_PlayerInfo>)_PlayersInfoBinding.GetType().GetField("_EnemyTeamData", flags).GetValue(_PlayersInfoBinding);
            _LocalTeamData = (List<BloodGUI.Data_PlayerInfo>)_PlayersInfoBinding.GetType().GetField("_LocalTeamData", flags).GetValue(_PlayersInfoBinding);

            var viewSystems = unityMain.GetType().GetField("_Systems", flags).GetValue(unityMain);
            prefabInstance = (EffectSystem.PrefabInstanceSystem)viewSystems.GetType().GetField("PrefabInstance", flags).GetValue(viewSystems);
            prefabStates = (List<EffectSystem.PrefabInstanceSystem.PrefabInstanceState>)Loader.Controller.prefabInstance.GetType().GetField("_PrefabStates", Loader.Controller.flags).GetValue(Loader.Controller.prefabInstance);

            var GetGame = core.ClientInterface.GetType().GetMethod("GetGame", flags);
            game = GetGame.Invoke(core.ClientInterface, new object[] { });
        }
        public GameValue GetGameState(GameObjectId id, String key)
        {
            return (GameValue)GetState.Invoke(game, new object[] { id, key, false });
        }
        public void SetGameState<T>(GameObjectId id, String key, T value)
        {
            SetState.MakeGenericMethod(typeof(T)).Invoke(Loader.Controller.game, new object[] { id, key, value });
        }
        public void Start()
        {
            unityMain = GameObject.Find("UnityMain(Clone)").GetComponent<Main>();
            OnMatchStart = new GameStart(GameStartInit);
            Type t = Type.GetType("#3IN.#oG,MergedUnity");
            var gameMethods = t.GetMethods();
            GetState = gameMethods.First(m => m.Name == "GetState" && m.ReturnParameter.ParameterType.ToString().Contains("GameValue") && m.GetParameters()[1].ParameterType.ToString().Contains("String"));
            SetState = gameMethods.First(m => m.Name == "SetState" && m.GetParameters()[1].ParameterType.ToString().Contains("String"));
        }
        public void SetCursorFromGamePos(Vector3 pos)
        {
            var viewportPos = Camera.main.WorldToViewportPoint(pos);
            if (viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1 && viewportPos.z > 0)
            {
                var screenPos = Camera.main.WorldToScreenPoint(pos);
                Forms.Cursor.Position = new Point((int)(screenPos.x + Forms.Cursor.Position.X - (int)Input.mousePosition.x),
                    (int)(Forms.Cursor.Position.Y - screenPos.y + (int)Input.mousePosition.y));
            }
        }

        public BloodGUI.Data_PlayerInfo GetClosestEnemy()
        {
            Single distance = Single.MaxValue;
            BloodGUI.Data_PlayerInfo closest = LocalPlayer;
            foreach (var enemy in _EnemyTeamData)
            {
                if (enemy.IsDead)
                    continue;
                if (enemy.Shield > 4)
                    continue;
                var newDistance = Vector3.Distance(LocalPlayer.Position(), enemy.Position());
                if (newDistance < distance)
                {
                    distance = newDistance;
                    closest = enemy;
                }
            }
            return closest;
        }
        public BloodGUI.Data_PlayerInfo GetClosestPlayer()
        {
            Single distance = Single.MaxValue;
            var _PlayersInfoBinding = (BloodGUI_Binding.HUD.UI_PlayersInfoBinding)hudBase.GetType().GetField("_PlayersInfoBinding", flags).GetValue(hudBase);
            var _EnemyTeamData = (List<BloodGUI.Data_PlayerInfo>)_PlayersInfoBinding.GetType().GetField("_EnemyTeamData", flags).GetValue(_PlayersInfoBinding);
            var _LocalTeamData = (List<BloodGUI.Data_PlayerInfo>)_PlayersInfoBinding.GetType().GetField("_LocalTeamData", flags).GetValue(_PlayersInfoBinding);
            var LocalPlayer = _LocalTeamData.Find(p => p.LocalPlayer);
            BloodGUI.Data_PlayerInfo closest = LocalPlayer;
            foreach (var enemy in _EnemyTeamData)
            {
                if (enemy.IsDead)
                    continue;
                var newDistance = Vector3.Distance(LocalPlayer.Position(), enemy.Position());
                if (newDistance < distance)
                {
                    distance = newDistance;
                    closest = enemy;
                }
            }
            foreach (var player in _LocalTeamData)
            {
                if (player.IsDead)
                    continue;
                var newDistance = Vector3.Distance(LocalPlayer.Position(), player.Position());
                if (newDistance < distance)
                {
                    distance = newDistance;
                    closest = player;
                }
            }
            return closest;
        }
        int previousRound = 0;
        public void Update()
        {
            if (unityMain.GetViewState() != null && !unityMain.GetViewState().IsInLobby && !unityMain.GetViewState().IsLoading && !unityMain.GetViewState().IsInCinematic)
            {
                var inPractice = unityMain.GetViewState().Huds.Count(a => a.Name == "Arena") == 0;
                int currentRound = 0;
                if (inPractice)
                    currentRound = unityMain.GetViewState().Huds.Find(a => a.Name == "Arena").Data.Get("CurrentRound");
                if (InGame == false || currentRound != previousRound)
                {
                    InGame = true;
                    previousRound = currentRound;
                    OnMatchStart();
                }
            }
            else
            {
                if (InGame == true)
                {
                    InGame = false;
                    //if (OnMatchEnd != null)
                    //    OnMatchEnd();
                }
            }
        }
        public void OnGUI()
        {
#if DEBUG
            GUI.Label(new Rect(0, 0, 800, 25), "init : " + this.GetType().Namespace + " : " + previousRound);
#else
            GUI.Label(new Rect(0, 0, 200, 25), "BattleSharp by Shalzuth");
#endif
        }
    }
}
