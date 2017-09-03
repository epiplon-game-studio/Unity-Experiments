using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputBurst : MonoBehaviour
{
				ParticleSystem particles;

				void Start()
				{
								particles = GetComponent<ParticleSystem>();
				}
				
				void Update()
				{
								if (Input.GetButton("Fire1"))
								{
												particles.Emit(1);
								}
				}
}
