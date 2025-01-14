using UnityEngine;

namespace HexaCraft
{
    public class PathEditingToggleState : IToggleState
    {
        private PathEditingState CurrentState {get; set; } = PathEditingState.Idle;

        public void UpdateState()
        {
            CurrentState = CurrentState switch
            {
                PathEditingState.Idle => PathEditingState.SelectingStart,
                PathEditingState.SelectingStart => PathEditingState.SelectingGoal,
                PathEditingState.SelectingGoal => PathEditingState.ReadyToGenerate,
                PathEditingState.ReadyToGenerate => PathEditingState.Idle,
                _ => PathEditingState.Idle,
            };
        }

        public bool IsActive()
        {
            return CurrentState != PathEditingState.Idle;
        }
    }
}
