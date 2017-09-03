using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ProjectileDefinition
{
				public GameObject prefab;
				public int amount;
				[HideInInspector, System.NonSerialized] public Stack<GameObject> stack;

}

[System.Serializable]
public class ProjectileDictionary : SerializableDictionary<string, ProjectileDefinition> { }
