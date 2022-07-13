using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonGenerator : MonoBehaviour
{
    [SerializeField] Transform[] _terrainObjects;
    // [SerializeField] float _diviationAngleThreshold = 30f;

    Color[] colors = new Color[] {
        Color.red,
        Color.blue,
        Color.green,
        Color.cyan,
        Color.magenta,
        Color.yellow,
        Color.black,
        Color.white
    };

    void OnDrawGizmos()
    {
        // Gizmos.color = Color.red;
        foreach (var terrain in _terrainObjects)
        {
            Mesh terrainMesh = terrain.GetComponent<MeshFilter>()?.sharedMesh;
            if(terrainMesh == null)
                continue;

            int[] triangles = terrainMesh.triangles;
            Vector3[] normals = terrainMesh.normals;
            Vector3[] vertices = terrainMesh.vertices;

            int triangleIndex = 0;
            while((triangleIndex * 3) < triangles.Length)
            {
                Debug.Log($"Indices: {triangles[triangleIndex * 3]}, {triangles[triangleIndex * 3 + 1]}, {triangles[triangleIndex * 3 + 2]}");
                ++triangleIndex;

                if((triangleIndex * 3 + 2) >= triangles.Length)
                    break;

                // Gizmos.color = colors[colorIndex];
                Vector3 vertex1 = vertices[triangles[triangleIndex * 3]];
                Vector3 vertex2 = vertices[triangles[triangleIndex * 3 + 1]];
                Vector3 vertex3 = vertices[triangles[triangleIndex * 3 + 2]];

                Gizmos.DrawLine(vertex1, vertex2);
                Gizmos.DrawLine(vertex1, vertex3);
                Gizmos.DrawLine(vertex2, vertex3);
            }

            // foreach (var normala in normals)
            // {
            //     var diviationAngle = Vector3.Angle(Vector3.up, normala);
            //     if (diviationAngle > _diviationAngleThreshold)
            //         continue;

            //     Gizmos.DrawRay(terrain.position, normala * 10);

            for (int i = 0; i < vertices.Length; ++i)
                {
                    var scaledVertex = Vector3.Scale(vertices[i], terrain.localScale);

                    if (scaledVertex.y <= 0)
                        continue;

                    var center = Vector3.Scale(vertices[i], terrain.localScale);
                    Gizmos.DrawSphere(center, .2f);
                }
            // }
        }
    }
}
