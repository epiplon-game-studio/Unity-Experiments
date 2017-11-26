using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using vnc.Dungeon;

namespace vnc.Editor
{
    [CustomEditor(typeof(BSPDungeon))]
    public class BSPDungeonEditor : UnityEditor.Editor
    {
        SerializedProperty boundary;
        SerializedProperty wall;
        SerializedProperty floor;

        SerializedProperty floorMaterial;
        SerializedProperty wallMaterial;

        private void OnEnable()
        {
            boundary = serializedObject.FindProperty("boundary_layer");
            wall = serializedObject.FindProperty("wall_layer");
            floor = serializedObject.FindProperty("floor_layer");

            floorMaterial = serializedObject.FindProperty("FloorMat");
            wallMaterial = serializedObject.FindProperty("WallMat");
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();

            EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
            EditorGUILayout.LabelField("Materials", EditorStyles.boldLabel);
            floorMaterial.objectReferenceValue = EditorGUILayout.ObjectField("Floor", floorMaterial.objectReferenceValue, typeof(Material), false);
            wallMaterial.objectReferenceValue = EditorGUILayout.ObjectField("Wall", wallMaterial.objectReferenceValue, typeof(Material), false);

            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();

            EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
            EditorGUILayout.LabelField("Layers", EditorStyles.boldLabel);
            boundary.intValue = EditorGUILayout.LayerField("Boundary", boundary.intValue);
            wall.intValue = EditorGUILayout.LayerField("Wall", wall.intValue);
            floor.intValue = EditorGUILayout.LayerField("Floor", floor.intValue);
            EditorGUILayout.EndVertical();
            serializedObject.ApplyModifiedProperties();
        }
    }
}

