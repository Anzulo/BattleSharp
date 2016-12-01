using System;
using System.Collections.Generic;
using UnityEngine;

namespace BattleSharpControllerGenericNamespace
{
    public class MenuItem
    {
        public String Text = "";
        public List<MenuItem> Children = new List<MenuItem>();
        public Boolean Enabled = false;
    }
    public class Menu : MonoBehaviour
    {
        public static List<MenuItem> RootItems;
        int ySize = 30;
        int xSize = 150;

        void Awake()
        {
            RootItems = new List<MenuItem>();
        }
        void DisableSiblings(MenuItem item, List<MenuItem> siblings)
        {
            if (siblings.Contains(item))
            {
                foreach (var sibling in siblings)
                    if (sibling != item)
                        sibling.Enabled = false;
            }
            else
            {
                foreach (var sibling in siblings)
                    if (sibling.Children != null && sibling.Children.Count > 0)
                        DisableSiblings(item, sibling.Children);
            }

        }
        void DrawItem(MenuItem item, int yOffset, int xOffset)
        {
            if (GUI.Button(new Rect(xOffset++ * xSize, yOffset * ySize, xSize, ySize), item.Text))
            {
                DisableSiblings(item, RootItems);
                item.Enabled = !item.Enabled;
            }
            if (item.Enabled && item.Children != null && item.Children.Count > 0)
            {
                foreach (var child in item.Children)
                    DrawItem(child, yOffset++, xOffset);
            }
        }

        void OnGUI()
        {
            if (!Input.GetKey(KeyCode.LeftShift))
                return;
            int yOffset = 0;
            int xOffset = 0;
            foreach (var item in RootItems)
                DrawItem(item, yOffset++, xOffset);
        }
    }
}
