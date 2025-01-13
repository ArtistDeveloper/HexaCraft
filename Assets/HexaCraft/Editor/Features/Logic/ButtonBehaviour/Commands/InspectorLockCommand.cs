using UnityEngine;

namespace HexaCraft
{
    public class InspectorLockCommand : IBasicCommand
    {
        private InspectorLock _receiver;

        public InspectorLockCommand(InspectorLock receiver)
        {
            _receiver = receiver;
        }

        public void Execute()
        {
            _receiver.LockInspector();
        }
    }
}
