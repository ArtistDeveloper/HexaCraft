using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class MaterialEditing : ICommand
    {
        private readonly HCPresenter _presenter;

        private Vector2 _mousePosition;

        private Ray _ray;

        private RaycastHit _hit;

        private Vector3 _point;

        public MaterialEditing(HCPresenter presenter)
        {
            _presenter = presenter;
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
                _point = _hit.point;
                GameObject selectedObject = _hit.collider.gameObject;
                Material[] materials = new Material[selectedObject.GetComponent<Renderer>().sharedMaterials.GetLength(0)];

                for (int i = 0; i < materials.GetLength(0); i++)
                {
                    materials[i] = _presenter.GetSelectionMaterial();
                }

                selectedObject.GetComponent<Renderer>().sharedMaterials = materials;
            }
        }
    }
}