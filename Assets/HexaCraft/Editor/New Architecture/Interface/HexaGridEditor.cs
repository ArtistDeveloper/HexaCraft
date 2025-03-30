using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace HexaCraft
{
    public enum EditorMode
    {
        None,
        ObjectSelection,
        MaterialEdit,
        PathManipulation
    }

    public class HexaGridEditor : EditorWindow
    {
        static HexaGridEditor s_instance = null;
        public static HexaGridEditor instance { get { return s_instance; } }

        private const int toolBarNum = 2;

        private GUIContent[] m_GCToolmodeIcons = null;

        private EditorMode _currentMode;

        private ISceneGUIHandler _activeHandler;

        private MaterialEditHandler _materialEditHandler;

        private NoneOperationHandler _noneOperationHandler;


        #region Input Property field
        private GameObject _hexPrefab;

        private int _gridRadius;

        private float _hexCircumscribedRadiusSize;

        private Material _material;

        private GameObject _pathPrefab;
        #endregion


        [MenuItem("Tools/HexaCraft/Hexa Grid Editor")]
        public static void ShowWindow()
        {
            GetWindow<HexaGridEditor>("Hexa Grid Editor");
        }

        void OnEnable()
        {
            HexaGridEditor.s_instance = this;
            Debug.Log("HexaGridEditor OnEnable");

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

        /// <summary>
        /// Toolbar to select the type of grid to create.
        /// </summary>
        private void DrawGridTypeToolbar()
        {
            GUILayout.Label("Select Grid Type", EditorStyles.boldLabel);

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

        // private void DrawMaterialEditingUI(float spaceSize, string label, GUIStyle style)
        // {
        //     GUILayout.Label("Edit Hex Material", EditorStyles.boldLabel);

        //     _material = (Material)EditorGUILayout.ObjectField("Select Material", _material, typeof(Material), false);

        //     if (GUILayout.Button(GetToggleButtonText("Finish Change Material", "Change Material", ToggleButton.MaterialChange)))
        //     {
        //         _presenter.OnToggleClicked(ToggleButton.MaterialChange);
        //     }
        // }

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