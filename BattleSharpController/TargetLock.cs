using System;
using UnityEngine;

namespace BattleSharpControllerGenericNamespace
{
    public class TargetLock : MonoBehaviour
    {
        public void Update()
        {
            if (Input.GetKey(KeyCode.LeftAlt))
                return;
            var targetEnemy = Loader.Controller.GetClosestEnemy();
            var hp = 0.0f;
            var targetAlly = Loader.Controller.LocalPlayer;
            foreach (var ally in Loader.Controller._LocalTeamData)
            {
                if (hp < ally.Health.Value.Max - ally.Health.Value.Current)
                {
                    targetAlly = ally;
                    hp = ally.Health.Value.Max - ally.Health.Value.Current;
                }
            }
            if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("alchemist"))
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
                if (Input.GetKey(KeyCode.Space) || Input.GetMouseButton(1))
                    Loader.Controller.SetCursorFromGamePos(targetAlly.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("glutton"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.Space))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("gunner"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("igniter"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.Space))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("inquisitor"))
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
                if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.R))
                    Loader.Controller.SetCursorFromGamePos(targetAlly.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("vanguard"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.Space))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("astronomer"))
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.R))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
                if (Input.GetMouseButton(1))
                    Loader.Controller.SetCursorFromGamePos(targetAlly.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("engineer"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.R))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
                if (Input.GetKey(KeyCode.Q))
                    Loader.Controller.SetCursorFromGamePos(targetAlly.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("harbinger"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.Space))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("herald"))
            {
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
                if (Input.GetMouseButton(1) || Input.GetKey(KeyCode.R))
                    Loader.Controller.SetCursorFromGamePos(targetAlly.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("inhibitor"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
                if (Input.GetKey(KeyCode.Space))
                    Loader.Controller.SetCursorFromGamePos(targetAlly.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("nomad"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("psychopomp"))
            {
                if (Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
                if (Input.GetMouseButton(1))
                    Loader.Controller.SetCursorFromGamePos(targetAlly.Position());
                if (Input.GetMouseButton(0) || Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.F))
                {
                    var target = Loader.Controller.GetClosestPlayer();
                    Loader.Controller.SetCursorFromGamePos(target.Position());
                }
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("ranid"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.F))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("ravener"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("seeker"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.Space))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("spearmaster"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
            }
            else if (Loader.Controller.LocalPlayer.CharacterIcon.name.Contains("stormcaller"))
            {
                if (Input.GetMouseButton(0) || Input.GetMouseButton(1) || Input.GetKey(KeyCode.E) || Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.F))
                    Loader.Controller.SetCursorFromGamePos(targetEnemy.Position());
            }
        }
#if DEBUG
        public void OnGUI()
        {
            GUI.Label(new Rect(0, 43, 800, 25), "target : " + Loader.Controller.GetClosestEnemy().Position());
        }
#endif
    }
}
