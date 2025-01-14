using System;

namespace HexaCraft
{
    public class BinaryToggleState : IToggleState
    {
        private bool _isActive;

        public void UpdateState()
        {
            _isActive = !_isActive;
        }

        public bool IsActive()
        {
            return _isActive;
        }

        public Object GetState()
        {
            return _isActive;
        }
    }
}
