using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace HexaCraft
{
    public class HCPresenter : IPresenter
    {
        private bool _isInspectorLocked = false;

        private HCGenerationEditor _view;

        private HCModel _model;

        private ButtonActionClient _buttonActionClient;


        public HCPresenter(HCGenerationEditor view)
        {
            _view = view;

            Init();
        }

        private void Init()
        {
            _model = new HCModel();
            _model.Init();

            _buttonActionClient = new ButtonActionClient(this);

            EditorApplication.update += CheckInspectorLockState;

            // 초기 상태 동기화
            _isInspectorLocked = ActiveEditorTracker.sharedTracker.isLocked;
        }

        public void OnGenerateGridClicked(int n, GameObject hexPrefab, float hexSize)
        {
            _model.HexGenerator.GenerateGrid(n, hexPrefab, hexSize);
        }

        public void OnToggleClicked(ToggleButton type)
        {
            _buttonActionClient.ButtonClicked(type);
            SetModeActive(type, !CheckModeActive(type));
        }

        public void OnButtonClicked(Button type)
        {
            _buttonActionClient.ButtonClicked(type);
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

        public void SetSelctionMaterial(Material material)
        {
            _model.SelectedMaterial = material;
        }

        public bool CheckModeActive(ToggleButton type)
        {
            return _model.CheckModeActive(type);
        }

        public void SetModeActive(ToggleButton type, bool targetState)
        {
            _model.ChangeToggleState(type, targetState);
        }

        public void Dispose()
        {
            EditorApplication.update -= CheckInspectorLockState;
        }

        private void CheckInspectorLockState()
        {
            bool currentLockState = ActiveEditorTracker.sharedTracker.isLocked;
            if (_isInspectorLocked != currentLockState)
            {
                _isInspectorLocked = currentLockState;
                _view.Repaint();
            }
        }

        public void OnInspectorLockClicked()
        {
            _isInspectorLocked = !_isInspectorLocked;
            ActiveEditorTracker.sharedTracker.isLocked = _isInspectorLocked;
            _view.Repaint();
        }

        public bool IsInspectorLocked()
        {
            return _isInspectorLocked;
        }
    }
}