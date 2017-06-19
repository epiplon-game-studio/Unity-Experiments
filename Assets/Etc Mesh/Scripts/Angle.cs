using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class Angle : MonoBehaviour
{
    public Transform second;

    private void OnGUI()
    {
        if (second == null)
            return;
        
        var angle = Vector3.Angle(transform.position, second.position);

        GUILayout.Label("Angle: " + angle);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawCube(transform.position, Vector3.one);
        if (second == null)
            return;

        Gizmos.DrawCube(second.position, Vector3.one);
        Gizmos.DrawLine(transform.position, second.position);
    }


}
