using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class UIExperiment : MonoBehaviour {

				public GUISkin skin;
				Rect windowRect;

				private void OnEnable()
				{
								windowRect = new Rect(10, 10, 100, 100);
				}

				private void OnGUI()
				{
								windowRect = GUI.Window(0, windowRect, CallWindow, "Test", skin.window);
				}

				void CallWindow(int id)
				{
								if (GUI.Button(new Rect(0, 0, 100, 20), "Hello World"))
												print("Got a click");
				}
}
