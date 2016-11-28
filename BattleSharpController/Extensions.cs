using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;

namespace BattleSharpController
{
    public static class Extensions
    {
        public static UnityEngine.Vector3 Position(this BloodGUI.Data_PlayerInfo playerInfo)
        {
            /*var target = Controller.unityMain.GetViewState().Models.Find(t => t.Id.ToString() == playerInfo.ID.ToString());
            StunShared.Optional<Gameplay.View.ActiveObject> aed2 = (StunShared.Optional<Gameplay.View.ActiveObject>)Controller.unityMain.GetType().GetMethod("GetActiveEntityData", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Static).Invoke(Controller.unityMain, new object[] { target.Id });
            return new UnityEngine.Vector3(aed2.Value.Position.X, 0, aed2.Value.Position.Y);*/
            return Controller.unityMain.GetObjectPosition(new Gameplay.GameObjects.GameObjectId(playerInfo.ID.Index, playerInfo.ID.Generation));
        }
    }
}
