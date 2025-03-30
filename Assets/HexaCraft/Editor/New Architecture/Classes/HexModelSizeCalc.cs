using UnityEngine;

namespace HexaCraft
{
    public static class HexModelSizeCalc
    {
        public static float CalcHexModelSize(GameObject hexPrefab)
        {
            if (hexPrefab == null)
            {
                Debug.LogError($"{nameof(HexModelSizeCalc)}: Hex prefab is null");
                return 0f;
            }

            Renderer[] renderers = hexPrefab.GetComponentsInChildren<Renderer>();
            if (renderers.Length == 0)
                return 0f;

            Bounds bounds = renderers[0].bounds;
            for (int i = 1; i < renderers.Length; i++)
            {
                bounds.Encapsulate(renderers[i].bounds);
            }

            // 외접원 반지름 계산 (x,z 평면 기준) == 중심으로부터 절반 크기
            float radius = Mathf.Max(bounds.extents.x, bounds.extents.z);
            return radius;
        }

        public static float CalcExactHexRadius(GameObject hexPrefab)
        {
            MeshFilter meshFilter = hexPrefab.GetComponent<MeshFilter>();
            if (meshFilter == null || meshFilter.sharedMesh == null)
                return 0f;

            Vector3[] vertices = meshFilter.sharedMesh.vertices;
            Vector3 center = Vector3.zero; // 메시의 피벗 포인트가 중심이라 가정
            float maxRadius = 0f;

            foreach (Vector3 vertex in vertices)
            {
                // xz 평면에서의 거리만 계산
                float distance = new Vector2(vertex.x, vertex.z).magnitude;
                maxRadius = Mathf.Max(maxRadius, distance);
            }

            return maxRadius;
        }
    }
}
