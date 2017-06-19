using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
public class MeshDeformer : MonoBehaviour
{
    public int deformingIterations;

    Mesh deformingMesh;
    MeshCollider meshCollider;
    Vector3[] originalVertices, displacedVertices;
    Vector3[] vertexVelocities;

    void Start()
    {
        meshCollider = GetComponent<MeshCollider>();
        deformingMesh = GetComponent<MeshFilter>().mesh;
        originalVertices = displacedVertices = deformingMesh.vertices;        
        vertexVelocities = new Vector3[originalVertices.Length];
    }

    public void AddDeformingForce(Vector3 point, float force)
    {
        for (int i = 0; i < displacedVertices.Length; i++)
        {
            AddForceToVertex(i, point, force);
        }
        StartCoroutine(DeformingIteration());
        meshCollider.sharedMesh = deformingMesh;
    }

    IEnumerator DeformingIteration()
    {
        for (int i = 0; i < deformingIterations; i++)
        {
            for (int vertex = 0; vertex < displacedVertices.Length; vertex++)
            {
                UpdateVertex(vertex);
            }
            deformingMesh.vertices = displacedVertices;
            deformingMesh.RecalculateNormals();
            yield return null;
        }
    }

    private void AddForceToVertex(int i, Vector3 point, float force)
    {
        Vector3 pointToVertex = displacedVertices[i] - point;
        float attenuatedForce = force / (1f + pointToVertex.sqrMagnitude);
        float velocity = attenuatedForce * Time.deltaTime;
        vertexVelocities[i] += pointToVertex.normalized * velocity;
    }

    private void UpdateVertex(int i)
    {
        Vector3 velocity = vertexVelocities[i];
        displacedVertices[i] += velocity * Time.deltaTime;
    }
}
