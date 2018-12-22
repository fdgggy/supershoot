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

    public EntityInfo EntityInfo { get; set; }
    public CampType GetCamp
    {
        get
        {
            return EntityInfo.campType;
        }
    }
    private NavMeshAgent aiAgent = null;
    private Animator animator = null;
    private int IsMoving;

    public void Active(bool show = true)
    {
        gameObject.SetActive(show);
    }

    private void Awake()
    {
        aiAgent = GetComponent<NavMeshAgent>();
        animator = GetComponentInChildren<Animator>();
        InitHashIDs();
    }
    public Transform GetTransform()
    {
        return this.transform;
    }

    private void InitHashIDs()
    {
        IsMoving = Animator.StringToHash("IsMoving");
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
        aiAgent.SetDestination(target);
    }

    public bool MoveOver()
    {
        if (aiAgent.enabled == true 
            && aiAgent.pathStatus == NavMeshPathStatus.PathComplete 
            && aiAgent.remainingDistance <= 0.5f && !aiAgent.pathPending)
        {
            return true;
        }

        animator.SetBool(IsMoving, true);
        return false;
    }
}
