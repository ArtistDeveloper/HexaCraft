using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;
using PlasticGui.Configuration.OAuth;

// TODO: 구현해야 하는 것. 
// - BrushMode.OnEanble()
// - BrushMode.OnDisable()
// - ApplyBrush()
// - OnBrushEnter() - 마우스가 편집 가능한 개체에 커서를 가져가기 시작하면 호출됩니다.

// 세부 구현 사항
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

        private GUIContent[] m_GCToolmodeIcons;


        // TODO : [SerializeField] 붙일지 말지 고민. 공부필요
        List<BrushMode> modes = new List<BrushMode>();

        public BrushTool brushTool = BrushTool.None;

        private GameObject _lastHoveredGameObject;

        private bool isHoverHextile;

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

            if (brushTool != BrushTool.None)
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
                case EventType.MouseMove:
                    UpdateBrush(mousePosition);
                    break;

                case EventType.MouseDown:
                case EventType.MouseDrag:
                    UpdateBrush(mousePosition);
                    ApplyBrush();
                    break;
                case EventType.Repaint:
                    mode.DrawGizmos(isHoverHextile);
                    break;
            }

            sceneView.Repaint();
            SceneView.RepaintAll();
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

            // 툴바에서 -1은 아무것도 선택되지 않았음을 의미. 0은 첫 번째 항목이 선택되었음을 의미
            int toolbarIndex = (int)brushTool - 1;

            using (new GUILayout.HorizontalScope())
            {
                toolbarIndex = GUILayout.Toolbar(toolbarIndex, m_GCToolmodeIcons, GUILayout.Width(s_instance.position.width - 6), GUILayout.Height(23));
            }

            if (EditorGUI.EndChangeCheck())
            {
                BrushTool newBrushTool = (BrushTool)(toolbarIndex + 1);
                SetTool(newBrushTool == brushTool ? BrushTool.None : (BrushTool)toolbarIndex + 1);
            }
        }

        private void UpdateBrush(Vector2 mousePosition)
        {
            GameObject go = null;

#if UNITY_2021_1_OR_NEWER
            // NOTE: PickGameObject에서 필요시 미리 hextile 오브젝트를 배열로 가진 뒤, Filter로 사용 가능할 수 있다.
            go = HandleUtility.PickGameObject(mousePosition, false);
            if (go == null)
            {
                isHoverHextile = false;
                return;
            }

            isHoverHextile = CheckHexComponent(go);
            Debug.Log($"isHoverHextile: {isHoverHextile}");
#else
            Ray ray = HandleUtility.GUIPointToWorldRay(mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                go = hit.collider.gameObject;
            }
#endif
        }

        private bool CheckHexComponent(GameObject go)
        {
            Hex component = go.GetComponent<Hex>();
            if (component == null)
                return false;
            return true;
        }

        public bool ApplyBrush()
        {
            // mode.OnBrushApply();

            Repaint();

            return true;
        }

        /// <summary>
        /// 선택된 브러시 도구에 따라 적절한 모드를 설정하고 활성화합니다.
        /// </summary>
        /// <param name="selectedBrushTool">설정할 브러시 도구</param>
        internal void SetTool(BrushTool selectedBrushTool)
        {
            // 선택된 brushTool의 Type을 가져옴
            System.Type modeType = selectedBrushTool.GetModeType();

            if (modeType != null)
            {
                // modes 리스트에서 현재 선택된 modeType과 일치하는 BrushMode 객체를 탐색하고 반환
                mode = modes.FirstOrDefault(x => x != null && x.GetType() == modeType);

                // 만약 mode 객체가 null이라면 해당하는 type의 객체 생성
                if (mode == null)
                {
                    mode = (BrushMode)ScriptableObject.CreateInstance(modeType);
                }
            }

            // tool을 자동으로 활성화/비활성화를 처리
            brushTool = selectedBrushTool;

            if (brushTool != BrushTool.None)
            {
                Tools.current = Tool.None;
                // NOTE: 당장은 OnEnable()에 세팅할 것들이 없다. 추후 세팅 연동에는 필요할 듯
                // mode.OnEnable(); 
            }

            Repaint();
        }
    }
}
