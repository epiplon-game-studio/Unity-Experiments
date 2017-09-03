using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DirectionsTest : MonoBehaviour
{
				public Vector2 enemyPos = Vector2.zero;
				public Vector2 playerPos = Vector2.one;

				public Vector2 mainDirection;
				public Vector2 rightAdjancent;
				public Vector2 leftAdjancent;

				private void Update()
				{
								var horizontal = Input.GetAxis("Horizontal") * Time.deltaTime;
								var vertical = Input.GetAxis("Vertical") * Time.deltaTime;
								playerPos = new Vector2(playerPos.x + horizontal, playerPos.y + vertical);

								mainDirection = (playerPos - enemyPos).normalized;

								rightAdjancent = Quaternion.Euler(0, 0, 45) * mainDirection;
								leftAdjancent = Quaternion.Euler(0, 0, -45) * mainDirection;
				}

				private void OnDrawGizmos()
				{
								Gizmos.color = Color.red;
								Gizmos.DrawSphere(enemyPos, 0.4f);

								Gizmos.color = Color.green;
								Gizmos.DrawSphere(playerPos, 0.4f);
								Gizmos.color = Color.white;
								Gizmos.DrawLine(enemyPos, playerPos);


								Gizmos.color = Color.yellow;
								Gizmos.DrawLine(Vector2.zero, mainDirection);
								Gizmos.DrawLine(Vector2.zero, rightAdjancent);
								Gizmos.DrawLine(Vector2.zero, leftAdjancent);
				}

				private void OnGUI()
				{
								var text = "Main Direction: " + mainDirection + "\n";
								text += "Magnitude : " + mainDirection.magnitude + "\n";
								text += "Adjacent : " + rightAdjancent + "\n";
								GUI.Box(new Rect(0, 0, 200, 400), text);

				}
}
