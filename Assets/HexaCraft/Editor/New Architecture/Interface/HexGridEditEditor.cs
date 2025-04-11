using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Linq;

// TODO: 왜 Brush를 BrushTool의 enum과 BrushMode 클래스 2개를 사용해서 상태를 표현하게 되었는지는 구조적인 고민이 필요하다.
// 어떤 구조적 장점이 있지?

namespace HexaCraft
{
    public class HexGridEditEditor : EditorWindow
    {
        private static HexGridEditEditor s_instance = null;

        public static HexGridEditEditor Instance { get { return s_instance; } }

        private const int toolBarNum = 2;

        private GUIContent[] m_GCToolmodeIcons = null;


        List<BrushMode> modes = new List<BrushMode>();

        // [SerializeField] TODO : 붙일지 말지 고민. 공부필요

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
            // SceneView.duringSceneGui -= OnSceneGUI;
#else
            // SceneView.onSceneGUIDelegate -= OnSceneGUI;
#endif
        }

        private void OnGUI()
        {
            DrawToolbar();


            // TODO: 이런 느낌으로 Toolbar 별 렌더링 
            if (tool != BrushTool.None)
                DrawActiveToolmodeSettings();

        }

        private void OnSceneGUI()
        {

        }

        private void DrawActiveToolmodeSettings()
        {
            using (new GUILayout.VerticalScope())
            {
                mode.DrawGUI();
            }
        }

        // ToolBar에서 -1은 아무것도 선택되지 않은 상태를 의미하며, 0은 첫 번째 것을 선택함을 의미한다.
        private void DrawToolbar()
        {
            GUILayout.Label("Select Editing Type", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();

            int toolbarIndex = (int)tool - 1;

            using (new GUILayout.HorizontalScope())
            {
                toolbarIndex = GUILayout.Toolbar(toolbarIndex, m_GCToolmodeIcons, GUILayout.Width(s_instance.position.width - 6), GUILayout.Height(23));
            }

            // BrushTool.None은 0이지만, 0은 첫 번째 것이 선택된 상태이므로 + 1을 더해준다
            if (EditorGUI.EndChangeCheck())
            {
                BrushTool newTool = (BrushTool)(toolbarIndex + 1);
                // 새로 클릭한 것과 기존의 것이 동일하다면 None, 아니면 새로 선택한 BurshTool을 넘김
                SetTool(newTool == tool ? BrushTool.None : (BrushTool)toolbarIndex + 1);
            }
        }

        // 
        internal void SetTool(BrushTool brushTool, bool enableTool = true)
        {
            // newTool == tool을 통해 None값을 받았을 때는,
            // tool이 None일 수가 없다. 이미 무언가가 선택되어있는 상태였기에 tool이 None이었을 수 없다.
            if (brushTool == tool && mode != null)
                return;

            if (mode != null)
            {
                // Exiting edit mode
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
                // FIXME: 왜 해당하는 타입은 가져오는데, 이에 맞는 UI는 렌더링 안되는거지?
                // 아 !!! Mode 객체가 삭제가 안되네 ㅋㅋㅋ
                // 이걸 어케하나.. 이래서 List를 쓴건가?
                mode = modes.FirstOrDefault(x => x != null && x.GetType() == modeType);

                // 해당하는 brushTool enum이 가리키는 type의 인스턴스를 생성
                if (mode == null)
                {
                    mode = (BrushMode)ScriptableObject.CreateInstance(modeType);
                    Debug.Log($"mode type : {mode.GetType().Name}");
                }

            }

            // Handle tool auto activation/deactivation.
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
