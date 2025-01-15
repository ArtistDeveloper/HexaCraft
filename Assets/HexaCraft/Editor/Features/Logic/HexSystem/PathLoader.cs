using Unity.VisualScripting;
using UnityEngine;

namespace HexaCraft
{
    public class PathLoader
    {
        public static Hex[,] LoadPath(GameObject rootGrid, int gridRadius)
        {
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
