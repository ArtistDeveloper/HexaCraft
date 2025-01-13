using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class MaterialChange
    {
        private readonly HCPresenter _presenter;

        private Vector2 _mousePosition;

        private Ray _ray;

        private RaycastHit _hit;

        public MaterialChange(HCPresenter presenter)
        {
            _presenter = presenter;
        }

        public void ApplyMaterialChange(Event evt)
        {
            _mousePosition = evt.mousePosition;
            _ray = HandleUtility.GUIPointToWorldRay(_mousePosition);
            if (Physics.Raycast(_ray, out _hit))
            {
                GameObject go = _hit.collider.gameObject;
                Renderer renderer = go.GetComponent<Renderer>();
                if (renderer == null)
                    return;

                var originalMaterials = renderer.sharedMaterials;
                Material[] newMaterials = new Material[originalMaterials.Length];

                for (int i = 0; i < newMaterials.Length; i++)
                {
                    newMaterials[i] = _presenter.GetSelectionMaterial();
                }

                renderer.sharedMaterials = newMaterials;
            }
        }
    }
}
