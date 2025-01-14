using System;

namespace HexaCraft
{
    public interface IToggleState
    {
        void UpdateState();
        bool IsActive();
        Object GetState();
    }
}
