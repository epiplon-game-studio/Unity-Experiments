using UnityEngine;
using UnityEngine.AI;

public class MovePlayerInNavmesh : MonoBehaviour
{
				public float speed = 1;
				NavMeshAgent agent;

				void Start()
				{
								agent = GetComponent<NavMeshAgent>();
				}


				void Update()
				{
								var h = Input.GetAxis("Horizontal");
								var v = Input.GetAxis("Vertical");
								var movement = (Vector3.forward * v) + (Vector3.right * h);
								agent.Move(movement * Time.deltaTime);
				}
}
