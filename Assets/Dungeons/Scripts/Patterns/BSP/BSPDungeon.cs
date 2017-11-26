using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using vnc.Utilities;

namespace vnc.Dungeon
{
    /// <summary>
    /// Dungeon built based on binary space tree
    /// </summary>
    [ExecuteInEditMode]
    public class BSPDungeon : Dungeon
    {
        BSPLeaf root; // Root node
        public Vector2 minimumSize; // Minimum size of a room

        #region Layers
        [SerializeField, HideInInspector] int boundary_layer;
        [SerializeField, HideInInspector] int wall_layer;
        [SerializeField, HideInInspector] int floor_layer;
        #endregion

        #region Materials
        [SerializeField, HideInInspector] Material FloorMat;
        [SerializeField, HideInInspector] Material WallMat;
        #endregion

        public bool DrawDebug;

        public override string Name
        {
            get
            {
                return "BSP";
            }
        }

        public BSPDungeon(int minWidth, int minHeight)
        {
            minimumSize = new Vector2(minWidth, minHeight);
            OnGenerateStart = new UnityEvent();
            OnGenerateFinish = new UnityEvent();
        }

        /// <summary>
        /// Start processing the room division
        /// </summary>
        /// <param name="width">Max room width</param>
        /// <param name="height">Max room height</param>
        public override void Generate(int width, int height)
        {
            if (minimumSize.Equals(Vector2.zero))
            {
                Debug.LogWarning("Ops! The minimum size cannot be (x: 0; y: 0).");
                return;
            }

            // Call event when it start
            OnGenerateStart.Invoke();
            root = new BSPLeaf(new Rect(0, 0, width, height));
            Divide(root);
            OnGenerateFinish.Invoke();
        }

        /// <summary>
        /// Divide the room in smaller rooms
        /// </summary>
        /// <param name="leaf"></param>
        private void Divide(BSPLeaf leaf)
        {
            Division div = Division.None;
            if (leaf.room.width > minimumSize.x)
                div = Division.Horizontal;
            else if (leaf.room.height > minimumSize.y)
                div = Division.Vertical;

            switch (div)
            {
                case Division.Horizontal:
                    // Left Leaf
                    // width takes between minimum width defined and 50% of room size 
                    var width = (int)Random.Range(minimumSize.x, (leaf.room.width / 2));

                    leaf.left = new BSPLeaf(leaf.room.position, new Vector2(width, leaf.room.height));
                    Divide(leaf.left);

                    // Right Leaf
                    var rightPositionHorizontal = new Vector2(leaf.room.position.x + width, leaf.room.position.y);
                    var rightSizeHorizontal = new Vector2(leaf.room.width - width, leaf.room.height);
                    leaf.right = new BSPLeaf(rightPositionHorizontal, rightSizeHorizontal);
                    Divide(leaf.right);

                    leaf.right.division = leaf.left.division = div;

                    break;
                case Division.Vertical:
                    leaf.division = div;

                    // Upper Leaf
                    // width takes between minimum width defined and 50% of room size 
                    var height = (int)Random.Range(minimumSize.y, (leaf.room.height / 2));

                    leaf.left = new BSPLeaf(leaf.room.position, new Vector2(leaf.room.width, height));
                    Divide(leaf.left);

                    // Bottom Leaf
                    var rightPositionVertical = new Vector2(leaf.room.position.x, leaf.room.position.y + height);
                    var rightSizeVertical = new Vector2(leaf.room.width, leaf.room.height - height);
                    leaf.right = new BSPLeaf(rightPositionVertical, rightSizeVertical);
                    Divide(leaf.right);

                    leaf.right.division = leaf.left.division = div;
                    break;
                default:
                    break;
            }
        }

        /// <summary>
        /// Create a visual representation of the dungeon
        /// </summary>
        public override void Draw()
        {
            if (Map != null)
                DestroyImmediate(Map);

            Map = new GameObject("Map");
            Map.isStatic = true;
            //DrawTiles(root);
            DrawRoom();
            DrawWalls(root);
            DrawCorridors(root);

            if (DrawDebug)
                DebugDraw();
        }

