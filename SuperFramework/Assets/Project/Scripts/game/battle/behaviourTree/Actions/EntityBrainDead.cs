using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EntityBrainDead : Action
{
    public SharedCampType campType;

    public override void OnStart()
    {
        EntityManager.Instance.CutEntityBrain((CampType)campType.GetValue());
    }

    public override TaskStatus OnUpdate()
    {
        return TaskStatus.Success;
    }
}
