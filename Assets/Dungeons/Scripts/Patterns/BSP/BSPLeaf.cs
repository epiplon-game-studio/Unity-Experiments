using UnityEngine;

namespace vnc.Dungeon
{
    public class BSPLeaf
    {
        public BSPLeaf right = null;
        public BSPLeaf left = null;
        public Rect room;
        public Rect walkable
        {
            get
            {
                Rect walkable = room;
                walkable.xMin += 1.0f;
                walkable.yMin += 1.0f;
                walkable.xMax -= 1.0f;
                walkable.yMax -= 1.0f;
                return walkable;
            }
        }

        public Division division;

        public BSPLeaf(int width, int height, int x = 0, int y = 0)
        {
            room = new Rect(x, y, width, height);
        }

        public BSPLeaf(Rect rect)
        {
            room = rect;
        }

        public BSPLeaf(Vector2 position, Vector2 size)
        {
            room = new Rect(position, size);
        }
    }
}