        // Fill each room with walls
        private void DrawWalls(BSPLeaf leaf)
        {
            if (leaf == null)
                return;

            if (leaf.left == null && leaf.right == null)
            {
                var wallBlock = GameObject.CreatePrimitive(PrimitiveType.Cube);
                wallBlock.name = "Wall";

                var rect = new Rect(Vector2.zero, Vector2.one);
                for (int x = 0; x < leaf.room.width; x++)
                {
                    for (int y = 0; y < leaf.room.height; y++)
                    {
                        rect.position = new Vector2(x + leaf.room.position.x, y + leaf.room.position.y);
                        if (!rect.Overlaps(leaf.walkable))
                        {
                            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
                            cube.name = "Wall";
                            var cubeRenderer = cube.GetComponent<MeshRenderer>();
                            cubeRenderer.material = WallMat;
                            cube.transform.position = rect.position;
                            cube.transform.parent = Map.transform;

                            if (rect.position.x == root.room.xMin
                                || rect.position.x == root.room.xMax - 1
                                || rect.position.y == root.room.yMin
                                || rect.position.y == root.room.yMax - 1)
                            {
                                cube.layer = boundary_layer;
                            }
                        }
                    }
                }

                DestroyImmediate(wallBlock);
            }

            DrawWalls(leaf.left);
            DrawWalls(leaf.right);
        }

        // Draw a floor
        public void DrawRoom()
        {
            var material = Instantiate(FloorMat);
            material.mainTextureScale = root.room.size;
            var map = CustomQuad.Create(root.room, material);
            map.name = "Room";
            map.transform.parent = Map.transform;
            map.isStatic = true;
            map.layer = floor_layer;
        }
        
        /// <summary>
        /// DEBUG
        /// </summary>
        /// <param name="leaf"></param>
        private void DrawDebugTiles(BSPLeaf leaf)
        {
            if (leaf == null)
                return;

            Debug.Log(leaf.walkable);
            if ((leaf.right == null && leaf.left == null))
            {
                var material = (Material)Instantiate(FloorMat);
                material.mainTextureScale = leaf.walkable.size;
                var walkableTile = CustomQuad.Create(leaf.walkable, material);
                walkableTile.name = "Walkable";
                walkableTile.transform.parent = Map.transform;
                walkableTile.isStatic = true;

                //var roomTile = CustomQuad.Create(leaf.room);
                //roomTile.name = "Room";
                //roomTile.transform.parent = Map.transform;
                //roomTile.transform.Translate(new Vector3(0, 0, 1));
            }

            DrawDebugTiles(leaf.left);
            DrawDebugTiles(leaf.right);
        }

        /// <summary>
        /// Draw corridors between rooms
        /// </summary>
        /// <param name="leaf">Leaf currently being processed</param>
        private void DrawCorridors(BSPLeaf leaf)
        {
            if (leaf.left == null || leaf.right == null)
                return;

            DrawPath(leaf.left, leaf.right);

            DrawCorridors(leaf.left);
            DrawCorridors(leaf.right);
        }

        /// <summary>
        /// Carve a path between rooms, removing walls except if they are in the boundaries of the map;
        /// </summary>
        /// <param name="start">Starting point</param>
        /// <param name="end">Ending point</param>
        private void DrawPath(BSPLeaf start, BSPLeaf end)
        {
            Vector3 origin = new Vector3(Mathf.Floor(start.walkable.center.x), Mathf.Floor(start.walkable.center.y));
            Vector3 direction = Vector3.zero;
            float distance = 0;
            switch (start.division)
            {
                case Division.Horizontal:
                    direction = Vector3.right;
                    distance = Mathf.Abs(end.walkable.center.x - origin.x);
                    break;
                case Division.Vertical:
                    direction = Vector3.up;
                    distance = Mathf.Abs(end.walkable.center.y - origin.y);
                    break;
                case Division.None:
                default:
                    return;
            }
            Ray ray = new Ray(origin, direction);
            Debug.Log(ray);
            foreach (var hit in Physics.RaycastAll(ray, distance, LayerMask.GetMask("Default")))
            {
                DestroyImmediate(hit.collider.gameObject);
            }
        }

        /// <summary>
        /// Draw a visual binary tree for debbuging
        /// </summary>
        private void DebugDraw()
        {
            DebugDrawLeaf(root, Vector2.zero, (new GameObject("Binary Tree")).transform);
        }

        private void DebugDrawLeaf(BSPLeaf leaf, Vector2 position, Transform parent)
        {
            if (leaf == null)
                return;

            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.localPosition = position;
            cube.transform.parent = parent;
            if (leaf.right == null && leaf.left == null)
            {
                cube.name = "Leaf";
            }

            DebugDrawLeaf(leaf.left, position + Vector2.left + Vector2.down, cube.transform);
            DebugDrawLeaf(leaf.right, position + Vector2.right + Vector2.down, cube.transform);
        }
    }

    public enum Division
    {
        None,
        Horizontal,
        Vertical
    }
}