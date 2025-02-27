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

        public static Hex[,] LoadPath(GameObject rootGrid, int gridRadius)
        {
            LoadHexTileData(rootGrid);

            Hex[,] ret = new Hex[gridRadius * 2 + 1, gridRadius * 2 + 1];

            for (int i = 0; i < rootGrid.transform.childCount; i++)
            {
                Hex hex = rootGrid.transform.GetChild(i).GetComponent<Hex>();
                ret[hex.Pos.Q, hex.Pos.R] = hex;
            }

            return ret;
        }
    }
}
