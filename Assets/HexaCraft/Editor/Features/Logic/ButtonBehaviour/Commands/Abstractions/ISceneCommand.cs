using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public interface ISceneCommand : ICommand
    {
        void Execute(SceneView sceneView, Event evt);
    }
}

