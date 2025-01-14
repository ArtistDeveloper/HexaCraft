using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace HexaCraft
{
    public class HCPresenter : IPresenter
    {
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
            _buttonActionClient = new ButtonActionClient(this);
        }

        public void Dispose()
        {

        }

        public void SetGridGenerationValues(GameObject hexPrefab, int n, float hexSize)
        {
            _model.HexPrefab = hexPrefab;
            _model.GridRadius = n;
            _model.HexCircumscribedRadius = hexSize;
        }

        public void OnToggleClicked(ToggleButton type)
        {
            _buttonActionClient.ButtonClicked(type);
            SetModeActive(type);
        }

        public void OnButtonClicked(Button type)
        {
            _buttonActionClient.ButtonClicked(type);
        }

        public void OnPathEditingButtonClicked()
        {

        }

        public void OnInspectorLockClicked()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
            _view.Repaint();
        }

        public bool IsInspectorLocked()
        {
            return ActiveEditorTracker.sharedTracker.isLocked;
        }

        public GameObject GetHexPrefab() => _model.HexPrefab;

        public int GetGridRadius() => _model.GridRadius;

        public float GetHexCircumscribedRadius() => _model.HexCircumscribedRadius;

        public Material GetSelectionMaterial() => _model.SelectedMaterial;

        public List<GameObject> GetSelectedObjects() => _model.SelectedObjects;

        public void AddSelectedObject(GameObject go) => _model.AddSelectedObject(go);

        public void SetSelctionMaterial(Material material) => _model.SelectedMaterial = material;

        public bool CheckModeActive(ToggleButton type) => _model.CheckModeActive(type);

        public void SetModeActive(ToggleButton type) => _model.ChangeToggleState(type);

        public PathEditingState GetPathEditingState() => _model.PathEditingState;
    }
}