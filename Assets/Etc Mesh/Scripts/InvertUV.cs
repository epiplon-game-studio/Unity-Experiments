using System.Linq;
using UnityEngine;

public class InvertUV : MonoBehaviour
{
    public Material tileMaterial;
    
    public void Generate()
    {
        var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        var filter = box.GetComponent<MeshFilter>();

        if (filter != null)
        {
            Mesh mesh = filter.mesh;

            Vector3[] normals = mesh.normals;
            for (int i = 0; i < normals.Length; i++)
                normals[i] = -normals[i];
            mesh.normals = normals;

            for (int m = 0; m < mesh.subMeshCount; m++)
            {
                int[] triangles = mesh.GetTriangles(m);
                for (int i = 0; i < triangles.Length; i += 3)
                {
                    int temp = triangles[i + 0];
                    triangles[i + 0] = triangles[i + 1];
                    triangles[i + 1] = temp;
                }
                mesh.SetTriangles(triangles, m);
            }
        }

        var renderer = box.GetComponent<MeshRenderer>();
        renderer.material = tileMaterial;
    }

    public void GenerateWithTriangles()
    {
        var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        box.name = "Room";
        box.transform.localScale = Vector3.one * 5;

        Mesh mesh = box.GetComponent<MeshFilter>().mesh;
        mesh.triangles = mesh.triangles.Reverse().ToArray();

        DestroyImmediate(GetComponent<Collider>());
        box.AddComponent<MeshCollider>();

        var renderer = box.GetComponent<MeshRenderer>();
        renderer.material = tileMaterial;
        renderer.material.mainTextureScale = Vector2.one * 2.5f;
    }
}
