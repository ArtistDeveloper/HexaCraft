using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class HCModel
    {
        private Dictionary<ToggleButton, bool> _isToggleActives;
        private Material _selectedMaterial = null;
        private List<GameObject> _selectedObjects = new List<GameObject>();

        public List<GameObject> SelectedObjects { get => _selectedObjects; }
        public Material SelectedMaterial { get => _selectedMaterial; set => _selectedMaterial = value; }
        public HexGridGenerator HexGenerator { get; } = new HexGridGenerator();

        public HCModel()
        {
            _isToggleActives = new Dictionary<ToggleButton, bool>();

            foreach (ToggleButton toggle in Enum.GetValues(typeof(ToggleButton)))
            {
                _isToggleActives[toggle] = false;
            }
        }

        public void ChangeToggleState(ToggleButton type, bool targetState)
        {
            _isToggleActives[type] = targetState;
            CleanState(type, targetState);
        }

        private void CleanState(ToggleButton type, bool targetState)
        {
            if (targetState)
                return;

            switch (type)
            {
                case ToggleButton.ObjectSelecting:
                    _selectedObjects.Clear();
                    break;
            }
        }

        public bool CheckModeActive(ToggleButton type)
        {
            return _isToggleActives.TryGetValue(type, out bool isActive) ? isActive : false;
        }

        public void AddSelectedObject(GameObject go)
        {
            _selectedObjects.Add(go);
        }

        public void RemoveSelectedObject(GameObject go)
        {
            _selectedObjects.Remove(go);
        }
    }
}
