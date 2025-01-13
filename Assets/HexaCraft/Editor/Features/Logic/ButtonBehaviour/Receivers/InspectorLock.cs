using System.Runtime.Hosting;
using UnityEditor;

namespace HexaCraft
{
    public class InspectorLock
    {
        public void LockInspector()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
            // ActiveEditorTracker.sharedTracker.ForceRebuild();
        }
    }
}
