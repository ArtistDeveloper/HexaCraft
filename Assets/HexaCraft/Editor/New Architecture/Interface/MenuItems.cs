using UnityEngine;
using UnityEditor;
using PlasticGui.WorkspaceWindow;

namespace HexaCraft
{
    public static class MenuItems
    {
        static HexGridEditEditor editor
        {
            get { return HexGridEditEditor.Instance; }
        }

        [MenuItem("Tools/" + PrefUtility.productName + "/Hex Grid Generation Window")]
        public static void MenuInitEditorWindow()
        {
            EditorWindow.GetWindow<HexGridGenerationEditor>("Hex Grid Generation Editor");
        }

        [MenuItem("Tools/" + PrefUtility.productName + "/Hex Grid Window")]
        public static void Init()
        {
            EditorWindow.GetWindow<HexGridEditEditor>("Hex Grid Editor");
        }
    }
}