using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

//寻找全局范围内的敌人，并没有区域限制
public class FindEnemy : Action
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
            Loger.Error("find enemy failed, target is null !");
            return TaskStatus.Failure;
        }

        Owner.SetVariableValue("aimTarget", target.GetTransform());

        return TaskStatus.Success;
    }
}
