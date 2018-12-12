using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class MoveEntity : Action
{
    private NavMeshAgent moveAgent;

    public override void OnAwake()
    {
        moveAgent = GetComponent<NavMeshAgent>();
    }

    public override void OnStart()
    {
        
    }

    public override TaskStatus OnUpdate()
    {
        moveAgent.SetDestination(new Vector3(5, -18, 10));
        return TaskStatus.Running;
    }
}
