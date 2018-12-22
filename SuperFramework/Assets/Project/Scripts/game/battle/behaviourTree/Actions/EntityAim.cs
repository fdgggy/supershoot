using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;

public class EntityAim : Action
{
    private SharedTransform target;
    private Entity entity = null;
    public override void OnAwake()
    {
        entity = GetComponent<Entity>();
    }

    public override void OnStart()
    {
        target = (Owner.GetVariable("aimTarget") as SharedTransform);
    }

    public override TaskStatus OnUpdate()
    {
        if (target.Value != null)
        {
            transform.LookAt(target.Value.position);
        }
        return TaskStatus.Success;
    }
}
