﻿using UnityEngine;
using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using System.Collections;

public class EntityBorn : Action
{
    private bool complete = false;
    public SharedInt enemyId;
    public SharedInt bornPoint;
    public override void OnAwake()
    {
        base.OnAwake();
    }

    public override void OnStart()
    {
        base.OnStart();
        complete = false;

        Enemy enemyExcel = ExcelDataManager.Instance.GetExcel(ExcelType.Enemy) as Enemy;
        EnemyData enemyData = enemyExcel.QueryByID(enemyId.Value);
        if (enemyData == null)
        {
            Loger.Error("EntityBorn not found the enemyId:{0}", enemyId.Value);
            return;
        }

        EntityInfo entityInfo = new EntityInfo()
        {
            PrefabName = enemyData.Prefabname,
            EntityId = EntityManager.Instance.EntityId,
            Camp = (CampType)enemyData.Camp,
            MoveSpeed = enemyData.Movespeed,
            RunSpeed = enemyData.Runspeed,
            FieldOfView = enemyData.Fieldofviewangle,
            FieldDistance = enemyData.Viewdistance,
        };

        Vector3 originalPos = new Vector3(0, -18, 0);
        if (bornPoint != null)
        {
            string path = string.Format("bornPoint/spawnPoint_{0}", bornPoint.Value);
            GameObject point = GameObject.Find(path);
            if (point == null)
            {
                Loger.Error("EntityBorn dont find the point !");
            }
            else
            {
                originalPos = point.transform.position;
            }
        }
        EntityManager.Instance.CreateEntity(entityInfo, originalPos, Quaternion.Euler(0, 0, 0), (Entity go) =>
        {
            go.Active(true);
            complete = true;
        });
    }   

    public override TaskStatus OnUpdate()
    {
        if (complete)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
