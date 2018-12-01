using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// AB类型
/// </summary>
public enum ABType
{
    Asset = 1,              //1 普通素材，被根素材依赖
    Root = 1 << 1,          //2 根素材
    Standalone = 1 << 2,    //4 被2个或以上对AB依赖，单独打包
    RootAsset = Asset | Root, //3 既是根又是被别人依赖的
}

public enum LoadState
{
    None = 1,
    Loading = 3,
    Complete = 4,  //AB 加载完成
}

public class ABInfo
{
    private ResManager.ResBackHandle csharpResBack = null; //AB依赖加载后的回调，不加载AB包资源
    private List<LoadAssetRequest> loadRequests = new List<LoadAssetRequest>();//实例化资源请求

    public string Name { get { return name; } }
    private string name = String.Empty;//名字，唯一表示
    private List<string> dependencies = new List<string>();

    private AssetBundle bundle = null;
    public int RefCount { get; private set; }
    public float UnloadTime { get; private set; }

    public LoadState State { get { return state; } }
    private LoadState state = LoadState.None;
    private Coroutine loadAssetCorotine = null;

    public void Release()
    {
        RefCount--;
        if (RefCount <= 0)
        {
            UnloadTime = Time.time;
        }
    }

    public void Retain()
    {
        RefCount++;
    }

    /// <summary>
    /// 外部调用
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="resBack">Res back.</param>
    /// <param name="request">Request.</param>
    public ABInfo(string name, ResManager.ResBackHandle resBack = null, LoadAssetRequest request = null)
    {
        RefCount = 1;
        this.name = name;

        if (resBack != null)
        {
            this.csharpResBack += resBack;
        }

        if (request != null)
        {
            loadRequests.Add(request);
        }


        string[] dependList = ResManager.Instance.Manifest.GetAllDependencies(this.name);
        if (dependList.Length > 0)
        {
            for (int i = 0; i < dependList.Length; i++)
            {
                string abName = dependList[i];   //有可能依赖正在加载，不要重复加载，会报错
                ABInfo res = ResManager.Instance.GetAB(abName);

                //DebugLog.Logf("self:{0} depend:{1}", this.name, abName);
                if (res == null)
                {
                    //DebugLog.Logf("self:{0} new ABInfo abName:{1}", this.name, abName);
                    ABInfo depend = new ABInfo(abName);
                    dependencies.Add(abName);
                    ResManager.Instance.AppendAB(abName, depend);
                }
                else
                {
                    //DebugLog.Logf("self:{0} not new ABInfo abName:{1}", this.name, abName);
                    res.Retain();
                    dependencies.Add(abName);
                }
            }
        }
    }
    /// <summary>
    /// 单纯的Load自身
    /// </summary>
    public void Load()
    {
        state = LoadState.Loading;
        ResManager.Instance.StartCoroutine(LoadAB());
    }
    private IEnumerator StartLoad()
    {
        if (state == LoadState.None)
        {
            //DebugLog.Logf("StartLoad AB:{0}", this.name);
            state = LoadState.Loading;   //同时是根和依赖资源时有用，必须加

            if (dependencies.Count > 0)
            {
                for (int i = 0; i < dependencies.Count; i++)
                {
                    ABInfo depend = ResManager.Instance.GetAB(dependencies[i]);
                    if (depend.State == LoadState.None) //依赖未开始都就开始
                    {
                        depend.Load();
                    }

                    while (depend.State != LoadState.Complete) //有一个未完成
                    {
                        yield return null;
                    }
                }
            }

            Load();//一定要控制好，有2处调用Load
        }
    }
    /// <summary>
    /// 开始加载
    /// </summary>
    public void Start()
    {
        ResManager.Instance.StartCoroutine(StartLoad());
    }

