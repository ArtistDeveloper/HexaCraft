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
            Debug.Log("BrushModeHexTile의 DrawGUI 호출되나요?");
            GUILayout.TextField("HexTileEdit Field");
        }
    }
}