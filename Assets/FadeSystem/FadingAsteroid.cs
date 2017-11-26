using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadingAsteroid : MonoBehaviour {

	public Camera m_camera;
	MeshRenderer m_Renderer;
	float distance;

	private void Start()
	{
		m_Renderer = GetComponent<MeshRenderer>();
	}

	private void Update()
	{
		distance = Vector3.Distance(transform.position, m_camera.transform.position)/ 10 ;
		m_Renderer.material.SetFloat("_Distance", Mathf.Clamp(distance, 0, 1));
	}

	private void OnGUI()
	{
		GUI.Label(new Rect(0, 0, 200, 50), distance.ToString());
	}
}
