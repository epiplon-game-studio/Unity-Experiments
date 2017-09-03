using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarebonesCamera : MonoBehaviour
{

    Rigidbody body;

    // Use this for initialization
    void Start () {
        body = GetComponent<Rigidbody>();
    }
	
	// Update is called once per frame
	void Update () {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        Vector3 move = new Vector3(horizontal, vertical);
        body.MovePosition(body.position + move * 2);
    }
}
