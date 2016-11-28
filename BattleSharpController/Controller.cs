using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using Forms = System.Windows.Forms;
using Gameplay.GameObjects;
using UnityEngine;
using UnityMain;
using BloodGUI_Binding.Base;

namespace BattleSharpController
{
    public class Controller : MonoBehaviour
    {
        public static UI_MainBinding uiMainBinding;
        public static UI_BloodgateBase uiBloodgateBase;
        public static BloodgateSceneManager bloodgateSceneManager;
        public static BloodgateModelPool bloodgateModelPool;
        public static List<object> currentModels;
        public static GameObject effects;
        public static UnityMain.Main unityMain;
        public static UI_HUDBase hudBase;
        public static UnityCore.Core core;
        public static Gameplay.View.ViewState viewState;
        
        public static BindingFlags flags = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static;

        public void Awake()
        {
            uiMainBinding = UI_MainBinding.Instance;
            uiBloodgateBase = (UI_BloodgateBase)uiMainBinding.GetType().GetField("_BloodgateBase", flags).GetValue(uiMainBinding);
            hudBase = (UI_HUDBase)uiMainBinding.GetType().GetField("_HUDBase", flags).GetValue(uiMainBinding);
            bloodgateSceneManager = (BloodgateSceneManager)uiBloodgateBase.GetType().GetField("_BloodgateSceneManager", flags).GetValue(uiBloodgateBase);
            //Controller.currentModels = (List<object>)Controller.bloodgateSceneManager.GetType().GetField("_CurrentModels", flags).GetValue(Controller.bloodgateSceneManager);
            bloodgateModelPool = BloodgateModelPool.Instance;
            unityMain = GameObject.Find("UnityMain(Clone)").GetComponent<UnityMain.Main>();
            core = GameObject.Find("Core").GetComponent<UnityCore.Core>();
            viewState = unityMain.GetViewState();

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
        public static void TargetEnemy(Boolean active, Vector3 pos)
        {
            var viewportPos = Camera.main.WorldToViewportPoint(pos);
            if (viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1 && viewportPos.z > 0)
            {
                var screenPos = Camera.main.WorldToScreenPoint(pos);
                Forms.Cursor.Position = new Point((int)(screenPos.x + Forms.Cursor.Position.X - (int)Input.mousePosition.x),
                    (int)(Forms.Cursor.Position.Y - screenPos.y + (int)Input.mousePosition.y));
            }
        }
        public static void SetCursorFromGamePos(Vector3 pos)
        {
            var viewportPos = Camera.main.WorldToViewportPoint(pos);
            if (viewportPos.x > 0 && viewportPos.x < 1 && viewportPos.y > 0 && viewportPos.y < 1 && viewportPos.z > 0)
            {
                var screenPos = Camera.main.WorldToScreenPoint(pos);
                Forms.Cursor.Position = new Point((int)(screenPos.x + Forms.Cursor.Position.X - (int)Input.mousePosition.x),
                    (int)(Forms.Cursor.Position.Y - screenPos.y + (int)Input.mousePosition.y));
            }
        }

        public static BloodGUI.Data_PlayerInfo GetClosestEnemy()
        {
            return GetClosestEnemy(out Single distance);
        }
        public static BloodGUI.Data_PlayerInfo GetClosestEnemy(out Single distance)
        {
            distance = Single.MaxValue;
            var _PlayersInfoBinding = (BloodGUI_Binding.HUD.UI_PlayersInfoBinding)hudBase.GetType().GetField("_PlayersInfoBinding", flags).GetValue(hudBase);
            var _LocalTeamData = (List<BloodGUI.Data_PlayerInfo>)_PlayersInfoBinding.GetType().GetField("_LocalTeamData", flags).GetValue(_PlayersInfoBinding);
            var LocalPlayer = _LocalTeamData.Find(p => p.ID.ToString() == viewState.LookAtObject.ToString());
            BloodGUI.Data_PlayerInfo closest = LocalPlayer;
            var _EnemyTeamData = (List<BloodGUI.Data_PlayerInfo>)_PlayersInfoBinding.GetType().GetField("_EnemyTeamData", flags).GetValue(_PlayersInfoBinding);
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
        public static BloodGUI.Data_PlayerInfo GetClosestPlayer(out Single distance)
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
            var _PlayersInfoBinding = (BloodGUI_Binding.HUD.UI_PlayersInfoBinding)hudBase.GetType().GetField("_PlayersInfoBinding", flags).GetValue(hudBase);
            var _LocalTeamData = (List<BloodGUI.Data_PlayerInfo>)_PlayersInfoBinding.GetType().GetField("_LocalTeamData", flags).GetValue(_PlayersInfoBinding);
            var LocalPlayer = _LocalTeamData.Find(p => p.LocalPlayer);
            var targetEnemy = GetClosestEnemy();
            var hp = 0.0f;
            var targetAlly = LocalPlayer;
            foreach (var ally in _LocalTeamData)
            {
                if (hp < ally.Health.Value.Max - ally.Health.Value.Current)
                {
                    targetAlly = ally;
                    hp = ally.Health.Value.Max - ally.Health.Value.Current;
                }
            }
            if (LocalPlayer.CharacterIcon.name.Contains("alchemist"))
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
                    SetCursorFromGamePos(targetEnemy.Position());
                if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(1))
                    SetCursorFromGamePos(targetAlly.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("glutton"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.Space))
                    SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("gunner"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
                    SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("igniter"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.Space))
                    SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("inquisitor"))
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F))
                    SetCursorFromGamePos(targetEnemy.Position());
                if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.R))
                    SetCursorFromGamePos(targetAlly.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("vanguard"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.Space))
                    SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("astronomer"))
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.R))
                    SetCursorFromGamePos(targetEnemy.Position());
                if (Input.GetMouseButton(1))
                    SetCursorFromGamePos(targetAlly.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("engineer"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) ||Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.R))
                    SetCursorFromGamePos(targetEnemy.Position());
                if (Input.GetKey(KeyCode.Q))
                    SetCursorFromGamePos(targetAlly.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("harbinger"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Space))
                    SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("herald"))
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R))
                    SetCursorFromGamePos(targetEnemy.Position());
                if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.R))
                    SetCursorFromGamePos(targetAlly.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("inhibitor"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
                    SetCursorFromGamePos(targetEnemy.Position());
                if (Input.GetKey(KeyCode.Space))
                    SetCursorFromGamePos(targetAlly.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("nomad"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
                    SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("psychopomp"))
            {
                if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R))
                    SetCursorFromGamePos(targetEnemy.Position());
                if (Input.GetMouseButton(1))
                    SetCursorFromGamePos(targetAlly.Position());
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.F))
                {
                    var target = GetClosestPlayer(out Single d);
                    SetCursorFromGamePos(target.Position());
                }
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("ranid"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F))
                    SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("ravener"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
                    SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("seeker"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.Space))
                    SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("spearmaster"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R))
                    SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (LocalPlayer.CharacterIcon.name.Contains("stormcaller"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
                    SetCursorFromGamePos(targetEnemy.Position());
            }
        }
        public void OnGUI()
        {
        }
    }
}
