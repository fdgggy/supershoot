﻿using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;

public class EntityAttack : Action
{
    private float now = 0.0f;
    private Entity entity = null;
    public override void OnAwake()
    {
        entity = GetComponent<Entity>();
    }

    public override void OnStart()
    {
        now = Time.time;
    }

    public override TaskStatus OnUpdate()
    {
        //if (Time.time > now + 2)
        //{
        //    now = Time.time;
        //    entity.Attack();

        //    return TaskStatus.Running;
        //}

        entity.Attack();
        return TaskStatus.Success;
    }
}
