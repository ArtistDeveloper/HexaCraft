using UnityEngine;
using UnityEditor;

namespace HexaCraft
{
    public class HexaGridEditor : EditorWindow
    {
        static HexaGridEditor s_instance = null;
        public static HexaGridEditor instance { get { return s_instance; } }

        private const int toolBarNum = 3;


        [MenuItem("Tools/HexaCraft/Hexa Grid Editor")]
        public static void ShowWindow()
        {
            GetWindow<HexaGridEditor>("Hexa Grid Editor");
        }

        void OnEnable()
        {
            HexaGridEditor.s_instance = this;
            Debug.Log("HexaGridEditor OnEnable");
        }

        private void OnGUI()
        {
            GUILayout.Label("Hello World");

            DrawToolbar();
        }

        /// <summary>
        /// 해당 툴바는 생성할 그리드의 타입을 선택하는 툴바입니다.
        /// </summary>
        void DrawToolbar()
        {
            EditorGUI.BeginChangeCheck(); // 변경 사항 체크

            int toolbarIndex = toolBarNum; // 선택된 버튼
            string[] toolbarStrings = {"Toolbar1", "Toolbar2", "Toolbar3"};

            using (new GUILayout.HorizontalScope())
            {
                toolbarIndex = GUILayout.Toolbar(toolbarIndex, toolbarStrings);
            }

            if (EditorGUI.EndChangeCheck()) // 변경 사항이 있으면 아래 코드 실행
            {
                // BrushTool newTool = (BrushTool)(toolbarIndex + 1);
                // SetTool(newTool == tool ? BrushTool.None : (BrushTool)toolbarIndex + 1);
            }
        }
    }
}