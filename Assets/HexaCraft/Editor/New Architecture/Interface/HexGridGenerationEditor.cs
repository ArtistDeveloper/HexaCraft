using UnityEngine;
using UnityEditor;

namespace HexaCraft
{
    enum GridType
    {
        Hexagon = 0,
        Rhombus = 1,
    }

    public class HexGridGenerationEditor : EditorWindow
    {
        #region Input Property field
        private GameObject _calcTargetHexPrefab;

        private GameObject _hexPrefab;

        private int _gridRadius;

        private float _hexCircumscribedRadiusSize;

        private Material _material;

        private GameObject _pathPrefab;
        #endregion

        private GUIContent[] _gridTypeIcons;

        private int _selectedGridTypeIndex;

        private float _calculatedRadius;

        private const int MAX_GRID_RADIUS = 50;

        private GridType _gridType;


        private void OnEnable()
        {
            _gridTypeIcons = new GUIContent[]
            {
                EditorGUIUtility.TrIconContent(IconUtility.GetIcon("Toolbar/Hex"), EditorGUIContent.Tooltips.HEX_GRID),
                EditorGUIUtility.TrIconContent(IconUtility.GetIcon("Toolbar/Rhombus"), EditorGUIContent.Tooltips.RHOMBUS_GRID),
            };
            _gridType = GridType.Hexagon;
        }

        private void OnGUI()
        {
            DrawGridTypeRadioButton();
            DrawHexModelSizeCalc();
            DrawGridGeneratingUI();
        }

        private void DrawGridTypeRadioButton()
        {
            GUILayout.Label("Select Grid Type", EditorStyles.boldLabel);

            EditorGUI.BeginChangeCheck();

            _selectedGridTypeIndex = GUILayout.SelectionGrid(
                _selectedGridTypeIndex,
                _gridTypeIcons,
                _gridTypeIcons.Length,
                GUILayout.Width(position.width - 6)
            );
            
            // 선택된 그리드 타입에 따른 처리
            if (EditorGUI.EndChangeCheck())
            {
                _gridType = (GridType)_selectedGridTypeIndex;
            }

            EditorGUILayout.Space(10);
        }

        private void DrawHexModelSizeCalc()
        {
            GUILayout.Label("Hex Model Size Calculation", EditorStyles.boldLabel);
            _calcTargetHexPrefab = (GameObject)EditorGUILayout.ObjectField(EditorGUIContent.Labels.HEX_PREFAB, _calcTargetHexPrefab, typeof(GameObject), false);

            if (GUILayout.Button(EditorGUIContent.Labels.CALC_HEX_MODEL_SIZE))
            {
                float radius = HexModelSizeCalc.CalcHexModelSize(_calcTargetHexPrefab);
                _calculatedRadius = radius;
                Debug.Log($"Calculated Hex Model Radius: {_calculatedRadius}");
            }

            // 계산된 반지름을 보여주는 읽기 전용 필드
            using (new EditorGUI.DisabledGroupScope(true))
            {
                EditorGUILayout.TextField("Calculated Radius", _calculatedRadius.ToString("F3"));
            }

            EditorGUILayout.Space(10);
        }

        private void DrawGridGeneratingUI()
        {
            GUILayout.Label("Hex Grid Generation", EditorStyles.boldLabel);

            _hexPrefab = (GameObject)EditorGUILayout.ObjectField(EditorGUIContent.Labels.HEX_PREFAB, _hexPrefab, typeof(GameObject), false);

            // IntField 대신 IntSlider를 사용하여 범위 제한
            _gridRadius = EditorGUILayout.IntSlider(EditorGUIContent.Labels.GRID_RADIUS, _gridRadius, 1, MAX_GRID_RADIUS);

            _hexCircumscribedRadiusSize = EditorGUILayout.FloatField(EditorGUIContent.Labels.HEX_MODEL_RADIUS, _hexCircumscribedRadiusSize);

            if (GUILayout.Button(EditorGUIContent.Labels.GENERATE_GRID))
            {
                switch (_gridType)
                {
                    case GridType.Hexagon:
                        HexGridGeneration.GenerateHexGrid(_hexPrefab, _gridRadius, _hexCircumscribedRadiusSize);
                        break;
                    case GridType.Rhombus:
                        HexGridGeneration.GenerateRhombusGrid(_hexPrefab, _gridRadius, _hexCircumscribedRadiusSize);
                        break;
                }
            }
        }
    }
}
