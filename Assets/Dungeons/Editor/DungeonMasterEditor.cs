using System;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace vnc.Dungeon.Editor
{
    [CustomEditor(typeof(DungeonMaster))]
    public class DungeonMasterEditor : UnityEditor.Editor
    {
        DungeonMaster dungeonMaster;
        Dungeon[] dungeons;
        int index;

        private void OnEnable()
        {
            dungeonMaster = (DungeonMaster)target;
            dungeons = dungeonMaster.GetComponents<Dungeon>();
            index = 0;
        }

        public override void OnInspectorGUI()
        {
            ShowDungeonListPopup();
            ShowSize();
            GenerateDungeon();
            serializedObject.ApplyModifiedProperties();
        }

        private void ShowSize()
        {
            EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
            EditorGUILayout.LabelField("Dungeon Size", EditorStyles.boldLabel);
            dungeonMaster.Size = EditorGUILayout.Vector2Field("", dungeonMaster.Size);
            EditorGUILayout.EndVertical();
        }

        private void ShowDungeonListPopup()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
            EditorGUILayout.LabelField("Dungeon", EditorStyles.boldLabel);
            var rect = EditorGUILayout.GetControlRect(true, EditorGUIUtility.singleLineHeight);
            if (dungeons.Length > 0)
            {
                index = EditorGUI.Popup(rect, index, dungeons.Select(d => d.Name).ToArray());
            }

            EditorGUILayout.EndVertical();
        }

        private void GenerateDungeon()
        {
            EditorGUILayout.Space();
            EditorGUILayout.BeginVertical(EditorStyles.inspectorDefaultMargins);
            if (GUILayout.Button("Generate"))
            {
                dungeonMaster.Create(dungeons[index]);
            }
            EditorGUILayout.EndVertical();
        }

    }
}
