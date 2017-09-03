using FluentBehaviourTree;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CameraControl : BehaviorExecutor
{
    Rigidbody body;

    [Param("Movement")]
    public Vector3 movement;

    public override void OnStart()
    {
        body = GetComponent<Rigidbody>();

        tree = builder
            .Sequence("default")
                .Do("input", t=> this.GetAxis("Movement"))
                .Do("move", t => this.Move(body, "Movement"))
                .End()
                .Build();
    }
}
