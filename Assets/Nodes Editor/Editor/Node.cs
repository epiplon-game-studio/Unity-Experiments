using System;
using UnityEditor;
using UnityEngine;

namespace vnc.Editor.Experimental
{
    public class Node
    {
        public Rect rect;
        public string title;
        public bool isDragged;
        public bool isSelected;

        public ConnectionPoint inPoint;
        public ConnectionPoint outPoint;

        public GUIStyle style;
        public GUIStyle defaultNodeStyle;
        public GUIStyle selectedNodeStyle;

        public Action<Node> OnRemoveNode;
        public Action<ConnectionPoint> OnClickInPoint;
        public Action<ConnectionPoint> OnClickOutPoint;
        public Action OnClearConnectionSelection;

        public Node(Vector2 position, float width, float height, 
            GUIStyle nodeStyle, GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle, 
            Action<ConnectionPoint> OnClickInPoint, 
            Action<ConnectionPoint> OnClickOutPoint, 
            Action<Node> OnClickRemoveNode,
            Action OnClearConnectionSelection)
        {
            rect = new Rect(position.x, position.y, width, height);
            style = nodeStyle;
            inPoint = new ConnectionPoint(this, ConnectionPointType.In, inPointStyle, OnClickInPoint);
            outPoint = new ConnectionPoint(this, ConnectionPointType.Out, outPointStyle, OnClickOutPoint);
            this.OnClickOutPoint = OnClickOutPoint;
            this.OnClickInPoint = OnClickInPoint;
            this.OnRemoveNode = OnClickRemoveNode;
            this.OnClearConnectionSelection = OnClearConnectionSelection;

            defaultNodeStyle = nodeStyle;
            selectedNodeStyle = selectedStyle;
        }

        public void Drag(Vector2 delta)
        {
            rect.position += delta;
        }

        public void Draw()
        {
            inPoint.Draw();
            outPoint.Draw();
            GUI.Box(rect, title, style);
        }

        public bool ProcessEvents(Event e)
        {
            switch (e.type)
            {
                case EventType.MouseDown:
                    if (e.button == 0)
                    {
                        if (rect.Contains(e.mousePosition))
                        {
                            isDragged = true;
                            isSelected = true;
                            style = selectedNodeStyle;
                        }
                        else
                        {
                            isSelected = false;
                            style = defaultNodeStyle;
                            OnClearConnectionSelection();
                        }
                        GUI.changed = true;
                    }
                    if (e.button == 1 && isSelected && rect.Contains(e.mousePosition))
                    {
                        ProcessContextMenu();
                        e.Use();
                    }
                    break;

                case EventType.MouseUp:
                    isDragged = false;
                    break;

                case EventType.MouseDrag:
                    if (e.button == 0 && isDragged)
                    {
                        Drag(e.delta);
                        e.Use();
                        return true;
                    }
                    break;
            }
            return false;
        }

        private void ProcessContextMenu()
        {
            GenericMenu genericMenu = new GenericMenu();
            genericMenu.AddItem(new GUIContent("Connect"), false, OnClickConnect);
            genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
            genericMenu.ShowAsContext();
        }

        private void OnClickConnect()
        {
            OnClickOutPoint(outPoint);
        }

        private void OnClickRemoveNode()
        {
            if (OnRemoveNode != null)
            {
                OnRemoveNode(this);
            }
        }
    }
}
