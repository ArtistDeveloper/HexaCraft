using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class PathManipulation
    {
        private HCPresenter _presenter;

        private PathEditingState _currentState;

        private GameObject _startPoint;

        private GameObject _goalPoint;


        public PathManipulation(HCPresenter presenter)
        {
            _presenter = presenter;
            _currentState = _presenter.GetPathEditingState();
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
                    _currentState = PathEditingState.SelectingGoal;
                    break;
                case PathEditingState.SelectingGoal:
                    SetGoalPoint(selected);
                    _currentState = PathEditingState.ReadyToGenerate;
                    break;
            }
        }

        private void SetStartPoint(GameObject selected)
        {
            if (_startPoint != null)
                ClearHighlight(_startPoint);

            _startPoint = selected;
            ApplyHighlight(selected, Color.green);
        }

        private void SetGoalPoint(GameObject selected)
        {
            if (_goalPoint != null)
                ClearHighlight(_goalPoint);

            _goalPoint = selected;
            ApplyHighlight(selected, Color.red);
        }

        public void GeneratePath()
        {
            if (_startPoint == null || _goalPoint == null)
                return;

            // 경로 생성 로직
            _currentState = PathEditingState.Idle;

            _startPoint = null;
            _goalPoint = null;
        }

        private void ApplyHighlight(GameObject go, Color color)
        {
            var renderer = go.GetComponent<Renderer>();
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
