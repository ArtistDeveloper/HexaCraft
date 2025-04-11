using UnityEngine;
using UnityEditor;

namespace HexaCraft
{
    public class BrushModeMaterial : BrushMode
    {
        Material _mat;

        internal new static class Styles
        {
            internal static GUIContent s_GCBrushEffect = new GUIContent("Material", "Select the material for hextile.");
        }

        public override void DrawGUI()
        {
            _mat = HexaGUILayout.MaterialField(Styles.s_GCBrushEffect, _mat);
        }

        public override void RegisterUndo()
        {

        }
    }
}
