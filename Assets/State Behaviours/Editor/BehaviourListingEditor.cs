using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(BehaviourListing))]
public class BehaviourListingEditor : Editor
{
    BehaviourListing behaviours;
    ReorderableList stateList;
    int scriptIndex;

    private void OnEnable()
    {
        behaviours = (BehaviourListing)target;
        if (behaviours.stateBehaviours == null)
            behaviours.stateBehaviours = new List<StateBehaviour>();

        stateList = new ReorderableList(behaviours.stateBehaviours, typeof(StateBehaviour),
            true, true, true, true);
    }

    public override void OnInspectorGUI()
    {
        var scripts = Resources.LoadAll<MonoScript>("");
        scriptIndex = EditorGUILayout.Popup(scriptIndex, scripts.Select(s => s.name).ToArray());

        serializedObject.Update();
        stateList.drawHeaderCallback = (Rect rect) =>
        {
            EditorGUI.LabelField(rect, "Behaviours");
        };
        stateList.onAddCallback = (ReorderableList list) =>
        {
            var script = scripts[scriptIndex];
            var type = script.GetClass();
            if (type.BaseType == typeof(StateBehaviour))
            {
                var state = System.Activator.CreateInstance(type);
                behaviours.stateBehaviours.Add(state as StateBehaviour);
                Debug.Assert((state as StateBehaviour) != null, "element reference is null");
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Not a State Behaviour", "Ok");
            }
        };

        stateList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) =>
        {
            var state = behaviours.stateBehaviours.ElementAt(index);
            rect.y += 2;
            EditorGUI.LabelField(new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight), state.ToString());
        };
        stateList.DoLayoutList();
        serializedObject.ApplyModifiedProperties();
    }
}
