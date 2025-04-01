using System;
using UnityEngine;

namespace HexaCraft
{
    public class HexGridGenerator
    {
        private GameObject _root;

        private GameObject _hexPrefab;

        private float _hexSize = 1.0f; // Radius size of a hexagonal circumscribed circle (distance from center to farthest vertex)

        private int _n;

        private int _arrRowSize;

        private int _arrColSize;

        public GameObject HexPrefab { set { _hexPrefab = value; } }

        private Hex[,] _hexTiles; // Note: Where a Tile does not exist, it exists as a null value.


        /// <summary>
        /// Create a hexagonal-shaped Grid.
        /// </summary>
        /// <param name="hexPrefab"></param>
        /// <param name="n"></param>
        /// <param name="hexSize"></param>
        /// <returns></returns>
        public Hex[,] GenerateHexGrid(GameObject hexPrefab, int n, float hexSize)
        {
            if (hexPrefab == null)
            {
                UnityEngine.Debug.LogError("Hex Prefab cannot be null.");
                return null;
            }

            if (n <= 0 || hexSize <= 0)
            {
                UnityEngine.Debug.LogError("Grid size and hex size must be greater than zero");
                return null;
            }

            _n = n;
            _hexPrefab = hexPrefab;
            _hexSize = hexSize;

            InitHexRoot();
            InitHexGrid(GridType.Hexagon);
            GenerateHexShapeGrid();
            return _hexTiles;
        }

        /// <summary>
        /// Create a rhombus-shaped Grid.
        /// </summary>
        /// <param name="hexPrefab"></param>
        /// <param name="n"></param>
        /// <param name="hexSize"></param>
        /// <returns></returns>
        public Hex[,] GenerateRhombusGrid(GameObject hexPrefab, int n, float hexSize)
        {
            if (hexPrefab == null)
            {
                UnityEngine.Debug.LogError("Hex Prefab cannot be null.");
                return null;
            }

            if (n <= 0 || hexSize <= 0)
            {
                UnityEngine.Debug.LogError("Grid size and hex size must be greater than zero");
                return null;
            }

            _n = n;
            _hexPrefab = hexPrefab;
            _hexSize = hexSize;

            InitHexRoot();
            InitHexGrid(GridType.Rhombus);
            GenerateRhombusShapeGrid();
            return _hexTiles;
        }

        /// <summary>
        /// Create a Parent Object to contain the Hex Tiles in the Grid
        /// </summary>
        private void InitHexRoot()
        {
            _root = new GameObject() { name = "RootGrid" };
        }

        /// <summary>
        /// Declare an array to hold the actual hex objects
        /// </summary>
        private void InitHexGrid(GridType gridType)
        {
            DecideArrSize(gridType);
            _hexTiles = new Hex[_arrRowSize, _arrColSize];
        }

        private void DecideArrSize(GridType gridType)
        {
            switch (gridType)
            {
                case GridType.Hexagon:
                    _arrRowSize = _n * 2 + 1;
                    _arrColSize = _n * 2 + 1;
                    break;
                case GridType.Rhombus:
                    _arrRowSize = _n * 2 + 1;
                    _arrColSize = _n * 2 + 1;
                    break;
            }
        }

        /// <summary>
        /// Collect hexagons to form a larger hexagonal grid
        /// </summary>
        private void GenerateHexShapeGrid()
        {
            for (int q = -_n; q <= _n; q++)
            {
                int r1 = Math.Max(-_n, -q - _n);
                int r2 = Math.Min(_n, -q + _n);
                for (int r = r1; r <= r2; r++) // max(-n, -q-n) <= r <= min(+n, -q+n)
                {
                    GameObject hexTile = CreateHex(q, r, _n);
                    _hexTiles[q + _n, r + _n] = hexTile.GetComponent<Hex>();
                    _hexTiles[q + _n, r + _n].HexType = HexTileType.Path;
                }
            }
        }

        /// <summary>
        /// Collect hexagons to form a rhombus grid
        /// </summary>
        private void GenerateRhombusShapeGrid()
        {
            for (int q = -_n; q <= _n; q++)
            {
                for (int r = -_n; r <= _n; r++)
                {
                    GameObject hexTile = CreateHex(q, r, _n);
                    _hexTiles[q + _n, r + _n] = hexTile.GetComponent<Hex>();
                    _hexTiles[q + _n, r + _n].HexType = HexTileType.Path;
                }
            }
        }

        /// <summary>
        /// Create a HexTile corresponding to the coordinates.
        /// Adds an offset equal to the range of the hex grid so that the Grid doesn't contain negative coordinates
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <param name="offset"></param>
        /// <returns></returns>
        private GameObject CreateHex(int q, int r, int offset)
        {
            Vector3 position = HexToWorldPosition(q, r);
            GameObject go = GameObject.Instantiate(_hexPrefab, position, Quaternion.identity, _root.transform);
            go.name = $"{q + offset},{r + offset}";

            Hex hex = go.AddComponent<Hex>();
            hex.Initialize(q + offset, r + offset, HexTileType.Blank);
            return go;
        }

        /// <summary>
        /// Converts a position in the Axial coordinate system to a WorldPosition and returns it.
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <returns></returns>
        private Vector3 HexToWorldPosition(int q, int r)
        {
            float x = _hexSize * (Mathf.Sqrt(3) * q + Mathf.Sqrt(3) / 2 * r);
            float z = -(_hexSize * (3.0f / 2 * r));
            return new Vector3(x, 0, z);
        }

        public void ClearGrid()
        {
            GameObject.DestroyImmediate(_root);
        }

        public void ClearChildHexTile()
        {
            for (int i = _root.transform.childCount - 1; i >= 0; i--)
            {
                GameObject.DestroyImmediate(_root.transform.GetChild(i).gameObject);
            }
            // InitHexGrid();
        }
    }
}
