using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public enum EditorMode
    {
        None,
        MaterialEdit,
        ObjectSelection,
        PathManipulation
    }


    public class HexGridEditEditor : EditorWindow
    {
        static HexGridEditEditor s_instance = null;

        public static HexGridEditEditor Instance { get { return s_instance; } }

        private const int toolBarNum = 2;

        private GUIContent[] m_GCToolmodeIcons = null;

        private EditorMode _currentMode;

        private ISceneGUIHandler _activeHandler;

        private MaterialEditHandler _materialEditHandler;

        private NoneOperationHandler _noneOperationHandler;
        

        private void OnEnable()
        {
            HexGridEditEditor.s_instance = this;

            m_GCToolmodeIcons = new GUIContent[]
            {
                EditorGUIUtility.TrIconContent(IconUtility.GetIcon("Toolbar/Hex"), EditorGUIContent.Tooltips.HEX_GRID),
                EditorGUIUtility.TrIconContent(IconUtility.GetIcon("Toolbar/Rhombus"), EditorGUIContent.Tooltips.RHOMBUS_GRID),
            };

            _materialEditHandler = new MaterialEditHandler();
            _noneOperationHandler = new NoneOperationHandler();

            SetEditorMode(EditorMode.None);

#if UNITY_2019_1_OR_NEWER
            // SceneView.duringSceneGui -= OnSceneGUI;
#else
            // SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
        }

        private void OnGUI()
        {
            DrawGridTypeToolbar();
        }

        private void OnSceneGUI()
        {

        }

        private void DrawGridTypeToolbar()
        {
            GUILayout.Label("Select Editing Type", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck(); // 변경 사항 체크

            int toolbarIndex = toolBarNum; // 선택된 버튼

            using (new GUILayout.HorizontalScope())
            {
                toolbarIndex = GUILayout.Toolbar(toolbarIndex, m_GCToolmodeIcons, GUILayout.Width(s_instance.position.width - 6), GUILayout.Height(23));
            }

            if (EditorGUI.EndChangeCheck()) // 변경 사항이 있으면 아래 코드 실행
            {
                // BrushTool newTool = (BrushTool)(toolbarIndex + 1);
                // SetTool(newTool == tool ? BrushTool.None : (BrushTool)toolbarIndex + 1);
            }
        }

        private void SetEditorMode(EditorMode mode)
        {
            _currentMode = mode;

            switch (_currentMode)
            {
                case EditorMode.None:
                    _activeHandler = _noneOperationHandler;
                    break;
                case EditorMode.MaterialEdit:
                    _activeHandler = _materialEditHandler;
                    break;
            }

            SceneView.RepaintAll();
        }
    }
}
