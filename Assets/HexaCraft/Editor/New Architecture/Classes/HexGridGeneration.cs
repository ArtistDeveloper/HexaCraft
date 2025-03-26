using UnityEngine;

namespace HexaCraft
{
    public class HexGridGeneration
    {
        private static HexGridGenerator _hexGridGenerator = new HexGridGenerator();

        public static void GenerateGrid(GameObject hexPrefab, int n, float hexSize)
        {
            _hexGridGenerator.GenerateGrid(hexPrefab, n, hexSize);  
        } 
    }
}
