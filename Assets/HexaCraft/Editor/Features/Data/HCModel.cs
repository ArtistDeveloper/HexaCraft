using System;
using System.Collections.Generic;
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

        public void Init()
        {
            _isToggleActives = new Dictionary<ToggleButton, bool>();

            // 각 토글 타입에 대한 초기화
            foreach (ToggleButton toggle in Enum.GetValues(typeof(ToggleButton)))
            {
                _isToggleActives[toggle] = false;
            }
        }

        public void ChangeToggleState(ToggleButton type, bool isModeActive)
        {
            // 다른 토글들을 비활성화
            // if (isModeActive)
            // {
            //     foreach (var toggle in _isToggleActives.Keys.ToList())
            //     {
            //         if (toggle != type)
            //         {
            //             _isToggleActives[toggle] = false;
            //         }
            //     }
            // }

            _isToggleActives[type] = isModeActive;
            _selectedObjects.Clear();
            
            // 토글이 비활성화될 때 선택된 오브젝트들 초기화
            // if (!isModeActive)
            // {
            //     _selectedObjects.Clear();
            // }
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
