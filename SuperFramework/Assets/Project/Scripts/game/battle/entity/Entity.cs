using System;
using UnityEngine;
using UnityEngine.AI;

public enum CampType
{
    Enemy = 0,
    Player = 1,
    Neutral = 2
}

public class Entity : MonoBehaviour
{
    public EntityInfo DataInfo { get; private set; }
    public CampType GetCamp
    {
        get
        {
            return DataInfo.Camp;
        }
    }

    private EntityAnimator entityAnimator;

    private NavMeshAgent aiAgent = null;
    private int IsMoving;

    public void Active(bool show = true)
    {
        gameObject.SetActive(show);
    }

    public void Init(EntityInfo entityInfo)
    {
        DataInfo = new EntityInfo();
        DataInfo = entityInfo;
    }

    private void Awake()
    {
        aiAgent = GetComponent<NavMeshAgent>();
        entityAnimator = GetComponentInChildren<EntityAnimator>();
    }
    public Transform GetTransform()
    {
        return this.transform;
    }

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
}
