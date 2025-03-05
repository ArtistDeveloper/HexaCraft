using System.Collections.Generic;
using UnityEngine;

namespace HexaCraft
{
    public class PathGeneration
    {
        private HCPresenter _presenter;

        private PathFinder _pathFinder;
        
        private Dictionary<GameObject, Material> _materialInstances = new Dictionary<GameObject, Material>();

        public PathGeneration(HCPresenter presenter)
        {
            _presenter = presenter;
            _pathFinder = new PathFinder();
        }

        public void GeneratePath()
        {
            Hex[,] hexTiles = PathLoader.LoadPath(_presenter.GetRootGrid());
            HexPos startPoint = _presenter.GetStartPoint().GetComponent<Hex>().Pos;
            HexPos goalPoint = _presenter.GetGoalPoint().GetComponent<Hex>().Pos;

            List<HexPos> hexPoses = _pathFinder.AStar(hexTiles, startPoint, goalPoint);
            RenderHighLight(hexTiles, hexPoses);
            // InitializePoint();
        }

        public void RenderHighLight(Hex[,] hexTiles, List<HexPos> hexPoses)
        {
            foreach (var hexPos in hexPoses)
            {
                GameObject hexTile = hexTiles[hexPos.Q, hexPos.R].HexTile;
                ApplyHighlight(hexTile, Color.yellow);
            }
        }

        public void InitializePoint()
        {
            ClearHighlight(_presenter.GetStartPoint());
            ClearHighlight(_presenter.GetGoalPoint());
        }

        private void ApplyHighlight(GameObject go, Color color)
        {
            var renderer = go.GetComponent<Renderer>();
            if (renderer == null) return;

            // 해당 게임오브젝트의 Material 인스턴스가 없다면 생성
            if (!_materialInstances.ContainsKey(go))
            {
                Material newMaterial = new Material(renderer.sharedMaterial);
                renderer.material = newMaterial;
                _materialInstances[go] = newMaterial;
            }

            var propertyBlock = new MaterialPropertyBlock();
            renderer.GetPropertyBlock(propertyBlock);

            Color blendColor = new Color(color.r, color.g, color.b, 0.5f);
            propertyBlock.SetColor("_EmissionColor", blendColor);

            _materialInstances[go].EnableKeyword("_EMISSION");
            renderer.SetPropertyBlock(propertyBlock);
        }

        private void ClearHighlight(GameObject obj)
        {
            var renderer = obj.GetComponent<Renderer>();
            if (renderer == null) return;

            var propertyBlock = new MaterialPropertyBlock();
            propertyBlock.SetColor("_EmissionColor", Color.black);
            renderer.SetPropertyBlock(propertyBlock);

            if (_materialInstances.ContainsKey(obj))
            {
                _materialInstances[obj].DisableKeyword("_EMISSION");
            }
        }

        // private void ApplyHighlight(GameObject go, Color color)
        // {
        //     var renderer = go.GetComponent<Renderer>();
        //     if (renderer == null) return;

        //     var propertyBlock = new MaterialPropertyBlock();
        //     renderer.GetPropertyBlock(propertyBlock);

        //     // 기존 색상과 블렌드되도록 알파값을 조정
        //     Color blendColor = new Color(color.r, color.g, color.b, 0.5f);
        //     propertyBlock.SetColor("_EmissionColor", blendColor);

        //     // Emission 활성화
        //     renderer.sharedMaterial.EnableKeyword("_EMISSION");
        //     renderer.SetPropertyBlock(propertyBlock);
        // }

        // private void ClearHighlight(GameObject obj)
        // {
        //     var renderer = obj.GetComponent<Renderer>();
        //     if (renderer == null) return;

        //     var propertyBlock = new MaterialPropertyBlock();
        //     propertyBlock.SetColor("_EmissionColor", Color.black);
        //     renderer.SetPropertyBlock(propertyBlock);

        //     // Emission 비활성화
        //     renderer.sharedMaterial.DisableKeyword("_EMISSION");
        // }
    }
}
