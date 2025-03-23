using UnityEngine;
using UnityEditor;
using PlasticGui.WorkspaceWindow;

namespace HexaCraft
{
    public static class MenuItems
    {
        static HexaGridEditor editor
        {
            get { return HexaGridEditor.instance; }
        }

        // [MenuItem("Tools/" + PrefUtility.productName + "/Polybrush Window %#v", false, PrefUtility.menuEditor)]
        // public static void MenuInitEditorWindow()
        // {
        //     EditorWindow.GetWindow<PolybrushEditor>(PolybrushEditor.s_FloatingWindow).Show();
        // }

        // [MenuItem("Tools/" + PrefUtility.productName + "/Bake Vertex Streams", false, PrefUtility.menuBakeVertexStreams)]
        // public static void Init()
        // {
        //     EditorWindow.GetWindow<BakeAdditionalVertexStreams>(true, "Bake Vertex Streams", true);
        // }
    }
}