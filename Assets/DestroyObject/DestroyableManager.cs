using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyableManager : MonoBehaviour {

	Destroyable destroyable = null;

	void Start () {
		destroyable = GetComponentInChildren<Destroyable>();
	}

	private void OnGUI()
	{
		GUI.Box(new Rect(0, 0, 200, 50), "Destroyable: " + destroyable);
	}
}
