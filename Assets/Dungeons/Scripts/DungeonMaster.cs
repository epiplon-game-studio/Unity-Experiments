using UnityEngine;

namespace vnc.Dungeon
{
    [ExecuteInEditMode]
    public sealed class DungeonMaster : MonoBehaviour
    {
        public Vector2 Size;

        public void Create(Dungeon dungeon)
        {
            dungeon.Generate((int)Size.x, (int)Size.y);
            dungeon.Draw();
        }
    }
}
