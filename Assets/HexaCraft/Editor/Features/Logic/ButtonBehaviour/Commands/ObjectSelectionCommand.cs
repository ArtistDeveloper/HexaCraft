using HexaCraft;
using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class ObjectSelectionCommand : ISceneCommand
    {
        private ObjectSelection _receiver;

        private bool _isPressed;

        public ObjectSelectionCommand(ObjectSelection receiver)
        {
            _receiver = receiver;
        }

        public void Execute(SceneView sceneView, Event evt)
        {
            if (evt.button == 1)
                return;

            if (evt.type == EventType.MouseDown && !_isPressed)
            {
                evt.Use();
                _receiver.ApplySelectGameObject(evt);
                _isPressed = true;
            }
            else if (evt.type == EventType.MouseDrag && _isPressed)
            {
                evt.Use();
                _receiver.ApplySelectGameObject(evt);
            }
            else if (evt.type == EventType.MouseUp)
            {
                _isPressed = false;
            }
        }
    }
}
