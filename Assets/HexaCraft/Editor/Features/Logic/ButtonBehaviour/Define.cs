using UnityEngine;

namespace HexaCraft
{
    public enum ToggleButton
    {
        [SceneRegistration(true)]
        MaterialEditing,

        [SceneRegistration(true)]
        ObjectSelecting,
    }

    public enum Button
    {
        GridGenerating,
    }
}
