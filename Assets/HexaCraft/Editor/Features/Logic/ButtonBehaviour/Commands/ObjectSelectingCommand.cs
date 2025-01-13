using HexaCraft;
using UnityEditor;
using UnityEngine;

public class ObjectSelectingCommand : ISceneCommand
{
    private ObjectSelectingReceiver _receiver;

    private bool _isPressed;

    public ObjectSelectingCommand(ObjectSelectingReceiver receiver)
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
