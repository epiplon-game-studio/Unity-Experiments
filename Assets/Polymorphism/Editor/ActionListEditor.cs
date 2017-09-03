using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ActionList))]
public class ActionListEditor : Editor {

				private List<Type> types = new List<Type>();
				private int index = 0;

				private void OnEnable()
				{
								Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies()
												.Where(a => a.FullName.StartsWith("Assembly-CSharp"))
												.ToArray();
								for (int i = 0; i < assemblies.Length; i++)
								{
												Type[] actionTypes = assemblies[i].GetTypes()
																.Where(t => (typeof(IAction).IsAssignableFrom(t)))
																.ToArray();
												for (int j = 0; j < actionTypes.Length; j++)
												{
																if (!actionTypes[j].IsInterface)
																{
																				types.Add(actionTypes[j]);
																}
												}
								}
								//Debug.Log("Is Move a subclass? " + typeof(Move).IsSubclassOf(typeof(IAction)));
				}

				public override void OnInspectorGUI()
				{
								var names = types.Select(t => t.ToString()).ToArray();
								index = EditorGUILayout.Popup("Actions", index, names);
				}

}
