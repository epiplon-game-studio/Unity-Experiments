using FluentBehaviourTree;
using UnityEngine;

public static class CameraBehaviours
{
    public static BehaviourTreeStatus GetAxis(this BehaviorExecutor executor, string parameter)
    {
        var horizontal = Input.GetAxis("Horizontal");
        var vertical = Input.GetAxis("Vertical");
        executor.SetParameter(parameter, new Vector3(horizontal, vertical, 0));
        return BehaviourTreeStatus.Success;
    }
    public static BehaviourTreeStatus Move(this BehaviorExecutor executor, Rigidbody body, string parameter)
    {
        Vector3 move = executor.GetParameter<Vector3>(parameter);
        body.MovePosition(body.position + move * 2);
        return BehaviourTreeStatus.Success;
    }

}
