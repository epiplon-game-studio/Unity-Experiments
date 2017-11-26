using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.VersionControl;
using UnityEngine;

public class SerializeID : MonoBehaviour
{
				public GameObject CerealObj;
				public int instanceID;

				void Start()
				{
								instanceID = CerealObj.GetInstanceID();
								var obj = Resources.Load(instanceID.ToString());
								if (obj == null)
								{
												Debug.Log("Does not work");
								}
								else
								{
												Debug.Log("here I CUM");
												Instantiate(obj);
								}
				}
}
