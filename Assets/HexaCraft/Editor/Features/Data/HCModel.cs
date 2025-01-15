using System;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace HexaCraft
{
    public struct PathPair
    {
        public GameObject Start { get; }
        public GameObject Goal { get; }

        public PathPair(GameObject start, GameObject goal)
        {
            Start = start;
            Goal = goal;
        }
    }

    // TODO:  추후 SceneView, Inspector 역할 분리하여 Model 클래스 분리 필요
    public class HCModel
    {
        // SceneView Model
        private Dictionary<ToggleButton, IToggleState> _toggleStates;

        private Material _selectedMaterial;

        private List<GameObject> _selectedObjects = new List<GameObject>();

        private List<PathPair> _pathPairs = new List<PathPair>();

        private Dictionary<GameObject, Material> _originalMaterials = new Dictionary<GameObject, Material>();

        // 현재 작업 중인 시작점 (임시 저장용)
        private GameObject _currentStartPoint;


        // Inspector Model
        private GameObject _hexPrefab;

        private int _gridRadius;

        private float _hexCircumscribedRadiusSize;


        public List<GameObject> SelectedObjects { get => _selectedObjects; }

        public Material SelectedMaterial { get => _selectedMaterial; set => _selectedMaterial = value; }

        public GameObject HexPrefab { get => _hexPrefab; set => _hexPrefab = value; }

        public int GridRadius { get => _gridRadius; set => _gridRadius = value; }

        public float HexCircumscribedRadius { get => _hexCircumscribedRadiusSize; set => _hexCircumscribedRadiusSize = value; }


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

        public bool CheckToggleActive(ToggleButton type)
        {
            if (_toggleStates.TryGetValue(type, out IToggleState toggleState))
            {
                return toggleState.IsActive();
            }

            throw new Exception("ToggleState doesen't exist");
        }

        public T GetToggleState<T>(ToggleButton type)
        {
            if (_toggleStates.TryGetValue(type, out IToggleState state))
            {
                return (T)state.GetState();
            }
            throw new Exception($"Toggle state not found for type: {type}");
        }

        public void AddSelectedObject(GameObject go)
        {
            _selectedObjects.Add(go);
        }

        public void RemoveSelectedObject(GameObject go)
        {
            _selectedObjects.Remove(go);
        }

        // public void AddStartPoint(GameObject point)
        // {
        //     if (point == null) return;
        //     SaveOriginalMaterial(point);
        //     _currentStartPoint = point;
        // }

        // public void AddGoalPoint(GameObject point)
        // {
        //     if (point == null || _currentStartPoint == null) return;
        //     SaveOriginalMaterial(point);

        //     _pathPairs.Add(new PathPair(_currentStartPoint, point));
        //     _currentStartPoint = null;  // 새 쌍 생성 후 리셋
        // }

        // private void SaveOriginalMaterial(GameObject obj)
        // {
        //     var renderer = obj.GetComponent<Renderer>();
        //     if (renderer && !_originalMaterials.ContainsKey(obj))
        //     {
        //         _originalMaterials[obj] = renderer.sharedMaterial;
        //     }
        // }
    }
}
