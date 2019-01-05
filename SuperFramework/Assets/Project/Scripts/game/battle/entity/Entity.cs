using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Entity : MonoBehaviour
{
#if UNITY_EDITOR
    public CampType editorCamp;
    public int editorEntityID;
#endif
    public EntityInfo DataInfo { get; private set; }
    public CampType GetCamp
    {
        get
        {
            return DataInfo.Camp;
        }
    }

    protected EntityAnimator entityAnimator;

    private NavMeshAgent aiAgent = null;
    private int IsMoving;
    private WeaponHandler weaponHandle;

    public void Active(bool show = true)
    {
        gameObject.SetActive(show);
    }

    public void Init(EntityInfo entityInfo)
    {
        DataInfo = new EntityInfo();
        DataInfo = entityInfo;

        weaponHandle = new WeaponHandler();
        weaponHandle.Init(DataInfo.WeaponIds);
    }

    private void Awake()
    {
        aiAgent = GetComponent<NavMeshAgent>();
        entityAnimator = GetComponentInChildren<EntityAnimator>();

#if UNITY_EDITOR
        ResManager.Instance.Init(() =>
        {
            ExcelDataManager.Instance.Init(() =>
            {
                EntityInfo info = new EntityInfo();
                if (editorCamp == CampType.Player)
                {
                    Role role = ExcelDataManager.Instance.GetExcel(ExcelType.Role) as Role;
                    RoleData roleData = role.QueryByID(editorEntityID);

                    info.PrefabName = roleData.Prefabname;
                    info.EntityId = EntityManager.Instance.EntityId;
                    info.Camp = CampType.Player;
                    info.WeaponIds = roleData.Weaponids;
                }
                else if (editorCamp == CampType.Enemy)
                {

                }
                else
                {
                    Loger.Error("Entity Editor Model Set Error !");
                    return;
                }

                Init(info);
            });
        });
#endif
    }

    private void Update()
    {
        if (weaponHandle != null)
        {
            weaponHandle.Update();
        }
    }
    public Transform GetTransform()
    {
        return this.transform;
    }
    #region AI

    public float Distance(Entity target)
    {
        return Vector3.SqrMagnitude(this.transform.position - target.GetTransform().position);
    }
    /// <summary>
    /// 提供给AI移动
    /// </summary>
    public void MoveTo(Vector3 target)
    {
        aiAgent.speed = DataInfo.MoveSpeed;
        aiAgent.SetDestination(target);
        entityAnimator.MovingStart();
    }

    public bool MoveOver()
    {
        if (aiAgent.enabled == true 
            && aiAgent.pathStatus == NavMeshPathStatus.PathComplete 
            && aiAgent.remainingDistance <= 0.5f && !aiAgent.pathPending)
        {
            entityAnimator.MovingOver();
            return true;
        }

        return false;
    }

    public void RunTo(Vector3 target)
    {
        //Debug.Log("DataInfo.RunSpeed:"+ DataInfo.RunSpeed);
        //Debug.Log("target:"+ target);

        aiAgent.speed = DataInfo.RunSpeed;
        aiAgent.SetDestination(target);
        entityAnimator.MovingStart();
        entityAnimator.RunStart();
    }

    public bool RunOver()
    {
        if (aiAgent.enabled == true
            && aiAgent.pathStatus == NavMeshPathStatus.PathComplete
            && aiAgent.remainingDistance <= 0.5f && !aiAgent.pathPending)
        {
            entityAnimator.MovingOver();
            entityAnimator.RunOver();
            return true;
        }

        return false;
    }
    #endregion
}
