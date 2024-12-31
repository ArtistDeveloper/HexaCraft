using UnityEditor;
using UnityEngine;

namespace HexaCraft
{
    public static class Utils
    {
        private static void DrawRaycastLine(Vector3 point)
        {
            Camera _camera = Camera.current;
            if (!_camera && SceneView.lastActiveSceneView != null)
            {
                _camera = SceneView.lastActiveSceneView.camera;
            }
            // offset을 추가하지 않으면 그려진 Line이 3D화면에선 보이지 않음
            Vector3 offsetPosition = _camera.transform.position + _camera.transform.forward * 0.3f;
            Handles.DrawAAPolyLine(10f, offsetPosition, point);
        }

        private static void SetLineProperty()
        {
            var color = new Color(0.43f, 0.76f, 0.96f, 1);
            Handles.color = color;
        }
    }
}
