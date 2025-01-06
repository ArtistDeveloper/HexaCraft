using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class ObjectSelecting : ICommand
    {
        private readonly HCPresenter _presenter;

        private Vector2 _mousePosition;

        private Ray _ray;

        private RaycastHit _hit;

        private Vector3 _point;

        private bool _isPressed;

        public ObjectSelecting(HCPresenter presenter)
        {
            _presenter = presenter;
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
                _point = _hit.point;
                GameObject selectedObject = _hit.collider.gameObject;

                if (!_presenter.GetSelectedObjects().Contains(selectedObject))
                {
                    _presenter.AddSelectedObject(selectedObject);
                }

                Selection.objects = _presenter.GetSelectedObjects().ToArray();
                SceneView.RepaintAll();
            }
        }
    }
}