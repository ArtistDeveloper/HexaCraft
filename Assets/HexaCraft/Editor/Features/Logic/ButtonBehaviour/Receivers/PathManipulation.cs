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

        private void SetStartPoint(GameObject obj)
        {
            if (_startPoint != null)
                ClearHighlight(_startPoint);

            _startPoint = obj;
            ApplyHighlight(obj, Color.green);
        }

        private void SetGoalPoint(GameObject obj)
        {
            if (_goalPoint != null)
                ClearHighlight(_goalPoint);

            _goalPoint = obj;
            ApplyHighlight(obj, Color.red);
        }

        public void GeneratePath()
        {
            if (_startPoint == null || _goalPoint == null)
                return;

            // 경로 생성 로직
            _currentState = PathEditingState.Idle;
        }

        private void ApplyHighlight(GameObject obj, Color color)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer == null) return;

            var propertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propertyBlock);
            propertyBlock.SetColor("_Color", color);
            renderer.SetPropertyBlock(propertyBlock);
        }

        private void ClearHighlight(GameObject obj)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer == null) return;

            var propertyBlock = new MaterialPropertyBlock();
            propertyBlock.Clear();
            renderer.SetPropertyBlock(propertyBlock);
        }
    }
}
