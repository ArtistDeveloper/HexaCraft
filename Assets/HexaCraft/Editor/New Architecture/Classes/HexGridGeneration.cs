using UnityEngine;

namespace HexaCraft
{
    public class HexGridGeneration
    {
        private static HexGridGenerator _hexGridGenerator = new HexGridGenerator();

        public static void GenerateHexGrid(GameObject hexPrefab, int n, float hexSize)
        {
            _hexGridGenerator.GenerateHexGrid(hexPrefab, n, hexSize);
        }

        public static void GenerateRhombusGrid(GameObject hexPrefab, int n, float hexSize)
        {
            _hexGridGenerator.GenerateRhombusGrid(hexPrefab, n, hexSize);
        }
    }
}
