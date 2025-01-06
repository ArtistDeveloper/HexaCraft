using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class MaterialEditingCommand : ICommand
    {
        public MaterialEditingReceiver _receiver;

        public MaterialEditingCommand(MaterialEditingReceiver receiver)
        {
            _receiver = receiver;
        }

        public void Execute(SceneView sceneView, Event evt)
        {
            if (evt.button == 1)
                return;

            if (evt.type == EventType.MouseDown)
            {
                evt.Use();
                _receiver.ApplyMaterialChange(evt);
            }
            else if (evt.type == EventType.MouseDrag)
            {
                evt.Use();
                _receiver.ApplyMaterialChange(evt);
            }
        }
    }
}

