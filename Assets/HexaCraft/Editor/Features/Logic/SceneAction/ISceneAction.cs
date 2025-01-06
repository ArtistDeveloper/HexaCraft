using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public interface ICommand
    {
        void Execute(SceneView sceneView, Event evt);
    }
}

