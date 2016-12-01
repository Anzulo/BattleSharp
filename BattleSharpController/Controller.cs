using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Forms = System.Windows.Forms;
using UnityEngine;
using UnityMain;
using BloodGUI_Binding.Base;

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
        public Gameplay.View.ViewState viewState;
        public EffectSystem.PrefabInstanceSystem prefabInstance;
        public List<EffectSystem.PrefabInstanceSystem.PrefabInstanceState> prefabStates;

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
        public BloodGUI.Data_PlayerInfo LocalPlayer;

        public void GameStartInit()
        {
            uiMainBinding = UI_MainBinding.Instance;
            uiBloodgateBase = (UI_BloodgateBase)uiMainBinding.GetType().GetField("_BloodgateBase", flags).GetValue(uiMainBinding);
            hudBase = (UI_HUDBase)uiMainBinding.GetType().GetField("_HUDBase", flags).GetValue(uiMainBinding);
            bloodgateSceneManager = (BloodgateSceneManager)uiBloodgateBase.GetType().GetField("_BloodgateSceneManager", flags).GetValue(uiBloodgateBase);
            core = GameObject.Find("Core").GetComponent<UnityCore.Core>();

            _PlayersInfoBinding = (BloodGUI_Binding.HUD.UI_PlayersInfoBinding)hudBase.GetType().GetField("_PlayersInfoBinding", flags).GetValue(hudBase);
            _EnemyTeamData = (List<BloodGUI.Data_PlayerInfo>)_PlayersInfoBinding.GetType().GetField("_EnemyTeamData", flags).GetValue(_PlayersInfoBinding);
            _LocalTeamData = (List<BloodGUI.Data_PlayerInfo>)_PlayersInfoBinding.GetType().GetField("_LocalTeamData", flags).GetValue(_PlayersInfoBinding);
            LocalPlayer = _LocalTeamData.Find(p => p.LocalPlayer);

            var viewSystems = unityMain.GetType().GetField("_Systems", flags).GetValue(unityMain);
            prefabInstance = (EffectSystem.PrefabInstanceSystem)viewSystems.GetType().GetField("PrefabInstance", flags).GetValue(viewSystems);
            prefabStates = (List<EffectSystem.PrefabInstanceSystem.PrefabInstanceState>)Loader.Controller.prefabInstance.GetType().GetField("_PrefabStates", Loader.Controller.flags).GetValue(Loader.Controller.prefabInstance);

            var GetGame = core.ClientInterface.GetType().GetMethod("GetGame", flags);
            game = GetGame.Invoke(core.ClientInterface, new object[] { });
            var gameMethods = game.GetType().GetMethods();

            foreach (var gameMethod in gameMethods)
            {
                if (gameMethod.Name == "GetState" && gameMethod.ReturnParameter.ParameterType.ToString().Contains("GameValue"))
                {
                    if (gameMethod.GetParameters()[1].ParameterType.ToString().Contains("String"))
                    {
                        GetState = gameMethod;
                    }
                }
                else if (gameMethod.Name == "SetState")
                {
                    if (gameMethod.GetParameters()[1].ParameterType.ToString().Contains("String"))
                    {
                        SetState = gameMethod;
                    }
                }
            }
        }
        public void Awake()
        {
            unityMain = GameObject.Find("UnityMain(Clone)").GetComponent<Main>();
            core = GameObject.Find("Core").GetComponent<UnityCore.Core>();
            OnMatchStart = new GameStart(GameStartInit);

            Menu.RootItems.Add(new MenuItem()
            {
                Text = "Common",
                Children = new List<MenuItem>()
                {
                    new MenuItem()
                    {
                        Text = "TargetSelect"
                    },
                    new MenuItem()
                    {
                        Text = "KeyBinding"
                    },
                    new MenuItem()
                    {
                        Text = "Enabled"
                    },
                }
            });
        }
        public void TargetEnemy(Boolean active, Vector3 pos)
        {
            var viewportPos = Camera.main.WorldToViewportPoint(pos);
            if (viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1 && viewportPos.z > 0)
            {
                var screenPos = Camera.main.WorldToScreenPoint(pos);
                Forms.Cursor.Position = new Point((int)(screenPos.x + Forms.Cursor.Position.X - (int)Input.mousePosition.x),
                    (int)(Forms.Cursor.Position.Y - screenPos.y + (int)Input.mousePosition.y));
            }
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
            Single distance;
            return GetClosestEnemy(out distance);
        }
        public BloodGUI.Data_PlayerInfo GetClosestEnemy(out Single distance)
        {
            distance = Single.MaxValue;
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
            return closest;
        }
        public BloodGUI.Data_PlayerInfo GetClosestPlayer(out Single distance)
        {
            distance = Single.MaxValue;
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

        public void Update()
        {
            viewState = unityMain.GetViewState();
            if (viewState != null && !viewState.IsInLobby && !viewState.IsLoading && !viewState.IsInCinematic)
            {
                if (InGame == false)
                {
                    InGame = true;
                    if (OnMatchStart != null)
                        OnMatchStart();
                }
            }
            else
            {
                if (InGame == true)
                {
                    InGame = false;
                    if (OnMatchStart != null)
                        OnMatchStart();
                }
            }
            return;
        }
        public void OnGUI()
        {
            GUI.Label(new Rect(0, 0, 200, 25), "BattleSharp by Shalzuth");
        }
    }
}