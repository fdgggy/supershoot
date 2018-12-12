using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EntityCount : Conditional
{
    public SharedCampType campType;
    public SharedInt min;
    public SharedInt max;

    public override TaskStatus OnUpdate()
    {
        int count = EntityManager.Instance.GetCampCount((CampType)campType.GetValue());

        if (count >= min.Value && count <= max.Value)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
