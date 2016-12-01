namespace BattleSharpControllerGenericNamespace
{
    public static class Extensions
    {
        public static UnityEngine.Vector3 Position(this BloodGUI.Data_PlayerInfo playerInfo)
        {
            return Loader.Controller.unityMain.GetObjectPosition(new Gameplay.GameObjects.GameObjectId(playerInfo.ID.Index, playerInfo.ID.Generation));
        }
    }
}