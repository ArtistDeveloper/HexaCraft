using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using HexaCraft;
using static UnityEngine.Rendering.DebugUI.MessageBox;

namespace HexaCraft
{
    [CustomEditor(typeof(HCGeneration))]
    public class HCGenerationEditor : Editor
    {
        private HCPresenter _presenter;

        private GUIStyle _mainHeaderStyle;

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
        #endregion

        private void OnEnable()
        {
            _presenter = new HCPresenterFactory().CreatePresenter(this) as HCPresenter;
            logo = EditorGUIUtility.Load("Assets/HexaCraft/Resources/HexaCraftLogo.png") as Texture;
        }

        private void OnDisable()
        {
            _presenter.Dispose();
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

        private string GetPathStateDescription(PathEditingState state)
        {
            return state switch
            {
                PathEditingState.Idle => "Click 'Start Path Editing' to begin",
                PathEditingState.SelectingStart => "Click on a hex to set the start point",
                PathEditingState.SelectingGoal => "Click on a hex to set the goal point",
                PathEditingState.ReadyToGenerate => "Ready to generate path",
                _ => ""
            };
        }

        private string GetToggleButtonText(string original, string changed, ToggleButton type)
        {
            return _presenter.GetToggleActiveState(type) ? original : changed;
        }

        private string GetInspectorLockButtonText()
        {
            return _presenter.IsInspectorLocked() ? "Locked Inspector" : "Unlocked Inspector";
        }

        private void DrawGridGeneratingUI(float spaceSize, string label, GUIStyle style)
        {
            DrawCustomEditorHeader(spaceSize, label, style);

            _hexPrefab = (GameObject)EditorGUILayout.ObjectField("Hex Prefab", _hexPrefab, typeof(GameObject), false);
            _gridRadius = EditorGUILayout.IntField("Grid Radius", _gridRadius);
            _hexCircumscribedRadiusSize = EditorGUILayout.FloatField("Hex Model Radius", _hexCircumscribedRadiusSize);
            _presenter.SetGridGenerationValues(_hexPrefab, _gridRadius, _hexCircumscribedRadiusSize);

            if (GUILayout.Button("Generate Grid"))
            {
                _presenter.OnButtonClicked(Button.GridGeneration);
            }
        }

        private void DrawMaterialEditingUI(float spaceSize, string label, GUIStyle style)
        {
            DrawCustomEditorHeader(spaceSize, label, style);

            _material = (Material)EditorGUILayout.ObjectField("Select Material", _presenter.GetSelectionMaterial(), typeof(Material), false);
            _presenter.SetSelctionMaterial(_material);

            if (GUILayout.Button(GetToggleButtonText("Finish Change Material", "Change Material", ToggleButton.MaterialChange)))
            {
                _presenter.OnToggleClicked(ToggleButton.MaterialChange);
            }
        }

        private void DrawObjectSelector(float spaceSize, string label, GUIStyle style)
        {
            DrawCustomEditorHeader(spaceSize, label, style);

            // 표준 컨트롤 영역 얻기
            Rect availableRect = EditorGUILayout.GetControlRect(false, _HorizonButtonHegiht);
            float totalWidth = availableRect.width;
            float firstButtonWidth = totalWidth * 0.3f;
            float secondButtonWidth = totalWidth * 0.7f;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.alignment = TextAnchor.MiddleCenter;

            // 얻은 Rect의 위치를 기준으로 버튼 배치
            if (GUI.Button(new Rect(availableRect.x, availableRect.y, firstButtonWidth, _HorizonButtonHegiht),
                GetInspectorLockButtonText(), buttonStyle))
            {
                _presenter.OnInspectorLockClicked();
            }

            if (GUI.Button(new Rect(availableRect.x + firstButtonWidth, availableRect.y, secondButtonWidth, _HorizonButtonHegiht),
                GetToggleButtonText("Finish Object Selector", "Select Object", ToggleButton.ObjectSelection), buttonStyle))
            {
                _presenter.OnToggleClicked(ToggleButton.ObjectSelection);
            }
        }

        private void DrawPathEditingUI(float spaceSize, string label, GUIStyle style)
        {
            DrawCustomEditorHeader(spaceSize, label, style);

            var pathEditingState = _presenter.GetPathEditingState();
            string mainButtonText = pathEditingState switch
            {
                PathEditingState.Idle => "Start Path Editing",
                PathEditingState.SelectingStart => "Select Start Point...",
                PathEditingState.SelectingGoal => "Select Goal Point...",
                PathEditingState.ReadyToGenerate => "Generate Path",
                _ => "Start Path Editing"
            };

            // 현재 상태 표시
            EditorGUILayout.HelpBox(GetPathStateDescription(pathEditingState), MessageType.Info);

            if (GUILayout.Button(mainButtonText))
            {
                _presenter.OnToggleClicked(ToggleButton.PathEditing);
            }
        }
    }
}

