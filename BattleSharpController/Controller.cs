using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;
using UnityEngine;
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
        public static MethodInfo GetActiveEntityData;
        public static UnityCore.Core core;
        public static Boolean targetLock = false;
        public static Int32 targetSelect = 0;
        public static List<String> targetSelector = new List<String>();
        public static Int32 xOff = 0;
        public static Int32 yOff = 0;

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1) || Input.GetKeyDown(KeyCode.Alpha1))
                targetSelect = 0;
            if (Input.GetKeyDown(KeyCode.Keypad2) || Input.GetKeyDown(KeyCode.Alpha2))
                targetSelect = 1;
            if (Input.GetKeyDown(KeyCode.Keypad3) || Input.GetKeyDown(KeyCode.Alpha3))
                targetSelect = 2;
            if (Input.GetKeyDown(KeyCode.Keypad4) || Input.GetKeyDown(KeyCode.Alpha4))
                targetSelect = 3;
            if (Input.GetKeyDown(KeyCode.Keypad5) || Input.GetKeyDown(KeyCode.Alpha5))
                targetSelect = 4;
            if (Input.GetKeyDown(KeyCode.Keypad6) || Input.GetKeyDown(KeyCode.Alpha6))
                targetSelect = 5;
            if (Input.GetKeyDown(KeyCode.BackQuote))
                targetLock = !targetLock;
            
            targetSelector.Clear();
            BloodGUI_Binding.HUD.UI_PlayersInfoBinding _PlayersInfoBinding = (BloodGUI_Binding.HUD.UI_PlayersInfoBinding)hudBase.GetType().GetField("_PlayersInfoBinding", Loader.flags).GetValue(hudBase);
            List<BloodGUI.Data_PlayerInfo> _TempList = (List<BloodGUI.Data_PlayerInfo>)_PlayersInfoBinding.GetType().GetField("_TempList", Loader.flags).GetValue(_PlayersInfoBinding);
            foreach (var hero in _TempList)
                targetSelector.Add(hero.Name.Value + "`" + hero.ID.ToString());

            if (targetLock)
            {
                var target = unityMain.GetViewState().Models.Find(t => t.Id.ToString() == targetSelector[targetSelect].Split(new char[1] { '`' }, StringSplitOptions.RemoveEmptyEntries)[1]);
                StunShared.Optional<Gameplay.View.ActiveObject> aed2 = (StunShared.Optional<Gameplay.View.ActiveObject>)GetActiveEntityData.Invoke(unityMain, new object[] { target.Id });
                Vector3 screenpoint = unityMain.GameplayCamera.Camera.WorldToScreenPoint(new Vector3(aed2.Value.Position.X, 0, aed2.Value.Position.Y));

                Point p;
                GetCursorPos(out p);
                xOff = p.X - (int)Input.mousePosition.x;
                yOff = p.Y - (int)(Screen.height - Input.mousePosition.y);
                SetCursorPos((int)(screenpoint.x + xOff), (int)(Screen.height - screenpoint.y + yOff));
            }
        }
        public void OnGUI()
        {
            int i = 0;
            GUI.Label(new Rect(100.0f, 15.0f, 1000, 100), "Targeted : " + targetSelector[targetSelect].Split(new char[1] { '`' }, StringSplitOptions.RemoveEmptyEntries)[0] + ", toggle with `");
            foreach (var availableTarget in targetSelector)
                GUI.Label(new Rect(100.0f, 30 + 15 * i++, 1000, 100), "Press " + i.ToString() + " to change to " + availableTarget.Split(new char[1] { '`' }, StringSplitOptions.RemoveEmptyEntries)[0]);
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct Point
        {
            public int X;
            public int Y;
        }
        [DllImport("user32.dll")]
        public static extern bool GetCursorPos(out Point lpPoint);
        [DllImport("user32.dll")]
        public static extern bool SetCursorPos(int X, int Y);
    }
}
