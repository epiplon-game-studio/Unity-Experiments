using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(ProjectileDictionary))]
public class ProjectileDictionaryDrawer : PropertyDrawer
{
				ProjectileDictionary _dictionary;

				//SerializedProperty keys;
				//SerializedProperty values;

				float GROUP_HEIGHT { get { return (EditorGUIUtility.singleLineHeight * 4) + 2 * SPACING_HORIZONTAL; } }
				const float CONTROLS_HEIGHT = 50;
				const float SPACING_RIGHT = 5f;
				const float SPACING_HORIZONTAL = 5f;

				public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
				{
								CheckDictionary(property);
								//keys = property.FindPropertyRelative("_Keys");

								if (_dictionary.Keys != null)
								{
												return (GROUP_HEIGHT * _dictionary.Keys.Count) + CONTROLS_HEIGHT;
								}
								return CONTROLS_HEIGHT;
				}

				public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
				{
								CheckDictionary(property);

								//keys = property.FindPropertyRelative("_Keys");
								//values = property.FindPropertyRelative("_Values");

								EditorGUI.BeginProperty(position, label, property);

								var groupRect = new Rect(position.x, position.y, position.width, GROUP_HEIGHT);
								if (_dictionary != null)
								{
												for (int i = 0; i < _dictionary.Count; i++)
												{
																var key = _dictionary.Keys.ElementAt(i);
																var value = _dictionary.Values.ElementAt(i);

																var keyRect = new Rect(groupRect.x, groupRect.y + SPACING_HORIZONTAL, groupRect.width - SPACING_RIGHT, EditorGUIUtility.singleLineHeight);
																var amountRect = new Rect(groupRect.x, groupRect.y + SPACING_HORIZONTAL + EditorGUIUtility.singleLineHeight, groupRect.width - SPACING_RIGHT, EditorGUIUtility.singleLineHeight);
																var prefabRect = new Rect(groupRect.x, groupRect.y + SPACING_HORIZONTAL + (2 * EditorGUIUtility.singleLineHeight), groupRect.width - SPACING_RIGHT, EditorGUIUtility.singleLineHeight);

																GUI.Box(groupRect, "", EditorStyles.helpBox);
																EditorGUI.LabelField(keyRect, key);
																value.amount = EditorGUI.IntField(amountRect, "Amount", value.amount);
																value.prefab = (GameObject) EditorGUI.ObjectField(prefabRect, "Prefab", value.prefab, typeof(GameObject), false);

																//EditorGUI.PropertyField(,
																//				amount, new GUIContent("Amount"));
																//EditorGUI.PropertyField(,
																//				prefab, new GUIContent("Prefab"));
																if (GUI.Button(new Rect(groupRect.x + 10f, groupRect.y + SPACING_HORIZONTAL + (3 * EditorGUIUtility.singleLineHeight), groupRect.width - SPACING_RIGHT - 20f, EditorGUIUtility.singleLineHeight),
																				"Remove"))
																{
																				_dictionary.Remove(key);
																				GUI.changed = true;
																}
																groupRect.y += GROUP_HEIGHT;
												}

												//for (int i = 0; i < keys.arraySize; i++)
												//{
												//				var key = keys.GetArrayElementAtIndex(i);
												//				var projectile = values.GetArrayElementAtIndex(i);

												//				var amount = projectile.FindPropertyRelative("amount");
												//				var prefab = projectile.FindPropertyRelative("amount");

												//				GUI.Box(groupRect, "", EditorStyles.helpBox);
												//				EditorGUI.PropertyField(new Rect(groupRect.x, groupRect.y + SPACING_HORIZONTAL, groupRect.width - SPACING_RIGHT, EditorGUIUtility.singleLineHeight),
												//								key, new GUIContent("Key"));
												//				EditorGUI.PropertyField(new Rect(groupRect.x, groupRect.y + SPACING_HORIZONTAL + EditorGUIUtility.singleLineHeight, groupRect.width - SPACING_RIGHT, EditorGUIUtility.singleLineHeight),
												//								amount, new GUIContent("Amount"));
												//				EditorGUI.PropertyField(new Rect(groupRect.x, groupRect.y + SPACING_HORIZONTAL + (2 * EditorGUIUtility.singleLineHeight), groupRect.width - SPACING_RIGHT, EditorGUIUtility.singleLineHeight),
												//								prefab, new GUIContent("Prefab"));
												//				if (GUI.Button(new Rect(groupRect.x + 10f, groupRect.y + SPACING_HORIZONTAL + (3 * EditorGUIUtility.singleLineHeight), groupRect.width - SPACING_RIGHT - 20f, EditorGUIUtility.singleLineHeight),
												//								"Remove"))
												//				{
												//								_dictionary.Remove(key.stringValue);
												//								GUI.changed = true;
												//				}
												//				groupRect.y += GROUP_HEIGHT;
												//}
								}

								var addRect = new Rect(position.x, groupRect.y + 20f, 100f, EditorGUIUtility.singleLineHeight);
								//var removeRect = new Rect(position.x + 100f, groupRect.y + 20f, 100f, EditorGUIUtility.singleLineHeight);
								if (GUI.Button(addRect, "Add"))
								{
												KeyInputWindow.Display(ref _dictionary);
												GUI.changed = true;
								}

								EditorGUI.EndProperty();
				}

				void CheckDictionary(SerializedProperty property)
				{
								var target = property.serializedObject.targetObject;
								_dictionary = fieldInfo.GetValue(target) as ProjectileDictionary;

				}
}

public class KeyInputWindow : EditorWindow
{
				static KeyInputWindow window = null;

				ProjectileDictionary dictionary;
				string input;

				public static void Display(ref ProjectileDictionary dictionary)
				{
								if (window != null)
								{
												window.Close();
								}
								window = CreateInstance<KeyInputWindow>();
								window.position = new Rect(Screen.width / 2, Screen.height / 2, 250, 50);
								window.dictionary = dictionary;
								window.ShowUtility();
				}

				private void OnGUI()
				{
								input = EditorGUILayout.TextField("Enter key: ", input);
								if (GUILayout.Button("Enter"))
								{
												if (string.IsNullOrEmpty(input))
												{
																EditorUtility.DisplayDialog("Error", "Key cannot be empty", "OK");
																return;
												}
												dictionary.Add(input, new ProjectileDefinition());
												GUI.changed = true;
												this.Close();
								}
				}


}
