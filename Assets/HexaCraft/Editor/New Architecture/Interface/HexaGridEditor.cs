using UnityEngine;
using UnityEditor;

namespace HexaCraft
{
    public class HexaGridEditor : EditorWindow
    {
        static HexaGridEditor s_instance = null;
        public static HexaGridEditor instance { get { return s_instance; } }


        private const int toolBarNum = 2;

        private GUIContent[] m_GCToolmodeIcons = null;


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
                EditorGUIUtility.TrIconContent(IconUtility.GetIcon("Toolbar/Hex"), "Hex type grid generation mode"),
                EditorGUIUtility.TrIconContent(IconUtility.GetIcon("Toolbar/Rhombus"), "Rhombus type grid generation mode"),
            };

#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -= OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
        }

        private void OnGUI()
        {
            DrawGridTypeToolbar();
            DrawGridGeneratingUI();
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            Event e = Event.current;


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

        private void DrawGridGeneratingUI()
        {
            _hexPrefab = (GameObject)EditorGUILayout.ObjectField("Hex Prefab", _hexPrefab, typeof(GameObject), false);
            _gridRadius = EditorGUILayout.IntField("Grid Radius", _gridRadius);
            _hexCircumscribedRadiusSize = EditorGUILayout.FloatField("Hex Model Radius", _hexCircumscribedRadiusSize);

            if (GUILayout.Button("Generate Grid"))
            {
                HexGridGeneration.GenerateGrid(_hexPrefab, _gridRadius, _hexCircumscribedRadiusSize);
            }
        }
    }
}