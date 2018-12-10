using System;
using UnityEngine;

public enum CampType
{
    Enemy = 0,
    Player = 1,
    Neutral = 2
}

public class Entity
{
    private EntityInfo entityInfo;
    private GameObject gameObject;

    public CampType GetCamp
    {
        get
        {
            return entityInfo.campType;
        }
    }

    public Entity(EntityInfo info, GameObject go)
    {
        entityInfo = info;
        gameObject = go;
    }

    public void Active(bool show = true)
    {
        gameObject.SetActive(show);
    }
}
