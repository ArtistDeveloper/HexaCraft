using UnityEngine;

namespace HexaCraft
{
    public class BinaryToggleState : IToggleState
    {
        private bool _isActive;
        private bool _targetState;

        public void UpdateState()
        {
            _isActive = !_isActive;
        }

        public bool IsActive()
        {
            return _isActive;
        }
    }
}
