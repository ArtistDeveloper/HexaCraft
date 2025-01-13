using System.Reflection;
using System.Collections.Generic;
using UnityEngine;
using System;

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
            _model.Init();

            _buttonActionClient = new ButtonActionClient(this);
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
            // _buttonActionClient.ButtonClicked(type);
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

        public void SetModeActive(ToggleButton type, bool isModeActive)
        {
            _model.ChangeToggleState(type, isModeActive);
        }

        public void Dispose()
        {
            
        }
    }
}