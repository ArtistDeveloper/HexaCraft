using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using static Codice.Client.BaseCommands.BranchExplorer.Layout.BrExLayout;

namespace HexaCraft
{
    public enum ToggleMode
    {
        MaterialEditing,
        ObjectSelecting,
        InspectorLocking,
    }

    public class HCPresenter : IPresenter
    {
        private HCGenerationEditor _view;
        private HCModel _model;
        private SceneInteractor _sceneInteractor;
        private ButtonAction _buttonAction;

        public HCPresenter(HCGenerationEditor view)
        {
            _view = view;

            Init();
        }

        private void Init()
        {
            _model = new HCModel();
            _model.Init();

            _sceneInteractor = new SceneInteractor(this);
            _sceneInteractor.OnObjectClicked += HandleObjectClicked;

            _buttonAction = new ButtonAction();
        }

        public void OnGenerateGridClicked(int n, GameObject hexPrefab, float hexSize)
        {
            if (hexPrefab == null)
            {
                UnityEngine.Debug.LogError("Hex Prefab cannot be null.");
                return;
            }

            if (n <= 0 || hexSize <= 0)
            {
                UnityEngine.Debug.LogError("Grid size and hex size must be greater than zero");
                return;
            }

            _model.HexGenerator.GenerateGrid(n, hexPrefab, hexSize);
        }

        private void HandleObjectClicked(GameObject go)
        {
            if (!_model.SelectedObjects.Contains(go))
            {
                _model.AddSelectedObject(go);
            }

            _view.UpdateSelectedObjects(_model.SelectedObjects);
        }


        // TODO: 업데이트 시 해당 내용 리팩토링. 일반 버튼과 토글 버튼을 전체적으로 감싸는 커맨드 필요
        // Toggle 버튼이라도 SceneAction 래핑이 필요하지 않은 경우가 존재한다.
        public void OnToggleInspectorLock(ToggleMode type)
        {
            _buttonAction.ToggleInspectorLock();
            SetModeActive(type, !CheckModeActive(type));
        }

        public void OnToggle(ToggleMode type)
        {
            _sceneInteractor.RegisterActionToSceneView(type);
            SetModeActive(type, !CheckModeActive(type));
        }

        public void OnClearGridClicked()
        {
            _model.HexGenerator.ClearGrid();
        }

        public Material GetSelectionMaterial()
        {
            return _model.SelectedMaterial;
        }

        public List<GameObject> GetSelectedObjects()
        {
            return _model.SelectedObjects;
        }

        public void AddSelectedObject(GameObject go)
        {
            _model.AddSelectedObject(go);
        }

        public void RemoveSelectedObject(GameObject go)
        {
            _model.RemoveSelectedObject(go);
        }

        public void SetSelctionMaterial(Material material)
        {
            _model.SelectedMaterial = material;
        }

        public bool CheckModeActive(ToggleMode type)
        {
            return _model.CheckModeActive(type);
        }

        public void SetModeActive(ToggleMode type, bool isModeActive)
        {
            _model.ChangeToggleState(type, isModeActive);
        }

        public void ClearAllActionToSceneView()
        {
            _sceneInteractor.ClearAllActionToSceneView();
        }
    }
}