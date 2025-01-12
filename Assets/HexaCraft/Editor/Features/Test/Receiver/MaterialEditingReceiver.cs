using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class MaterialEditingReceiver
    {
        // TODO: _presenter를 직접 참조하는 것이 아닌, 이벤트 형식으로 변경할 필요 존재.
        // private readonly HCPresenter _presenter;

        private Vector2 _mousePosition;

        private Ray _ray;

        private RaycastHit _hit;


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
                    // materials[i] = _presenter.GetSelectionMaterial();
                }

                selectedObject.GetComponent<Renderer>().sharedMaterials = materials;
            }
        }
    }
}
