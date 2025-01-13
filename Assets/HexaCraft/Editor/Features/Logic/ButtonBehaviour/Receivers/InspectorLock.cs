using System.Runtime.Hosting;
using UnityEditor;

namespace HexaCraft
{
    public class InspectorLock
    {
        private bool _isActive = false;
        
        public void LockInspector()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !_isActive;
            _isActive = !_isActive;
            // ActiveEditorTracker.sharedTracker.ForceRebuild();
        }
    }
}
