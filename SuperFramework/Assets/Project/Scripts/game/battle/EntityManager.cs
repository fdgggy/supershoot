using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityManager : MonoBehaviour
{
    public static EntityManager Instance
    {
        get
        {
            return SingleComponent<EntityManager>.Instance();
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
                go = vp_Utility.Instantiate((original as UnityEngine.Object), position, rotation) as GameObject;

                if (callBack != null)
                {
                    if (go != null)
                    {
                        Entity entity = go.AddComponent<Entity>();
                        entity.EntityInfo = entityInfo;

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

    public void FreeEntity(EntityInfo entityInfo, GameObject entityObj)
    {
        if (entityDic.ContainsKey(entityInfo.EntityId))
        {
            entityDic.Remove(entityInfo.EntityId);

            vp_Utility.Destroy(entityObj);
        }
        else
        {
            Loger.Error("FreeEntity dont exist the entityId:{0}", entityInfo.EntityId);
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
