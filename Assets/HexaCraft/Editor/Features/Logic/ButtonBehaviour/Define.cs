using UnityEngine;

namespace HexaCraft
{
    public enum PathEditingState
    {
        Idle,

        SelectingStart,

        SelectingGoal,

        ReadyToGenerate
    }

    public enum ToggleButton
    {
        [SceneRegistration(true)]
        MaterialChange,

        [SceneRegistration(true)]
        ObjectSelection,

        [SceneRegistration(true)]
        PathEditing,
    }

    public enum Button
    {
        GridGeneration,

        PathGeneration,
    }
}
