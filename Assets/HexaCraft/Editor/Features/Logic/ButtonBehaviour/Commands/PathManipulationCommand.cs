using HexaCraft;
using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class PathManipulationCommand : ISceneCommand
    {
        private PathManipulation _receiver;

        public PathManipulationCommand(PathManipulation receiver)
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
                _receiver.Execute(evt);
            }
        }
    }
}
