using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace HexaCraft
{
    // TODO: BrushSize에 따른 Gizmos 렌더링. 원형 모양의 Brush
    public class BrushModeMaterial : BrushMode
    {
        private Material _mat;

        private float _brushSize;

        private readonly HCPresenter _presenter;

        private Vector2 _mousePosition;

        private Ray _ray;

        private RaycastHit _hit;


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
            _brushSize = HexaGUILayout.FreeSlider(Styles.s_BrsuhSize, _brushSize, BRUSH_SIZE_MIN, BRUSH_SIZE_MAX);
        }

        public override void RegisterUndo()
        {

        }

        public override void OnBrushApply(GameObject target) // IEnumerable<GameObject> targets
        {
            Renderer renderer = target.GetComponent<Renderer>();
            if (renderer == null)
                return;

            var originalMaterials = renderer.sharedMaterials;
            Material[] newMaterials = new Material[originalMaterials.Length];

            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = _presenter.GetSelectionMaterial();
            }

            renderer.sharedMaterials = newMaterials;
        }

        // TODO: 마우스 커서 아이콘 브러쉬 size에 맞추어 원형으로 표시되게 제작
        // - 마우스 포인터가 Screen Coordinate에 있는게 아니라, 3D 평면에서 동작하도록. 즉, 기존 것은 비활성화되게 만들어야 할지도 모름
        // - Handles 클래스 참조
        // - Handles.DrawSolidDisc
        public override void DrawGizmos(bool isHoverHextile)
        {
            Rect cursorRect = new Rect(0, 0, Screen.width, Screen.height);
            if (isHoverHextile)
                EditorGUIUtility.AddCursorRect(cursorRect, MouseCursor.Pan);
            else
                EditorGUIUtility.AddCursorRect(cursorRect, MouseCursor.Arrow);
        }
    }
}
