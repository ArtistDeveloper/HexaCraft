using System;
using UnityEngine;
using UnityEditor;
using PlasticGui.WorkspaceWindow.Items;

namespace HexaCraft
{
    public class HCMainPresenter
    {
        IHCMainView _mainView;

        private HCModel _model;

        private ButtonActionClient _buttonActionClient;

        public event Action disposeAction;

        public HCMainPresenter(IHCMainView view)
        {
            _mainView = view;
            _mainView.HexPrefabChanged += OnHexPrefabChanged;
            _mainView.GridRadiusChanged += OnGridRadiusChanged;
            _mainView.HexSizeChanged += OnHexSizeChanged;
            _mainView.MaterialChanged += OnMaterialChanged;
            _mainView.PathGridPrefabChanged += OnPathGridPrefabChanged;
            _mainView.ToggleButtonClicked += OnToggleClicked;
            _mainView.ButtonClicked += OnButtonClicked;
            _mainView.InspectorLockRequested += OnInspectorLockClicked;
            _mainView.InspectorLockRequested += UpdateInspectorLockButtonText;

            Init();
        }

        private void Init()
        {
            _model = new HCModel();

            _mainView.UpdateMaterialButtonText("Change Material");
            _mainView.UpdateInspectorLockButtonText("Locked Inspector");
            _mainView.UpdateObjectSelectButtonText("Select Object");
            _mainView.UpdatePathEditingDescriptionText("Click 'Start Path Editing to begin");
            _mainView.UpdatePathEditingButtonText("Start Path Editing");
            // _buttonActionClient = new ButtonActionClient(this);
        }

        public void Dispose()
        {
            _mainView.HexPrefabChanged -= OnHexPrefabChanged;
            _mainView.GridRadiusChanged -= OnGridRadiusChanged;
            _mainView.HexSizeChanged -= OnHexSizeChanged;
            _mainView.MaterialChanged -= OnMaterialChanged;
            _mainView.PathGridPrefabChanged -= OnPathGridPrefabChanged;
            _mainView.ToggleButtonClicked -= OnToggleClicked;
            _mainView.ButtonClicked -= OnButtonClicked;
            _mainView.InspectorLockRequested -= OnInspectorLockClicked;
            _mainView.InspectorLockRequested -= UpdateInspectorLockButtonText;

            disposeAction.Invoke();
        }

        #region inspector property changed
        private void OnHexPrefabChanged(GameObject hexPrefab)
        {
            _model.HexPrefab = hexPrefab;
        }

        private void OnGridRadiusChanged(int radius)
        {
            _model.GridRadius = radius;
        }

        private void OnHexSizeChanged(float size)
        {
            _model.HexCircumscribedRadius = size;
        }

        private void OnMaterialChanged(Material material)
        {
            _model.SelectedMaterial = material;
        }

        private void OnPathGridPrefabChanged(GameObject pathPrefab)
        {
            _model.RootGrid = pathPrefab;
        }
        #endregion

        #region Button Click
        public void OnToggleClicked(ToggleButton type)
        {
            UpdateToggleState(type);
            // Update button text based on toggle type
            switch (type)
            {
                case ToggleButton.MaterialChange:
                    UpdateMaterialButtonText();
                    break;
                case ToggleButton.ObjectSelection:
                    UpdateObjectSelectButtonText();
                    break;
                case ToggleButton.PathEditing:
                    UpdatePathEditingButtonText();
                    break;
            }
            _buttonActionClient.ToggleButtonClicked(type);
        }

        public void OnButtonClicked(Button type)
        {
            _buttonActionClient.ButtonClicked(type);
        }

        public void OnInspectorLockClicked()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        }
        #endregion


        #region Update UI
        private void UpdateMaterialButtonText()
        {
            UpdateToggleButtonText(ToggleButton.MaterialChange, "Finish Change Material", "Change Material",
                text => _mainView.UpdateMaterialButtonText(text));
        }

        private void UpdateObjectSelectButtonText()
        {
            UpdateToggleButtonText(ToggleButton.ObjectSelection, "Finish Object Selector", "Select Object",
                text => _mainView.UpdateObjectSelectButtonText(text));
        }

        private void UpdateInspectorLockButtonText()
        {
            _mainView.UpdateInspectorLockButtonText(IsInspectorLocked() ? "Locked Inspector" : "Unlocked Inspector");
        }

        public void UpdatePathEditingButtonText()
        {
            var pathEditingState = GetPathEditingState();

            string descriptionText = pathEditingState switch
            {
                PathEditingState.Idle => "Click 'Start Path Editing' to begin",
                PathEditingState.SelectingStart => "Click on a hex to set the start point",
                PathEditingState.SelectingGoal => "Click on a hex to set the goal point",
                PathEditingState.ReadyToGenerate => "Ready to generate path",
                _ => ""
            };
            string buttonText = pathEditingState switch
            {
                PathEditingState.Idle => "Start Path Editing",
                PathEditingState.SelectingStart => "Select Start Point...",
                PathEditingState.SelectingGoal => "Select Goal Point...",
                PathEditingState.ReadyToGenerate => "Generate Path",
                _ => "Start Path Editing"
            };

            _mainView.UpdatePathEditingDescriptionText(descriptionText);
            _mainView.UpdatePathEditingButtonText(buttonText);
        }

        private void UpdateToggleButtonText(ToggleButton toggleType, string activeText, string inactiveText, Action<string> updateAction)
        {
            bool isActive = _model.CheckToggleActive(toggleType);
            string text = isActive ? activeText : inactiveText;
            updateAction(text);
        }
        #endregion
        

        #region Get or Update Model States
        public bool IsInspectorLocked()
        {
            return ActiveEditorTracker.sharedTracker.isLocked;
        }

        public void UpdateToggleState(ToggleButton type) => _model.ChangeToggleState(type);

        public PathEditingState GetPathEditingState() => _model.GetToggleState<PathEditingState>(ToggleButton.PathEditing);
        #endregion
    }
}