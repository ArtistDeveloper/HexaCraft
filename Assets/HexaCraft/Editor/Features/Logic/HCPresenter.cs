using System.Reflection;
using System.Collections.Generic;
using UnityEngine;

namespace HexaCraft
{
    public class HCPresenter : IPresenter
    {
        private HCGenerationEditor _view;
        private HCModel _model;
        private SceneInteractor _sceneInteractor;

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
            _sceneInteractor.OnMaterialEditingRequested += HandleMaterialEditing;
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

        // MaterialEditing이 일으킨 이벤트 처리
        private void HandleMaterialEditing(GameObject go)
        {
            _view.UpdateSelectedMaterial(go, _model.SelectedMaterial);
        }

        public void OnToggleClicked(ToggleButton type)
        {
            bool requiresSceneRegistration = IsSceneRegistrationRequire(type);

            if (requiresSceneRegistration)
                _sceneInteractor.RegisterActionToSceneView(type);

            SetModeActive(type, !CheckModeActive(type));
        }

        public void OnButtonClicked()
        {

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

        public void SetSelctionMaterial(Material material)
        {
            _model.SelectedMaterial = material;
        }

        public bool CheckModeActive(ToggleButton type)
        {
            return _model.CheckModeActive(type);
        }

        public void SetModeActive(ToggleButton type, bool isModeActive)
        {
            _model.ChangeToggleState(type, isModeActive);
        }

        public void ClearAllActionToSceneView()
        {
            _sceneInteractor.ClearAllActionToSceneView();
        }

        private bool IsSceneRegistrationRequire(ToggleButton type)
        {
            var memberInfo = type.GetType().GetMember(type.ToString())[0];
            var attribute = memberInfo.GetCustomAttribute<SceneRegistrationAttribute>();
            return attribute?.RequiresRegistration ?? false;
        }
    }
}