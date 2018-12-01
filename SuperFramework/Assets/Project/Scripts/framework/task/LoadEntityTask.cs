using System;
using System.Collections.Generic;
using UnityEngine;


public class LoadEntityTask : ICoTask
{
    private string entityName = string.Empty;
    private int totalCount = 0;
    private int curCount = 0;

    private AsyncOperation aysnOp = null;
    public LoadEntityTask(string name)
    {
        entityName = name;
    }

    public void Enter()
    {
        totalCount = 1;
        curCount = 0;
        ResUtil.LoadPrefab(this.entityName, EntityLoaded);
    }

    private void EntityLoaded(string assetName, object go)
    {
        curCount++;
    }

    public bool Excuse()
    {
        if (curCount >= totalCount)
        {
            return true;
        }

        return false;
    }

    public void Exit()
    {
    }
}
