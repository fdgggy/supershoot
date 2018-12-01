using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICoTask
{
    void Enter();
    bool Excuse();
    void Exit();
}

public class CoroutineTaskManager : MonoBehaviour
{
    private Util.VoidDelegate onFinishCall = null;
    private Util.VoidDelegate onEachFinish = null;

    private GList<ICoTask> m_coNodeList = new GList<ICoTask>();

    public static CoroutineTaskManager Instance
    {
        get
        {
            return SingleComponent<CoroutineTaskManager>.Instance();
        }
    }

    public void SetCallBack(Util.VoidDelegate onFinishCall, Util.VoidDelegate onEachFinish = null)
    {
        this.onFinishCall = onFinishCall;
        this.onEachFinish = onEachFinish;
    }

    public void LoadScene(string name)
    {
        LoadSceneTask task = new LoadSceneTask(name);
        AddNode(task);
    }

    public void LoadEntity(string name)
    {
        LoadEntityTask task = new LoadEntityTask(name);
        AddNode(task);
    }

    public void LoadPrefab(string name)
    {

    }
    private void AddNode(ICoTask Node)
    {
        m_coNodeList.Add(Node);
    }

    public void Run()
    {
        ExcuseNext();
    }

    public void ExcuseOne(ICoTask node)
    {
        StartCoroutine(ExcuseNode(node));
    }

    public IEnumerator ExcuseNode(ICoTask node)
    {
        node.Enter();
        while (!node.Excuse())
        {
            yield return 0;
        }

        node.Exit();
        if (onEachFinish != null)
        {
            onEachFinish();
        }

        ExcuseNext();
    }
    private void ExcuseNext()
    {
        if (m_coNodeList.size > 0)
        {
            ICoTask taskNode = m_coNodeList.Pop();
            ExcuseOne(taskNode);
        }
        else
        {
            if (onFinishCall != null)
            {
                onFinishCall();
                onFinishCall = null;
            }
            onEachFinish = null;
        }
    }
}
