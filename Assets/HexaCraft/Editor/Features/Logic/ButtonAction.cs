using UnityEngine;
using UnityEditor;

namespace HexaCraft
{
    public class ButtonAction
    {
        public void ToggleInspectorLock()
        {
            ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
        }
    }
}