    /// <summary>
    /// 增加资源加载请求，只能是ResAgent调用, 针对既是依赖，又是根的情况
    /// </summary>
    /// <param name="request">Request.</param>
    public void AddLoadRequest(LoadAssetRequest request)
    {
        loadRequests.Add(request);

        if (state == LoadState.Complete && loadRequests.Count <= 1) //先做依赖，再做根(做依赖完成了)
        {
            //DebugLog.Logf("AddLoadRequest abInfo:{0} loadrequest but only one LoadAssetRequest", this.name);
            loadAssetCorotine = ResManager.Instance.StartCoroutine(LoadAsset());
        }
    }

    /// <summary>
    /// 加载自身AB包
    /// </summary>
    /// <returns>The ab.</returns>
    IEnumerator LoadAB()
    {
        //DebugLog.Logf("LoadAB abInfo:{0} start", this.name);

        string url = Util.ABInterLoadWWWPath + "/" + this.name;
        WWW req = WWW.LoadFromCacheOrDownload(url, ResManager.Instance.GetABHash(this.name), 0);

        yield return req;
        bundle = req.assetBundle;

        //DebugLog.Logf("LoadAB:abInfo:{0} end", this.name);

        if (csharpResBack != null)  //没有依赖时，回调通知订阅者
        {
            //DebugLog.Logf("LoadAB:{0} ok, csharpResBack", this.name);
            csharpResBack(this.name, this);
            csharpResBack = null;
        }
        state = LoadState.Complete;

        if (loadRequests.Count > 0)
        {
            loadAssetCorotine = ResManager.Instance.StartCoroutine(LoadAsset());
        }
    }

    /// <summary>
    /// 异步加载具体资源类型
    /// </summary>
    /// <returns>The load.</returns>
    public IEnumerator LoadAsset()
    {
        //DebugLog.Logf("LoadAsset ab:{0} start", this.name);
        for (int i = 0; i < loadRequests.Count; i++)
        {
            string assetName = loadRequests[i].assetName;
            AssetBundleRequest request = bundle.LoadAssetAsync(assetName, loadRequests[i].assetType);

            yield return request;

            if (request.isDone)
            {
                if (loadRequests[i].luaCallBack != null)
                {
                    //DebugLog.Logf("LoadAsset luaCallBack asset:{0}", this.name);
                    loadRequests[i].luaCallBack(assetName, request.asset);

                    Release();
                }
            }
        }

        loadRequests.Clear();
        loadAssetCorotine = null;

        //DebugLog.Logf("LoadAsset ab:{0} end", this.name);
    }
    /// <summary>
    /// 释放外部资源请求，切换场景时，就不要再回调出去了(对象池已经都释放，但是回调回去还是会触发代码逻辑,引起空引用的错误)
    /// </summary>
    public void DisposeRequest()
    {
        //DebugLog.Logf("DisposeRequest abInfo:{0} start", this.name);
        if (loadAssetCorotine != null)
        {
            //DebugLog.Logf("StopCoroutine::=> abInfo:{0} loadAssetCorotine", this.name);
            ResManager.Instance.StopCoroutine(loadAssetCorotine);
        }

        for (int i = 0; i < loadRequests.Count; i++)
        {
            Release();
        }
        loadRequests.Clear();
    }
    /// <summary>
    /// 彻底释放
    /// </summary>
    /// <param name="isThorough"></param>
    public void Dispose(bool isThorough)
    {
        //DebugLog.Logf("ab:{0} Dispose", this.name);

        if (bundle != null)
        {
            //DebugLog.Logf("ab:{0} Dispose, bundle is not null, isThorough:{1}", this.name, isThorough);
            bundle.Unload(isThorough);
            bundle = null;
            state = LoadState.None;
        }
        else
        {
            //DebugLog.LogError("ab:{0} not Dispose, bundle is null", this.name);
        }

        for (int i = 0; i < dependencies.Count; i++)
        {
            ABInfo depend = ResManager.Instance.GetAB(dependencies[i]);
            depend.Release();
            //DebugLog.Logf("self:{0} Dispose, depend:{1} Release, Ref:{2}", this.name, dependencies[i], depend.RefCount);
        }
        dependencies.Clear();
        dependencies = null;

        csharpResBack = null;
        loadRequests.Clear();
        loadRequests = null;
    }
}
