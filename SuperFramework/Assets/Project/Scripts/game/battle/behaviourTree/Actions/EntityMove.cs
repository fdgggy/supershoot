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
        //SharedTransform m_target = (Owner.GetVariable("aimTarget") as SharedTransform);

        //entity.MoveTo(m_target.Value.position);
        entity.RunTo(new Vector3(0f, -19f, 18f));
    }

    public override TaskStatus OnUpdate()
    {
        if (entity.RunOver())
        {
            return TaskStatus.Success;
        }

        
        return TaskStatus.Running;
    }
}
