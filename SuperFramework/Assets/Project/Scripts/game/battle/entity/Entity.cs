using System;
using UnityEngine;

public enum CampType
{
    Player = 0,
    Enemy = 1,
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

    public void Active(bool show = true)
    {
        gameObject.SetActive(show);
    }
}
