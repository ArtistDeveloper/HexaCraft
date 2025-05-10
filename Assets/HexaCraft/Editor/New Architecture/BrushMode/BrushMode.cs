using UnityEngine;

namespace HexaCraft
{
    public abstract class BrushMode : ScriptableObject
    {
        public abstract void DrawGUI();

        public virtual void OnEnable()
        {

        }

        public virtual void OnBrushEnter(GameObject target)
        {

        }

        public virtual void OnBrushMove()
        {

        }

        public virtual void OnBrushExit() // params : EditableObject target
        {

        }

        public abstract void OnBrushApply(GameObject target);

        public abstract void RegisterUndo(); // BrushTarget brushTarget

        public virtual void DrawGizmos(bool isHoverHextile)
        {
            
        }

        public virtual void UndoRedoPerformed() // List<GameObject> modified
        {
            // DestroyTempComponent();
        }
    }
}