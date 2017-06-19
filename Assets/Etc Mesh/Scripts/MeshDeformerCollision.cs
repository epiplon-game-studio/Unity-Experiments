using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshDeformer))]
public class MeshDeformerCollision : MonoBehaviour
{
    public float force = 10f;
    public float forceOffset = 0.1f;
    bool isBroken = false;

    MeshDeformer deformer;

	void Start ()
    {
        deformer = GetComponent<MeshDeformer>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (!isBroken)
        {
            foreach (var contact in collision.contacts)
            {
                var point = contact.point;
                point += contact.normal * forceOffset;
                deformer.AddDeformingForce(contact.point, force);
            }
            isBroken = true;   
        }
    }
}
