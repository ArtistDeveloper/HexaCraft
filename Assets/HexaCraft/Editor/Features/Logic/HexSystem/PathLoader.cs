using Unity.VisualScripting;
using UnityEngine;

namespace HexaCraft
{
    public class PathLoader
    {
        public static void LoadHexTileData(GameObject rootGrid)
        {
            for (int i = 0; i < rootGrid.transform.childCount; i++)
            {
                Hex hex = rootGrid.transform.GetChild(i).GetComponent<Hex>();

                // GameObject 이름에서 좌표 파싱 (예: "0,15")
                string[] coordinates = hex.gameObject.name.Split(',');
                int q = int.Parse(coordinates[0]);
                int r = int.Parse(coordinates[1]);

                // Hex 컴포넌트 초기화
                hex.Initialize(q, r, hex.HexType);
                hex.HexTile = hex.gameObject;
            }
        }

        public static Hex[,] LoadPath(GameObject rootGrid, int gridRadius = -1)
        {
            LoadHexTileData(rootGrid);

            if (gridRadius < 0) 
            {
                gridRadius = CalculateGridRadius(rootGrid);
            }

            Hex[,] ret = new Hex[gridRadius * 2 + 1, gridRadius * 2 + 1];

            for (int i = 0; i < rootGrid.transform.childCount; i++)
            {
                Hex hex = rootGrid.transform.GetChild(i).GetComponent<Hex>();
                ret[hex.Pos.Q, hex.Pos.R] = hex;
            }

            return ret;
        }

        private static int CalculateGridRadius(GameObject rootGrid)
        {
            int minQ = int.MaxValue, maxQ = int.MinValue;
            int minR = int.MaxValue, maxR = int.MinValue;

            for (int i = 0; i < rootGrid.transform.childCount; i++)
            {
                Hex hex = rootGrid.transform.GetChild(i).GetComponent<Hex>();
                minQ = Mathf.Min(minQ, hex.Pos.Q);
                maxQ = Mathf.Max(maxQ, hex.Pos.Q);
                minR = Mathf.Min(minR, hex.Pos.R);
                maxR = Mathf.Max(maxR, hex.Pos.R);
            }

            return (maxQ - minQ) / 2;  // 또는 (maxR - minR) / 2 도 같은 값
        }
    }
}
