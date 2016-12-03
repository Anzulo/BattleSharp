namespace BattleSharpControllerGenericNamespace
{
    public static class Extensions
    {
        public static UnityEngine.Vector3 Position(this BloodGUI.Data_PlayerInfo playerInfo)
        {
            return Loader.Controller.unityMain.GetObjectPosition(playerInfo.ID.ToGame());
        }
        public static Gameplay.GameObjects.GameObjectId ToGame(this BloodGUI.UIGameObjectId id)
        {
            return new Gameplay.GameObjects.GameObjectId(id.Index, id.Generation);
        }
    }
}
