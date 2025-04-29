using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using PlasticGui.Configuration.OAuth;

// TODO: 구현해야 하는 것. 
// - BrushMode.OnEanble()
// - BrushMode.OnDisable()
// - UpdateBrush()
// - ApplyBrush()
// - OnBrushEnter() - 마우스가 편집 가능한 개체에 커서를 가져가기 시작하면 호출됩니다.

// 세부 구현 사항
// 1. 객체 hovering
// 2. hovering된 객체가 hextile인지 판별 (아마 호버링된 게임오브젝트의 hex.cs 컴포넌트 유무로 가능할 듯)
// 3. hovering 된 객체를 List로 관리할 때도 필요하고, 단일 게임오브젝트로 관리할 때도 필요하다.
// 3.1 예를 들어, BrushModeMaterial은 BrushSize가 존재하기에 이에 따른 Brush 크기가 커질수록 한 번에 hovering되는 객체가 많아진다.
// 3.2 그러나 ObjectAdd, PathManipulation 같은 조작은 단일 hex.cs를 가진 게임오브젝트만 선택해도 되기에, 이를 어떻게 전체적으로 BrushMode에서 관리할지 고민이 필요.

namespace HexaCraft
{
    public class HexGridEditEditor : EditorWindow
    {
        private static HexGridEditEditor s_instance = null;

        public static HexGridEditEditor Instance { get { return s_instance; } }

        private const int toolBarNum = 2;

        private GUIContent[] m_GCToolmodeIcons = null;


        // TODO : [SerializeField] 붙일지 말지 고민. 공부필요
        List<BrushMode> modes = new List<BrushMode>();

        public BrushTool tool = BrushTool.None;

        private GameObject _lastHoveredGameObject = null;

        internal BrushMode mode
        {
            get
            {
                return modes.Count > 0 ? modes[0] : null;
            }
            set
            {
                if (modes.Contains(value))
                    modes.Remove(value);
                modes.Insert(0, value);
            }
        }


        private void OnEnable()
        {
            s_instance = this;

            m_GCToolmodeIcons = new GUIContent[]
            {
                EditorGUIUtility.TrIconContent(IconUtility.GetIcon("Toolbar/Hex"), EditorGUIContent.Tooltips.HEX_GRID),
                EditorGUIUtility.TrIconContent(IconUtility.GetIcon("Toolbar/Rhombus"), EditorGUIContent.Tooltips.RHOMBUS_GRID),
                EditorGUIUtility.TrIconContent(IconUtility.GetIcon("Toolbar/Rhombus"), EditorGUIContent.Tooltips.RHOMBUS_GRID),
            };


            SetTool(BrushTool.MaterialChange, false);

#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui += OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate += OnSceneGUI;
#endif
        }

        private void OnDisable()
        {
#if UNITY_2019_1_OR_NEWER
            SceneView.duringSceneGui -= OnSceneGUI;
#else
            SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
        }

        private void OnGUI()
        {
            DrawToolbar();

            if (tool != BrushTool.None)
                DrawActiveToolmodeSettings();
        }

        private void OnSceneGUI(SceneView sceneView)
        {
            if (mode == null)
                return;

            Event e = Event.current;
            int controlID = GUIUtility.GetControlID(FocusType.Passive);

            Vector2 mousePosition = e.mousePosition;

            switch (e.GetTypeForControl(controlID))
            {
                // 이 부분에서 마우스 아이콘 변하는 것을 구현해야 할 듯
                // // 브러시 업데이트 (OnBrushEnter, OnBrushExit, OnBrushMove 처리)
                case EventType.MouseMove:
                    UpdateBrush(mousePosition);
                    break;

                case EventType.MouseDown:
                case EventType.MouseDrag:
                    UpdateBrush(mousePosition);
                    ApplyBrush();
                    break;
            }
        }

        private void DrawActiveToolmodeSettings()
        {
            using (new GUILayout.VerticalScope())
            {
                mode.DrawGUI();
            }
        }

        /// <summary>
        /// Draw toolbar menu
        /// </summary>
        private void DrawToolbar()
        {
            GUILayout.Label("Select Editing Type", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();

            // In the ToolBar, -1 means that nohting is selected, and 0 means that the first thing is selected.
            int toolbarIndex = (int)tool - 1;

            using (new GUILayout.HorizontalScope())
            {
                toolbarIndex = GUILayout.Toolbar(toolbarIndex, m_GCToolmodeIcons, GUILayout.Width(s_instance.position.width - 6), GUILayout.Height(23));
            }

            if (EditorGUI.EndChangeCheck())
            {
                BrushTool newTool = (BrushTool)(toolbarIndex + 1);
                SetTool(newTool == tool ? BrushTool.None : (BrushTool)toolbarIndex + 1);
            }
        }

        private void UpdateBrush(Vector2 mousePosition)
        {
            GameObject go = null;

#if UNITY_2021_1_OR_NEWER
            // 마우스 이동 이벤트 중에만 HandleUtility.PickGameObject를 확인해야 한다.
            int materialIndex;
            go = HandleUtility.PickGameObject(mousePosition, false, null, Selection.gameObjects, out materialIndex);
#else
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                go = hit.collider.gameObject;
            }
#endif
            Debug.Log($"Go.name: {go.name}");
        }

        public bool ApplyBrush()
        {
            // mode.OnBrushApply();

            Repaint();

            return true;
        }

        private void OnBrusnEnter()
        {
            // mode.OnBrushEnter();
        }

        internal void SetTool(BrushTool brushTool, bool enableTool = true)
        {
            // newTool == tool을 통해 None값을 받았을 때는,
            // tool이 None일 수가 없다. 이미 무언가가 선택되어있는 상태였기에 tool이 None이었을 수 없다.
            if (brushTool == tool && mode != null)
                return;

            if (mode != null)
            {
                // mode가 존재한다면 mode 탈출
                // if (m_LastHoveredGameObject != null)
                // {
                //     OnBrushExit(m_LastHoveredGameObject);
                //     FinalizeAndResetHovering();
                // }

                // mode.OnDisable();
            }

            // m_LastHoveredGameObject = null;

            // 선택된 brushTool의 Type을 가져옴
            System.Type modeType = brushTool.GetModeType();

            if (modeType != null)
            {
                // modes 리스트에서 현재 선택된 modeType과 일치하는 BrushMode 객체를 탐색하고 반환한다.
                mode = modes.FirstOrDefault(x => x != null && x.GetType() == modeType);

                // 만약 mode 객체가 null이라면 해당하는 type의 객체 생성
                if (mode == null)
                {
                    mode = (BrushMode)ScriptableObject.CreateInstance(modeType);
                }
            }

            // tool을 자동으로 활성화/비활성화를 처리
            tool = enableTool ? brushTool : BrushTool.None;

            if (tool != BrushTool.None)
            {
                Tools.current = Tool.None;
                mode.OnEnable();
            }

            Repaint();
        }
    }
}
