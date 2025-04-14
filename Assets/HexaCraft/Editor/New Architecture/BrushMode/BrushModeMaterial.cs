using UnityEngine;
using UnityEditor;

namespace HexaCraft
{
    public class BrushModeMaterial : BrushMode
    {
        Material _mat;

        float _brushSize;

        internal new static class Styles
        {
            internal static GUIContent s_GCBrushEffect = new GUIContent("Material", "Select the material for hextile.");
            internal static GUIContent s_BrsuhSize = new GUIContent("Brush Size");
        }

        public override void DrawGUI()
        {
            const float BRUSH_SIZE_MIN = 0.0f;
            const float BRUSH_SIZE_MAX = 1.0f;

            _mat = HexaGUILayout.MaterialField(Styles.s_GCBrushEffect, _mat);
            // _brushSize = EditorGUILayout.Slider(_brushSize, BRUSH_SIZE_MIN, BRUSH_SIZE_MAX);
            _brushSize = HexaGUILayout.FreeSlider(Styles.s_BrsuhSize, _brushSize, BRUSH_SIZE_MIN, BRUSH_SIZE_MAX);
        }

        public override void RegisterUndo()
        {

        }
    }
}
