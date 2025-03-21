using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using HexaCraft;
using static UnityEngine.Rendering.DebugUI.MessageBox;
using UnityEngine.UI;

namespace HexaCraft
{
    // [CustomEditor(typeof(HCGeneration))]
    public class HCMainView : Editor, IHCMainView
    {
        private GUIStyle _mainHeaderStyle;

        #region Events
        public event Action<GameObject> HexPrefabChanged;

        public event Action<int> GridRadiusChanged;

        public event Action<float> HexSizeChanged;

        public event Action<Material> MaterialChanged;

        public event Action<GameObject> PathGridPrefabChanged;
        
        public event Action<ToggleButton> ToggleButtonClicked;

        public event Action<Button> ButtonClicked;

        public event Action InspectorLockRequested;
        #endregion

        private HCMainPresenter _presenter;


        private GUIStyle _subHeaderStyle;

        private Texture logo;

        private const float _MainHeaderSpace = 5f;

        private const float _SubHeaderSpace = 7f;

        private const float _HorizonButtonHegiht = 20f;


        #region Property field
        private GameObject _hexPrefab;

        private int _gridRadius;

        private float _hexCircumscribedRadiusSize;

        private Material _material;

        private GameObject _pathPrefab;
        #endregion


        private PathEditingState _currentPathEditingState;
        private string _materialButtonText;
        private string _inspectorLockButtonText;
        private string _objectSelectButtonText;
        private string _pathEditingDescriptionText;
        private string _pathEditingButtonText;


        private void OnEnable()
        {
            _presenter = new HCMainPresenter(this);
            logo = EditorGUIUtility.Load("Assets/HexaCraft/Resources/HexaCraftLogo.png") as Texture;
        }

        private void OnDisable()
        {
            // _presenter.Dispose();
        }

        private void Init()
        {
            _mainHeaderStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 18,
                fixedHeight = 30f,
                alignment = TextAnchor.MiddleCenter
            };

            _subHeaderStyle = new GUIStyle(EditorStyles.boldLabel)
            {
                fontSize = 15,
                fixedHeight = 20f,
                alignment = TextAnchor.MiddleLeft
            };
        }

        public override void OnInspectorGUI()
        {
            Init();

            //GUILayout.Label(logo);
            DrawCustomEditorHeader(_MainHeaderSpace, "HexaCraftEditor", _mainHeaderStyle);
            DrawGridGeneratingUI(_SubHeaderSpace, "HexaGrid Generation", _subHeaderStyle);
            DrawMaterialEditingUI(_SubHeaderSpace, "Material Changer", _subHeaderStyle);
            DrawObjectSelector(_SubHeaderSpace, "Object Selector", _subHeaderStyle);
            DrawPathEditingUI(_SubHeaderSpace, "Path Editing", _subHeaderStyle);
        }

        private void DrawCustomEditorHeader(float spaceSize, string label, GUIStyle style)
        {
            GUILayout.Space(spaceSize);
            GUILayout.Label(label, style);
        }

        // Grid Generation UI
        private void DrawGridGeneratingUI(float spaceSize, string label, GUIStyle style)
        {
            DrawCustomEditorHeader(spaceSize, label, style);

            var newHexPrefab = (GameObject)EditorGUILayout.ObjectField("Hex Prefab", _hexPrefab, typeof(GameObject), false);
            if (newHexPrefab != _hexPrefab)
            {
                _hexPrefab = newHexPrefab;
                HexPrefabChanged?.Invoke(_hexPrefab);
            }

            var newGridRadius = EditorGUILayout.IntField("Grid Radius", _gridRadius);
            if (newGridRadius != _gridRadius)
            {
                _gridRadius = newGridRadius;
                GridRadiusChanged?.Invoke(_gridRadius);
            }

            var newHexSize = EditorGUILayout.FloatField("Hex Model Radius", _hexCircumscribedRadiusSize);
            if (newHexSize != _hexCircumscribedRadiusSize)
            {
                _hexCircumscribedRadiusSize = newHexSize;
                HexSizeChanged?.Invoke(_hexCircumscribedRadiusSize);
            }

            if (GUILayout.Button("Generate Grid"))
            {
                ButtonClicked?.Invoke(Button.GridGeneration);
            }
        }

