using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;

public class BornEntity : Action
{
    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override void OnStart()
    {
        base.OnStart();
    }   

    public override TaskStatus OnUpdate()
    {
        EntityInfo entityInfo = new EntityInfo()
        {
            PrefabName = "HeroHDWeapons",
            EntityId = EntityManager.Instance.EntityId,
            campType = CampType.Player,
        };

        EntityManager.Instance.CreateEntity(entityInfo, new Vector3(0, -18, 0), Quaternion.Euler(0, 180, 0), (Entity go) =>
        {
            go.Active(true);


        });

        return TaskStatus.Success;
    }
}
