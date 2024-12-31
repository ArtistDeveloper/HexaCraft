using UnityEngine;
using UnityEditor;

public class ButtonAction
{
    public void ToggleInspectorLock()
    {
        ActiveEditorTracker.sharedTracker.isLocked = !ActiveEditorTracker.sharedTracker.isLocked;
    }
}
