using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RecursiveLayer : MonoBehaviour
{
				public LayerMask somelayer;

				private void Start()
				{
								SetLayer(transform, gameObject.layer);
								
				}

				public void SetLayer(Transform trans, int layer)
				{
								trans.gameObject.layer = layer;
								foreach (Transform child in trans)
												SetLayer(child, layer);
				}
}
