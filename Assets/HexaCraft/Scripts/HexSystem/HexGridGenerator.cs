using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

namespace HexaCraft
{
    public class HexGridGenerator
    {
        private GameObject _root;
        private GameObject _hexPrefab;
        private float _hexSize = 1.0f; // 육각형 외접원의 반지름 크기(중심에서 꼭짓점까지의 거리)
        private int _n = 3;
        private int _arrRowSize;
        private int _arrColSize;



        #region 생성자
        public HexGridGenerator() { }

        public HexGridGenerator(GameObject hexPrefab)
        {
            _hexPrefab = hexPrefab;
        }

        public HexGridGenerator(GameObject hexPrefab, int n)
        {
            _hexPrefab = hexPrefab;
            _n = n;
        }

        public HexGridGenerator(GameObject hexPrefab, float hexSize)
        {
            _hexPrefab = hexPrefab;
            _hexSize = hexSize;
        }

        public HexGridGenerator(GameObject hexPrefab, int n, float hexSize)
        {
            _hexPrefab = hexPrefab;
            _n = n;
            _hexSize = hexSize;
        }
        #endregion

        public GameObject HexPrefab { set { _hexPrefab = value; } }

        // 주의: 타일이 빈 곳은 null값이 들어가 있음
        public Hex[,] HexTiles { get; private set; }

        /// <summary>
        /// n을 설정하지 않고 처음 지정된 n의 값을 사용하여 생성하려는 경우 호출하여 사용
        /// </summary>
        /// <returns></returns>
        public Hex[,] GenerateGrid()
        {
            InitHexRoot();
            InitHexGrid();
            GenerateHexShapeGrid();
            return HexTiles;
        }

        /// <summary>
        /// 동적으로 n의 크기가 변하여 Grid를 생성해야 할 경우 해당 함수를 호출하여 사용
        /// </summary>
        /// <param name="n"></param>
        /// <returns></returns>
        public Hex[,] GenerateGrid(int n)
        {
            _n = n;

            InitHexRoot();
            InitHexGrid();
            GenerateHexShapeGrid();
            return HexTiles;
        }

        /// <summary>
        /// 동적으로 n의 크기와 사용할 HexPrefab을 지정해주고 싶은 경우 사용
        /// </summary>
        /// <param name="n"></param>
        /// <param name="hexPrefab"></param>
        /// <returns></returns>
        public Hex[,] GenerateGrid(int n, GameObject hexPrefab)
        {
            _n = n;
            _hexPrefab = hexPrefab;

            InitHexRoot();
            InitHexGrid();
            GenerateHexShapeGrid();
            return HexTiles;
        }

        public Hex[,] GenerateGrid(GameObject hexPrefab, int n, float hexSize)
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
            InitHexGrid();
            GenerateHexShapeGrid();
            return HexTiles;
        }

        /// <summary>
        /// 생성자에서 바로 Root 오브젝트 생성시 
        /// HexGridGeneratorEditor 클래스에서 에디터가 렌더링될 떄마다 계속 Root 오브젝트 생성
        /// </summary>
        private void InitHexRoot()
        {
            _root = new GameObject() { name = "RootGrid" };
        }

        /// <summary>
        /// 실제 Hex 객체가 담긴 Hex[,] 배열 선언 및
        /// Hex 객체에 접근하는 연산을 담당하기 위한 HexTileType[,] 배열 선언
        /// </summary>
        private void InitHexGrid()
        {
            DecideArrSize();
            HexTiles = new Hex[_arrRowSize, _arrColSize];
        }

        private void DecideArrSize()
        {
            _arrRowSize = _n * 2 + 1;
            _arrColSize = _n * 2 + 1;
        }

        /// <summary>
        /// 육각형을 모아 더 큰 육각형 형태를 띄는 그리드를 생성
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
                    HexTiles[q + _n, r + _n] = hexTile.GetComponent<Hex>();
                    HexTiles[q + _n, r + _n].HexType = HexTileType.Path;

                    //CheckPos(q, r, hexTile);
                }
            }
        }

        /// <summary>
        /// 육각형에 부착되어 있는 TMP에 q, r 좌표 표시
        /// </summary>
        /// <param name="q"></param>
        /// <param name="r"></param>
        /// <param name="hexTile"></param>
        private void CheckPos(int q, int r, GameObject hexTile)
        {
            Transform textObj = hexTile.transform.GetChild(0);
            TextMeshPro textComp = textObj.GetComponent<TextMeshPro>();
            textComp.text = $"{q + _n},{r + _n}";
        }

        /// <summary>
        /// 좌표에 해당하는 HexTile을 생성 
        /// 음수 좌표를 포함하지 않도록 육각 그리드의 range범위만큼 offset을 더함
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
            hex.Initialize(q + offset, r + offset, HexTileType.Path);
            return go;
        }

        /// <summary>
        /// Axial 좌표계의 위치를 WorldPoition으로 변환 후 반환
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
            InitHexGrid();
        }
    }
}
