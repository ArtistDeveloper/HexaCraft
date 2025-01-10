using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class MaterialEditing : ICommand
    {
        private readonly SceneInteractor _interactor;

        private Vector2 _mousePosition;

        private Ray _ray;

        private RaycastHit _hit;

        public MaterialEditing(SceneInteractor interactor)
        {
            _interactor = interactor;
        }

        public void Execute(SceneView sceneView, Event evt)
        {
            if (evt.button == 1)
                return;

            if (evt.type == EventType.MouseDown)
            {
                evt.Use();
                ApplyMaterialChange(evt);
            }
            else if (evt.type == EventType.MouseDrag)
            {
                evt.Use();
                ApplyMaterialChange(evt);
            }
        }
                
        private void ApplyMaterialChange(Event evt)
        {
            _mousePosition = evt.mousePosition;
            _ray = HandleUtility.GUIPointToWorldRay(_mousePosition);
            if (Physics.Raycast(_ray, out _hit))
            {
                // _point = _hit.point;
                GameObject selectedObject = _hit.collider.gameObject;
                _interactor.NotifyMaterialEditing(selectedObject);
            }
        }
    }
}