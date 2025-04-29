using UnityEngine;

namespace HexaCraft
{
    public class BrushModeHexTile : BrushMode
    {
        public override void RegisterUndo()
        {
            throw new System.NotImplementedException();
        }

        public override void DrawGUI()
        {
            GUILayout.TextField("HexTileEdit Field");
        }

        public override void OnBrushApply(GameObject target)
        {
            throw new System.NotImplementedException();
        }
    }
}