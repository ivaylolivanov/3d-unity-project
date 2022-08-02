using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonGenerator : MonoBehaviour
{
    [SerializeField] Transform[] _terrainObjects;
    [SerializeField] float _diviationAngleThreshold = 30f;

    private Color[] colors = new Color[] {
        Color.red,
        Color.blue,
        Color.green,
        Color.cyan,
        Color.magenta,
        Color.yellow,
        Color.black,
        Color.white
    };

    private HashSet<Vector3> polygonPoints;

    void OnEnable()
    {
        polygonPoints = new HashSet<Vector3>();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
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
                if((triangleIndex * 3 + 2) >= triangles.Length)
                    break;

                Vector3 vertex1 = vertices[triangles[triangleIndex * 3]];
                Vector3 vertex2 = vertices[triangles[triangleIndex * 3 + 1]];
                Vector3 vertex3 = vertices[triangles[triangleIndex * 3 + 2]];

                Vector3 a = vertex2 - vertex1;
                Vector3 b = vertex3 - vertex1;

                Vector3 triangleNormal = new Vector3(
                    a.y * b.z - a.z * b.y,
                    a.z * b.x - a.x * b.z,
                    a.x * b.y - a.y * b.x
                );

                float diviationAngle = Vector3.Angle(Vector3.up, triangleNormal);
                if (diviationAngle > _diviationAngleThreshold)
                    continue;

                polygonPoints.Add(Vector3.Scale(vertex1, terrain.localScale));
                polygonPoints.Add(Vector3.Scale(vertex2, terrain.localScale));
                polygonPoints.Add(Vector3.Scale(vertex3, terrain.localScale));

                Gizmos.DrawRay(terrain.position, triangleNormal * 10);

                ++triangleIndex;
            }

            // for (int i = 0; i < vertices.Length; ++i)
            // {
            //     var scaledVertex = Vector3.Scale(vertices[i], terrain.localScale);

            //     if (scaledVertex.y <= 0)
            //         continue;

            //     var center = Vector3.Scale(vertices[i], terrain.localScale);
            //     Gizmos.DrawSphere(center, .2f);
            // }
        }

        Gizmos.color = Color.red;
        foreach(var point in polygonPoints)
        {
            Gizmos.DrawSphere(point, .2f);
        }
    }
}
