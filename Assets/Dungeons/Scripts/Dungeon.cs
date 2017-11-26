using UnityEngine;
using UnityEngine.Events;

namespace vnc.Dungeon
{
    [ExecuteInEditMode]
    [RequireComponent(typeof(DungeonMaster))]
    public abstract class Dungeon : MonoBehaviour
    {
        public abstract string Name { get; }

        public UnityEvent OnGenerateStart;
        public UnityEvent OnGenerateFinish;

        [HideInInspector]
        public GameObject Map;

        public abstract void Generate(int width, int height);
        public abstract void Draw();
    }
}
