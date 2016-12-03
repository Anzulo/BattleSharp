using UnityEngine;

namespace BattleSharpControllerGenericNamespace
{
    public class Menu : MonoBehaviour
    {
        Rect window;
        void Start()
        {
            window = new Rect(100, 15, 115, ((Loader.BaseObject.GetComponents<MonoBehaviour>().Length) * 18 - 9));
        }
        void OnGUI()
        {
            if (!Input.GetKey(KeyCode.LeftAlt))
                return;
            window = GUI.Window(0, window, ShowAllAddons, "BattleSharp");
        }
        void ShowAllAddons(int windowID)
        {
            int y = 1;
            foreach (var component in Loader.BaseObject.GetComponents<MonoBehaviour>())
            {
                if (component.GetType().Name == "Controller" || component.GetType().Name == "Menu")
                    continue;
                component.enabled = GUI.Toggle(new Rect(0, 18 * y++, 200, 20), component.enabled, component.GetType().Name);
            }
            GUI.DragWindow();
        }
    }
}
