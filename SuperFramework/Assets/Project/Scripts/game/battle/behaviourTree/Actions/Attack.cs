using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class Attack : Action
{

    public override void OnAwake()
    {
    }

    public override void OnStart()
    {

    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Running;
    }
}
