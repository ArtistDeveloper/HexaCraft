using UnityEngine;
using UnityEditor;
using System;
using System.Collections.Generic;

namespace HexaCraft
{
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

        public override void OnBrushEnter(GameObject target)
        {
            base.OnBrushEnter(target);
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
    }
}
