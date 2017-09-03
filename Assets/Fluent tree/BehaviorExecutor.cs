using FluentBehaviourTree;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public abstract class BehaviorExecutor : MonoBehaviour
{
    public BehaviourTreeBuilder builder { get; private set; }
    public IBehaviourTreeNode tree;
    public AnimatorUpdateMode update;

    List<ParamAttribute> attributes;

    private void Start()
    {
        builder = new BehaviourTreeBuilder();
        CreateAttributes();
        OnStart();
    }

    private void Update()
    {
        tree.Tick(new TimeData(Time.deltaTime));
    }

    public abstract void OnStart();

    void CreateAttributes()
    {
        var fields = GetType().GetFields();
        attributes = new List<ParamAttribute>();
        foreach (var f in fields)
        {
            var attr = System.Attribute.GetCustomAttribute(f, typeof(ParamAttribute)) as ParamAttribute;
            if(attr != null)
            {
                attr.value = f.GetValue(this);
                attributes.Add(attr);
            }
        }
    }

    public void SetParameter(string name, object value)
    {
        if (attributes != null)
        {
            var param = attributes.SingleOrDefault(p => p.name.Equals(name));
            if (param == null)
            {
                Debug.LogError("Parameter " + name + " was not found.");
                return;
            }

            param.value = value;
        }
    }
    public T GetParameter<T>(string name)
    {
        if (attributes != null)
        {
            var param = attributes.SingleOrDefault(p => p.name.Equals(name));
            if (param == null)
            {
                Debug.LogError("Parameter " + name + " was not found.");
                return default(T);
            }
            return (T)param.value;

        }
        return default(T);
    }

}
