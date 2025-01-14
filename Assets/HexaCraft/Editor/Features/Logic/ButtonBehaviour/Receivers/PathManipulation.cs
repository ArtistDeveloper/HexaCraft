using HexaCraft;
using UnityEngine;

namespace HexaCraft
{
    public class PathManipulation
    {
        private HCPresenter _presenter;

        private PathEditingState _currentState = PathEditingState.Idle;

        private GameObject _startPoint;

        private GameObject _goalPoint;


        public PathManipulation(HCPresenter presenter)
        {
            _presenter = presenter;
        }

        public void OnStateUpdate()
        {
            switch (_currentState)
            {
                case PathEditingState.SelectingStart:
                    // 시작점 선택 로직
                    break;
                case PathEditingState.SelectingGoal:
                    // 도착점 선택 로직
                    break;
                case PathEditingState.ReadyToGenerate:
                    // 경로 생성 준비 상태
                    break;
            }
        }

        public void HandleSelection(GameObject selected)
        {
            switch (_currentState)
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
