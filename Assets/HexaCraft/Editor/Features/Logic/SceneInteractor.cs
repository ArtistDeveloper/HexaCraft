using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace HexaCraft
{
    public class SceneInteractor
    {
        private readonly HCPresenter _presenter;

        public event Action<GameObject> OnObjectClicked;

        public event Action<GameObject> OnMaterialEditingRequested;

        private readonly Dictionary<ToggleButton, Action<SceneView>> _sceneActionsWrapper;

        // TODO: Presenter와 결합 줄일 필요 존재
        // 이벤트 방식으로 결합도를 줄여보자. 그리고 겪는 문제점이 생기면 그 때 다시 수정하면 되지.
        public SceneInteractor(HCPresenter presenter)
        {
            _presenter = presenter;

            _sceneActionsWrapper = new Dictionary<ToggleButton, Action<SceneView>>()
            {
                {
                    ToggleButton.MaterialEditing,
                    CreateSceneActionWrapper(new MaterialEditing(this))
                },
                {
                    ToggleButton.ObjectSelecting,
                    CreateSceneActionWrapper(new ObjectSelecting(this))
                },
            };
        }

        private Action<SceneView> CreateSceneActionWrapper(ISceneCommand action)
        {
            return (SceneView sceneView) => OnSceneGUI(sceneView, action);
        }

        public void RegisterActionToSceneView(ToggleButton type)
        {
            bool isModeActive = _presenter.CheckModeActive(type);

            if (_sceneActionsWrapper.TryGetValue(type, out var sceneActionWrapper))
            {
                if (isModeActive)
                {
                    SceneView.duringSceneGui -= sceneActionWrapper;
                }
                else
                {
                    SceneView.duringSceneGui += sceneActionWrapper;
                }

                SceneView.RepaintAll();
            }
        }

        public void ClearAllActionToSceneView()
        {
            foreach (var item in _sceneActionsWrapper)
            {
                SceneView.duringSceneGui -= item.Value;
            }
        }

        private void OnSceneGUI(SceneView sceneView, ISceneCommand action)
        {
            Event evt = Event.current;
            action.Execute(sceneView, evt);
        }

        public void NotifyObjectClicked(GameObject go)
        {
            OnObjectClicked?.Invoke(go);
        }

        public void NotifyMaterialEditing(GameObject go)
        {
            OnMaterialEditingRequested?.Invoke(go);
        }
    }
}
