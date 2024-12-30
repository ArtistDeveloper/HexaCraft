using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using HexaCraft;
using static UnityEngine.Rendering.DebugUI.MessageBox;

namespace HexaCraft
{
    [CustomEditor(typeof(HCGeneration))]
    public class HCGenerationEditor : Editor, IView
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

        private int _n;

        private float _hexSize;

        private Material _material;
        #endregion

        // 테스트
        private void OnEnable()
        {
            _presenter = new HCPresenterFactory().CreatePresenter(this) as HCPresenter;
            logo = EditorGUIUtility.Load("Assets/HexaCraft/Resources/HexaCraftLogo.png") as Texture;
        }

        private void OnDisable()
        {
            _presenter.ClearAllActionToSceneView();
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

        private string GetToggleButtonText(string original, string changed, ToggleMode type)
        {
            return _presenter.CheckModeActive(type) ? original : changed;
        }

        private void DrawGridGeneratingUI(float spaceSize, string label, GUIStyle style)
        {
            DrawCustomEditorHeader(spaceSize, label, style);

            _hexPrefab = (GameObject)EditorGUILayout.ObjectField("Hex Prefab", _hexPrefab, typeof(GameObject), false);
            _n = EditorGUILayout.IntField("N", _n);
            _hexSize = EditorGUILayout.FloatField("HexSize", _hexSize);

            if (GUILayout.Button("Generate Grid"))
            {
                _presenter.OnGenerateGridClicked(_n, _hexPrefab, _hexSize);
            }

            if (GUILayout.Button("Clear Grid"))
            {
                _presenter.OnClearGridClicked();
            }
        }

        private void DrawMaterialEditingUI(float spaceSize, string label, GUIStyle style)
        {
            DrawCustomEditorHeader(spaceSize, label, style);

            _material = (Material)EditorGUILayout.ObjectField("Select Material", _presenter.GetSelectionMaterial(), typeof(Material), false);
            _presenter.SetSelctionMaterial(_material);
            
            if (GUILayout.Button(GetToggleButtonText("Finish Change Material", "Change Material", ToggleMode.MaterialEditing)))
            {
                _presenter.OnToggle(ToggleMode.MaterialEditing);
            }
        }

        private void DrawObjectSelector(float spaceSize, string label, GUIStyle style)
        {
            DrawCustomEditorHeader(spaceSize, label, style);

            float totalWidth = EditorGUIUtility.currentViewWidth;
            float firstButtonWidth = totalWidth * 0.3f;
            float secondButtonWidth = totalWidth * 0.7f;

            GUIStyle buttonStyle = new GUIStyle(GUI.skin.button);
            buttonStyle.alignment = TextAnchor.MiddleCenter;

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(
                GetToggleButtonText("Lock Inspector", "Unlock Inspector", ToggleMode.InspectorLocking),
                buttonStyle, GUILayout.Width(firstButtonWidth), GUILayout.Height(_HorizonButtonHegiht)))
            {
                _presenter.OnToggle(ToggleMode.InspectorLocking);
            }
                        
            if (GUILayout.Button(
                GetToggleButtonText("Finish Object Selector", "Select Object", ToggleMode.ObjectSelecting),
                buttonStyle, GUILayout.Width(secondButtonWidth), GUILayout.Height(_HorizonButtonHegiht))
                )
            {
                _presenter.OnToggle(ToggleMode.ObjectSelecting);
            }

            GUILayout.EndHorizontal();
        }

        private void DrawPathEditingUI(float spaceSize, string label, GUIStyle style)
        {
            DrawCustomEditorHeader(spaceSize, label, style);

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("Select Start Point"))
            {

            }

            if (GUILayout.Button("Selecte Goal Point"))
            {

            }

            if (GUILayout.Button("Generate Path"))
            {

            }

            GUILayout.EndHorizontal();
        }
    }
}
