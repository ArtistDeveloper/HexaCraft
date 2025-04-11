using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace HexaCraft
{
    public class HexGridEditEditor : EditorWindow
    {
        private static HexGridEditEditor s_instance = null;

        public static HexGridEditEditor Instance { get { return s_instance; } }

        private const int toolBarNum = 2;

        private GUIContent[] m_GCToolmodeIcons = null;


        // [SerializeField] TODO : 붙일지 말지 고민. 공부필요
        BrushMode mode;

        public BrushTool tool = BrushTool.None;



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

        private void DrawToolbar()
        {
            GUILayout.Label("Select Editing Type", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck(); // 변경 사항 체크

            int toolbarIndex = (int)tool - 1; // 선택된 버튼

            using (new GUILayout.HorizontalScope())
            {
                toolbarIndex = GUILayout.Toolbar(toolbarIndex, m_GCToolmodeIcons, GUILayout.Width(s_instance.position.width - 6), GUILayout.Height(23));
            }

            if (EditorGUI.EndChangeCheck()) // 변경 사항이 있으면 아래 코드 실행
            {
                BrushTool newTool = (BrushTool)(toolbarIndex + 1);
                SetTool(newTool == tool ? BrushTool.None : (BrushTool)toolbarIndex + 1);
            }
        }

        internal void SetTool(BrushTool brushTool, bool enableTool = true)
        {
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

            System.Type modeType = brushTool.GetModeType();

            if (modeType != null)
            {
                if (mode == null)
                    mode = (BrushMode)ScriptableObject.CreateInstance(modeType);
            }

            // Handle tool auto activation/deactivation.
            tool = enableTool ? brushTool : BrushTool.None;

            if (tool != BrushTool.None)
            {
                Tools.current = Tool.None;
                mode.OnEnable();
            }

            // EnsureBrushSettingsListIsValid();
            // DoRepaint();
        }
    }
}
