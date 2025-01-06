using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class PathManipulation : ICommand
    {
        public Hex start;
        public Hex goal;

        public void Execute(SceneView sceneView, Event evt)
        {
            throw new System.NotImplementedException();
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
