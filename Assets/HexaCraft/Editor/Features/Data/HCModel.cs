using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HexaCraft
{
    public class HCModel
    {
        private Dictionary<ToggleButton, IToggleState> _toggleStates;

        private PathEditingState _pathEditingState = PathEditingState.Idle;

        private Material _selectedMaterial;

        private List<GameObject> _selectedObjects = new List<GameObject>();

        private GameObject _hexPrefab;

        private int _gridRadius;

        private float _hexCircumscribedRadiusSize;


        public List<GameObject> SelectedObjects { get => _selectedObjects; }

        public Material SelectedMaterial { get => _selectedMaterial; set => _selectedMaterial = value; }

        public GameObject HexPrefab { get => _hexPrefab; set => _hexPrefab = value; }

        public int GridRadius { get => _gridRadius; set => _gridRadius = value; }

        public float HexCircumscribedRadius { get => _hexCircumscribedRadiusSize; set => _hexCircumscribedRadiusSize = value; }

        public PathEditingState PathEditingState { get => _pathEditingState; set => _pathEditingState = value; }


        public HCModel()
        {
            _toggleStates = new Dictionary<ToggleButton, IToggleState>();

            foreach (ToggleButton toggle in Enum.GetValues(typeof(ToggleButton)))
            {
                _toggleStates[toggle] = CreateToggleState(toggle);
            }
        }

        private void CleanState(ToggleButton type)
        {
            switch (type)
            {
                case ToggleButton.ObjectSelection:
                    _selectedObjects.Clear();
                    break;
            }
        }

        private IToggleState CreateToggleState(ToggleButton type)
        {
            return type switch
            {
                ToggleButton.PathEditing => new PathEditingToggleState(),
                _ => new BinaryToggleState(),
            };
        }

        public void ChangeToggleState(ToggleButton type)
        {
            _toggleStates[type].UpdateState();
            if (!_toggleStates[type].IsActive())
            {
                CleanState(type);
            }
        }

        public bool CheckModeActive(ToggleButton type)
        {
            if (_toggleStates.TryGetValue(type, out IToggleState toggleState))
            {
                return toggleState.IsActive();
            }

            throw new Exception("ToggleState doesen't exist");
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
