using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace vnc.Editor.Experimental
{
    [System.Serializable]
    public class Parameter
    {
        public string Name;
        public ParameterType Type;
        public object Value;
    }

    public enum ParameterType
    {
        Float,
        Int,
        Bool,
        String,
        Vector2,
        Vector3,
        GameObject
    }
}
