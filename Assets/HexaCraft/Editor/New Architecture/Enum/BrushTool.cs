using NUnit.Framework.Constraints;
using UnityEngine;

namespace HexaCraft
{
    public enum BrushTool
    {
        None = 0,
        MaterialChange = 1,
        HexTileEdit = 2,
    }

    public static class BrushToolUtility
    {
        public static System.Type GetModeType(this BrushTool tool)
        {
            switch (tool)
            {
                case BrushTool.MaterialChange:
                    Debug.Log("MaterailChange");
                    return typeof(BrushModeMaterial);
                case BrushTool.HexTileEdit:
                    Debug.Log("HexTileEdit");
                    return typeof(BrushModeHexTile);
                default:
                    return null;
            }
        }
    }
}
