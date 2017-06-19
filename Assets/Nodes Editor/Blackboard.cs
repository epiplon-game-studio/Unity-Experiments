using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace vnc.Editor.Experimental
{
    [CreateAssetMenu(menuName = "vnc Library/Experimental/Blackboard")]
    public class Blackboard : ScriptableObject
    {
        public string Title;
        public List<Parameter> Parameters;
        public AnimatorUpdateMode Update;

        public Blackboard()
        {
            Parameters = new List<Parameter>();
        }

        public void SetValue(string name, object value, ParameterType type)
        {
            if(Parameters != null)
            {
                var param = Parameters.FirstOrDefault(p => p.Name.Equals(name));
                if(param.Type != type)
                {
                    Debug.Log("Blackboard: Invalid (" + type.ToString() + ") for parameter " + name);
                    return;
                }
            }
        }
    }
}