        // Material Changer UI
        public void UpdateMaterialButtonText(string text)
        {
            _materialButtonText = text;
            Repaint();
        }

        private void DrawMaterialEditingUI(float spaceSize, string label, GUIStyle style)
        {
            DrawCustomEditorHeader(spaceSize, label, style);

            var newMaterial = (Material)EditorGUILayout.ObjectField("Select Material", _material, typeof(Material), false);
            if (newMaterial != _material)
            {
                _material = newMaterial;
                MaterialChanged?.Invoke(_material);
            }

            if (GUILayout.Button(_materialButtonText))  // Use stored text
            {
                ToggleButtonClicked?.Invoke(ToggleButton.MaterialChange);
            }
        }

        // Object Selector UI
        public void UpdateInspectorLockButtonText(string text)
        {
            _inspectorLockButtonText = text;
            Repaint();
        }

        public void UpdateObjectSelectButtonText(string text)
        {
            _objectSelectButtonText = text;
            Repaint();
        }

        private void DrawObjectSelector(float spaceSize, string label, GUIStyle style)
        {
            DrawCustomEditorHeader(spaceSize, label, style);

            Rect availableRect = EditorGUILayout.GetControlRect(false, _HorizonButtonHegiht);
            float totalWidth = availableRect.width;
            float firstButtonWidth = totalWidth * 0.3f;
            float secondButtonWidth = totalWidth * 0.7f;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button)
            {
                alignment = TextAnchor.MiddleCenter
            };

            if (GUI.Button(new Rect(availableRect.x, availableRect.y, firstButtonWidth, _HorizonButtonHegiht),
                _inspectorLockButtonText, buttonStyle))
            {
                InspectorLockRequested?.Invoke();
            }

            if (GUI.Button(new Rect(availableRect.x + firstButtonWidth, availableRect.y, secondButtonWidth, _HorizonButtonHegiht),
                _objectSelectButtonText, buttonStyle))
            {
                ToggleButtonClicked?.Invoke(ToggleButton.ObjectSelection);
            }
        }


        // Path Editing UI
        public void UpdatePathEditingDescriptionText(string text)
        {
            _pathEditingDescriptionText = text;
            Repaint();   
        }

        public void UpdatePathEditingButtonText(string text)
        {
            _pathEditingButtonText = text;
            Repaint();
        }

        public void UpdatePathEditingState(PathEditingState newState)
        {
            _currentPathEditingState = newState;
            Repaint();
        }

        private void DrawPathEditingUI(float spaceSize, string label, GUIStyle style)
        {
            DrawCustomEditorHeader(spaceSize, label, style);

            EditorGUILayout.HelpBox(_pathEditingDescriptionText, MessageType.Info);

            if (_currentPathEditingState == PathEditingState.ReadyToGenerate)
            {
                var newPathPrefab = (GameObject)EditorGUILayout.ObjectField("Path Prefab", _pathPrefab, typeof(GameObject), true);
                if (newPathPrefab != _pathPrefab)
                {
                    _pathPrefab = newPathPrefab;
                    PathGridPrefabChanged?.Invoke(_pathPrefab);
                }
            }

            if (GUILayout.Button(_pathEditingButtonText))
            {
                if (_currentPathEditingState != PathEditingState.ReadyToGenerate)
                {
                    ToggleButtonClicked?.Invoke(ToggleButton.PathEditing);
                }
                else
                {
                    ToggleButtonClicked?.Invoke(ToggleButton.PathEditing);
                    ButtonClicked?.Invoke(Button.PathGeneration);
                }
            }
        }
    }
}

