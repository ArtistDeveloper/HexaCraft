using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public interface ISceneGUIHandler
    {
        public void OnSceneGUI(SceneView sceneView);
    }
}