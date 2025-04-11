using UnityEngine;
using UnityEditor;
using System.Linq.Expressions;

namespace HexaCraft
{
    public static class HexaGUILayout
    {
        public static Material MaterialField(GUIContent gc, Material value)
        {
            using (new GUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField(gc, GUILayout.Width(140));
                Material ret = EditorGUILayout.ObjectField(value, typeof(Material), false) as Material;

                return ret;
            }
        }
    }
}