using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    // Model에는 공용으로 접근할 상태만 저장하는 것이 Prsetner와 연결된 로직의 복잡성을 줄여준다.
    // 각 상태를 따로 관리해도 된다면, Model에 무조건 위임시키지는 말자.
    public class HCModel
    {
        private Dictionary<ToggleMode, bool> _isToggleActives;

        private Material _selectedMaterial = null;

        private List<GameObject> _selectedObjects = new List<GameObject>();


        public List<GameObject> SelectedObjects { get => _selectedObjects; }

        public HexGridGenerator HexGenerator { get; } = new HexGridGenerator();

        public Material SelectedMaterial { get => _selectedMaterial; set => _selectedMaterial = value; }


        public void Init()
        {
            _isToggleActives = new Dictionary<ToggleMode, bool>()
            {
                { ToggleMode.MaterialEditing, false },
                { ToggleMode.ObjectSelecting, false },
                { ToggleMode.InspectorLocking, false }
            };
        }

        public void ChangeToggleState(ToggleMode type, bool isModeActive)
        {
            _isToggleActives[type] = isModeActive;
            _selectedObjects = new List<GameObject>(); // TODO: 추후 해당 부분도 커맨드 패턴 도입할 때 같이 리팩토링 필요
        }

        public bool CheckModeActive(ToggleMode type)
        {
            if (_isToggleActives.TryGetValue(type, out bool isActive))
            {
                return isActive;
            }
            else
            {
                throw new Exception("No ToggleMode");
            }
        }

        public void AddSelectedObject(GameObject go)
        {
            SelectedObjects.Add(go);
        }

        public void RemoveSelectedObject(GameObject go)
        {
            SelectedObjects.Remove(go);
        }
    }
}
