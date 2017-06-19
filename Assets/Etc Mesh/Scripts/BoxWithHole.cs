using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxWithHole : MonoBehaviour
{
    MeshFilter meshFilter;

    void Start()
    {
        var box = GameObject.CreatePrimitive(PrimitiveType.Cube);
        meshFilter = box.GetComponent<MeshFilter>();
        Debug.Log("Vertices: " + meshFilter.mesh.vertices.Length);
    }

    private void OnDrawGizmos()
    {
        if (meshFilter != null)
        {
            Gizmos.color = Color.gray;
            foreach (var v in meshFilter.mesh.vertices)
            {
                Gizmos.DrawSphere(v, 0.1f);
            }
        }
    }
}
