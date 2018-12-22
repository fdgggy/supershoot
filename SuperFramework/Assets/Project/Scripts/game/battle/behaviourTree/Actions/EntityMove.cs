using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;

public class EntityMove : Action
{
    private Entity entity = null;
    public override void OnAwake()
    {
        entity = GetComponent<Entity>();
    }

    public override void OnStart()
    {
        SharedTransform m_target = (Owner.GetVariable("aimTarget") as SharedTransform);

        entity.MoveTo(m_target.Value.position);
    }

    public override TaskStatus OnUpdate()
    {
        if (entity.MoveOver())
        {
            return TaskStatus.Success;
        }

        
        return TaskStatus.Running;
    }
}
