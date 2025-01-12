using UnityEngine;

namespace HexaCraft
{
    public enum ToggleButton
    {
        [SceneRegistration(true)]
        MaterialEditing,

        [SceneRegistration(true)]
        ObjectSelecting,

        [SceneRegistration(false)]
        InspectorLocking,
    }

    public enum Button
    {
        GridGenerating,
    }
}
