using UnityEditor;
using UnityEditor.UI;
using UnityEngine;

namespace HexaCraft
{
    public class PathManipulation
    {
        private HCPresenter _presenter;

        private GameObject _startPoint;

        private GameObject _goalPoint;


        public PathManipulation(HCPresenter presenter)
        {
            _presenter = presenter;
        }

        public void Execute(Event evt)
        {
            Vector2 mousePosition = evt.mousePosition;
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                GameObject go = hit.collider.gameObject;
                HandleSelection(go);
            }
        }

        public void HandleSelection(GameObject selected)
        {
            switch (_presenter.GetPathEditingState())
            {
                case PathEditingState.SelectingStart:
                    SetStartPoint(selected);
                    break;
                case PathEditingState.SelectingGoal:
                    SetGoalPoint(selected);
                    break;
            }
        }

        private void SetStartPoint(GameObject selected)
        {
            if (_startPoint != null)
                ClearHighlight(_startPoint);

            _startPoint = selected;
            _presenter.SetStartPoint(selected);
            ApplyHighlight(selected, Color.green);
        }

        private void SetGoalPoint(GameObject selected)
        {
            if (_goalPoint != null)
                ClearHighlight(_goalPoint);

            _goalPoint = selected;
            _presenter.SetGoalPoint(selected);
            ApplyHighlight(selected, Color.red);
        }

        private void ApplyHighlight(GameObject selected, Color color)
        {
            var renderer = selected.GetComponent<Renderer>();
            if (renderer == null) return;

            var propertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propertyBlock);

            // 기존 색상과 블렌드되도록 알파값을 조정
            Color blendColor = new Color(color.r, color.g, color.b, 0.5f);
            propertyBlock.SetColor("_EmissionColor", blendColor);

            // Emission 활성화
            renderer.sharedMaterial.EnableKeyword("_EMISSION");
            renderer.SetPropertyBlock(propertyBlock);
        }

        private void ClearHighlight(GameObject obj)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer == null) return;

            var propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor("_EmissionColor", Color.black);
            renderer.SetPropertyBlock(propertyBlock);

            // Emission 비활성화
            renderer.sharedMaterial.DisableKeyword("_EMISSION");
        }
    }
}
