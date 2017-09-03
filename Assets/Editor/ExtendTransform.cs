using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(Transform))]
public class ExtendTransform : Editor {

				public override void OnInspectorGUI()
				{
								base.OnInspectorGUI();
								if (GUILayout.Button("Show Assets"))
								{
												Vector2 mousePos = Event.current.mousePosition;
												EditorUtility.DisplayPopupMenu(new Rect(mousePos.x, mousePos.y, 0, 0), "Assets/", null);
								}
				}
}
