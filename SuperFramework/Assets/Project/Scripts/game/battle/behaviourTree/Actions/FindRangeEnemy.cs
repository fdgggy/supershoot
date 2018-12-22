using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections.Generic;
using UnityEngine;

//寻找视野范围内的敌人
public class FindRangeEnemy : Action
{
    private Entity entity;
    public override void OnStart()
    {
        entity = this.GetComponent<Entity>();
    }

    public override TaskStatus OnUpdate()
    {
        Entity target = EntityManager.Instance.GetNearestEnemy(entity);
        if (target == null)
        {
            Loger.Error("FindRangeEnemy targets is null");
            return TaskStatus.Failure;
        }

        Transform tar = BehaviorDesigner.Manager.Movement.MovementUtility.WithinSight(transform, Vector3.zero, 90, 5, target.GetTransform());
        if (tar == null)
        {
            return TaskStatus.Failure;
        }
        else
        {
            Owner.SetVariableValue("aimTarget", tar);
        }

        return TaskStatus.Success;
    }

    public override void OnDrawGizmos()
    {
        BehaviorDesigner.Manager.Movement.MovementUtility.DrawLineOfSight(Owner.transform, Vector3.zero, 90, 5, false);
    }
}
