using System;
using UnityEngine;

namespace HexaCraft
{
    public interface IHCMainView
    {
        event Action<GameObject> HexPrefabChanged;

        event Action<int> GridRadiusChanged;

        event Action<float> HexSizeChanged;

        event Action<Material> MaterialChanged;

        event Action<GameObject> PathGridPrefabChanged;

        event Action<ToggleButton> ToggleButtonClicked;

        event Action<Button> ButtonClicked;
        
        event Action InspectorLockRequested;

        void UpdateMaterialButtonText(string text);

        void UpdateInspectorLockButtonText(string text);

        void UpdateObjectSelectButtonText(string text);

        void UpdatePathEditingDescriptionText(string text);

        void UpdatePathEditingButtonText(string text);
    }
}
