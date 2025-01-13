using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class ObjectSelecting : ISceneCommand
    {
        private readonly SceneInteractor _interactor;

        private Vector2 _mousePosition;

        private Ray _ray;

        private RaycastHit _hit;

        private bool _isPressed;

        public ObjectSelecting(SceneInteractor interactor)
        {
            _interactor = interactor;
        }

        public void Execute(SceneView sceneView, Event evt)
        {
            if (evt.button == 1)
                return;

            if (evt.type == EventType.MouseDown && !_isPressed)
            {
                evt.Use();
                ApplySelectGameObject(evt);
                _isPressed = true;
            }
            else if (evt.type == EventType.MouseDrag && _isPressed)
            {
                evt.Use();
                ApplySelectGameObject(evt);
            }
            else if (evt.type == EventType.MouseUp)
            {
                _isPressed = false;
            }
        }

        private void ApplySelectGameObject(Event evt)
        {
            _mousePosition = evt.mousePosition;
            _ray = HandleUtility.GUIPointToWorldRay(_mousePosition);

            if (Physics.Raycast(_ray, out _hit))
            {
                GameObject selectedObject = _hit.collider.gameObject;
                _interactor.NotifyObjectClicked(selectedObject);
            }
        }
    }
}