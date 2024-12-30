using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public interface ISceneAction
    {
        void Execute(SceneView sceneView, Event evt);
    }
}

