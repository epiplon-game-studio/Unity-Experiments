using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(menuName = "vnclib/Behaviour Listing")]
public class BehaviourListing : ScriptableObject
{
    public List<StateBehaviour> stateBehaviours;

}
