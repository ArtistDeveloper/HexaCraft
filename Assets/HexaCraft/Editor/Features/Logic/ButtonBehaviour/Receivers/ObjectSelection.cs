using HexaCraft;
using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class ObjectSelection
    {
        private Vector2 _mousePosition;

        private Ray _ray;

        private RaycastHit _hit;

        private HCPresenter _presenter;

        public ObjectSelection(HCPresenter presenter)
        {
            _presenter = presenter;
        }

        public void ApplySelectGameObject(Event evt)
        {
            _mousePosition = evt.mousePosition;
            _ray = HandleUtility.GUIPointToWorldRay(_mousePosition);

            if (Physics.Raycast(_ray, out _hit))
            {
                GameObject selectedObject = _hit.collider.gameObject;

                if (!_presenter.GetSelectedObjects().Contains(selectedObject))
                {
                    _presenter.AddSelectedObject(selectedObject);
                }
            }

            Selection.objects = _presenter.GetSelectedObjects().ToArray();
            SceneView.RepaintAll();
        }
    }
}
