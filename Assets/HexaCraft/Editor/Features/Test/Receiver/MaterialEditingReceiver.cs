using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class MaterialEditingReceiver
    {
        private readonly HCPresenter _presenter;

        private Vector2 _mousePosition;

        private Ray _ray;

        private RaycastHit _hit;

        public MaterialEditingReceiver(HCPresenter presenter)
        {
            _presenter = presenter;
        }

        public void ApplyMaterialChange(Event evt)
        {
            _mousePosition = evt.mousePosition;
            _ray = HandleUtility.GUIPointToWorldRay(_mousePosition);
            if (Physics.Raycast(_ray, out _hit))
            {
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
