using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace HexaCraft
{
    [InitializeOnLoad]
    public static class IconUtility
    {
        const string _iconPath = "Assets/HexaCraft/Content/Icons/";
        static Dictionary<string, Texture2D> _icons = new Dictionary<string, Texture2D>();

        public static Texture2D GetIcon(string iconName)
        {
            // return GetTextureInFolder(_iconPath, iconName);
            return GetTextureInFolder(_iconPath, iconName + ((EditorGUIUtility.pixelsPerPoint > 1f) ? "@2x" : ""));
        }

        public static Texture2D GetTextureInFolder(string folder, string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                return null;
            }

            int ext = name.LastIndexOf('.');
            string nameWithoutExtension = ext < 0 ? name : name.Substring(0, ext);
            Texture2D icon = null;

            if (!_icons.TryGetValue(nameWithoutExtension, out icon))
            {
                string fullPath = string.Format("{0}{1}.png", folder, nameWithoutExtension);

                icon = (Texture2D)AssetDatabase.LoadAssetAtPath(fullPath, typeof(Texture2D));

                if (icon == null)
                {
                    _icons.Add(nameWithoutExtension, null);
                    return null;
                }

                _icons.Add(nameWithoutExtension, icon);
            }

            return icon;
        }
    }
}