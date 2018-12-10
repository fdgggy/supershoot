using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager
{
    private static EntityManager entityMgr = null;
    public static EntityManager Instance
    {
        get
        { 
            if (entityMgr == null)
            {
                return new EntityManager(); 
            }

            return entityMgr;
        }
    }

    private int entityId = 0;
    public int EntityId
    {
        get
        {
            return entityId++;
        }
    }

    private Dictionary<int, Entity> entityDic = new Dictionary<int, Entity>();

    public void CreateEntity(EntityInfo entityInfo, Vector3 position, Quaternion rotation, Util.CommonDelegate<Entity>callBack = null)
    {
        if(!string.IsNullOrEmpty(entityInfo.PrefabName))
        {
            //TODO 资源加载放到pool里面管理
            ResManager.Instance.LoadPrefab(entityInfo.PrefabName, (string asstName, object original) => 
            {
                GameObject go = null;
                if (vp_PoolManager.Instance == null || !vp_PoolManager.Instance.enabled || !(original is GameObject))
                {
                    go = GameObject.Instantiate((original as GameObject), position, rotation);
                }
                else
                {
                    go = vp_PoolManager.Spawn((original as GameObject), position, rotation);
                }

                if (callBack != null)
                {
                    if (go != null)
                    {
                        Entity entity = new Entity(entityInfo, go);
                        entityDic.Add(entityInfo.EntityId, entity);

                        callBack(entity);
                    }
                    else
                    {
                        Loger.Error("CreateEntity create entityId:{0} failed !", entityInfo.EntityId);
                        callBack(null);
                    }
                }
            });
        }
        else
        {
            Loger.Warn("CreateEntity failed, check the prefabName !");
        }
    }

    public bool IsEnemy(CampType camp1, CampType camp2)
    {
        Camp camp = ExcelDataManager.Instance.GetExcel(ExcelType.Camp) as Camp;
        CampData data = camp.QueryByID((int)camp1);
        if (data != null)
        { 
            if (camp2 == CampType.Enemy)
            {
                return data.Enemy == 1;
            }
            else if (camp2 == CampType.Player)
            {
                return data.Player == 1;
            }
            else if (camp2 == CampType.Neutral)
            {
                return data.Neutral == 1;
            }
        }

        return false;
    }

    public int GetCampCount(CampType campType)
    {
        int count = 0;
        foreach(KeyValuePair<int, Entity>kv in entityDic)
        {
            if (kv.Value.GetCamp == campType)
            {
                count++;
            }
        }

        return count;
    }
}
