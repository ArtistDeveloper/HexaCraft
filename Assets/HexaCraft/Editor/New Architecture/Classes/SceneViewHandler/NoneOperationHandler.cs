using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public class NoneOperationHandler : ISceneGUIHandler
    {
        public void OnSceneGUI(SceneView sceneView)
        {
            // No Operation. Null Object
            return;
        }
    }

}