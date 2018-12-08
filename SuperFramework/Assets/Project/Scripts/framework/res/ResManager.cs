using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using SimpleJSON;
using UObject = UnityEngine.Object;

public class LoadAssetRequest
{
    public Type assetType;
    public string assetName;
    public ResManager.ResBackHandle luaCallBack;  //lua回调
}

public class ResManager : MonoBehaviour
{
    public delegate void ResBackHandle(string assetName, object obj);
    //json存储的
    private class PrefabToAbData
    {
        public string abName;
        public string md5;
        public string path;      //兼容模式用的
    }
    private Dictionary<string, PrefabToAbData> m_pf2Asset = new Dictionary<string, PrefabToAbData>();
    /// <summary>
    /// 检查间隔时间
    /// </summary>
    private float checkInterval = 5f;
    /// <summary>
    /// AB延迟卸载时间
    /// </summary>
    private float delayInterval = 3f;

    public AssetBundleManifest Manifest
    {
        get { return m_AssetBundleManifest; }
    }
    private AssetBundleManifest m_AssetBundleManifest = null;

    private List<string> cacheToRemove = new List<string>();
    private Dictionary<string, ABInfo> abInfos = new Dictionary<string, ABInfo>();

    public static ResManager Instance
    {
        get
        {
            return SingleComponent<ResManager>.Instance();
        }
    }
    private AssetBundle sceneBundle = null;

    private void Awake()
    {
        InvokeRepeating("GarbageCollect", 0, checkInterval);
    }

    public void Init(Util.VoidDelegate func)
    {
        m_pf2Asset.Clear();

        TextAsset configInfo = Resources.Load<TextAsset>("assetInfo");
        JSONNode jsonNode = JsonUtils.LoadJson(configInfo.bytes);

        if (jsonNode != null)
        {
            foreach (string key in jsonNode.Keys)
            {
                JSONClass jsonObj = jsonNode[key].AsObject;

                PrefabToAbData preabdata = new PrefabToAbData()
                {
                    abName = jsonObj["abName"],
                    md5 = jsonObj["md5"],
                    path = jsonObj["path"],
                };

                m_pf2Asset.Add(key, preabdata);
            }
        }

#if _DEBUG
        if (func != null) func();
#else
        StartCoroutine(LoadMainfest("StreamingAssets", func));
#endif
    }

    public Hash128 GetABHash(string abName)
    {
        return m_AssetBundleManifest.GetAssetBundleHash(abName);
    }

    public IEnumerator LoadMainfest(string manifestName, Util.VoidDelegate func)
    {
        string url = Util.ABInterLoadWWWPath + "/" + manifestName;
        WWW req = new WWW(url);

        yield return req;

        AssetBundleRequest request = req.assetBundle.LoadAssetAsync("AssetBundleManifest", typeof(AssetBundleManifest));
        yield return request;

        m_AssetBundleManifest = request.asset as AssetBundleManifest;

        if (func != null) func();
    }

    /// <summary>
    /// 只需要加载bundle并回调出去即可，在SceneTransMgr加载场景
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="prefabName"></param>
    /// <param name="callBack"></param>
    public void LoadScene(string prefabName, ResManager.ResBackHandle callBack)
    {
        if (m_pf2Asset.ContainsKey(prefabName))
        {
            PrefabToAbData preabdata = m_pf2Asset[prefabName];
#if UNITY_EDITOR
            if (callBack != null)
            {
                callBack(prefabName, preabdata.path);
            }
#else
            StartCoroutine(LoadSceneAB(preabdata.abName, callBack));
#endif
        }
        else
        {
            Loger.Error("not found prefabName:{0}", prefabName);
        }
    }

    private IEnumerator LoadSceneAB(string abName, ResManager.ResBackHandle callBack)
    {
        string url = Util.ABInterLoadWWWPath + "/" + abName;
        WWW req = WWW.LoadFromCacheOrDownload(url, m_AssetBundleManifest.GetAssetBundleHash(abName), 0);

        yield return req;
        sceneBundle = req.assetBundle;

        callBack(abName, sceneBundle);
    }

    public void UnLoadSceneAB()
    {
        if (sceneBundle != null)
        {
            sceneBundle.Unload(false);
            sceneBundle = null;
        }
        else
        {
            Loger.Error("UnLoadSceneAB not found Scene AB");
        }
    }

    public void LoadPrefab(string prefabName, ResBackHandle callBack = null)
    {
        if (m_pf2Asset.ContainsKey(prefabName))
        {
            PrefabToAbData preabdata = m_pf2Asset[prefabName];
#if _DEBUG && UNITY_EDITOR
            UnityEngine.Object prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(preabdata.path);

            if (callBack != null)
            {
                callBack(prefabName, prefab);
                callBack = null;
            }
#else
            LoadAsset<GameObject>(preabdata.abName, prefabName, callBack);
#endif
        }
        else
        {
            Loger.Error("not found prefabName:{0}", prefabName);
        }
    }

    public void LoadScriptObject(string prefabName, ResBackHandle callBack = null)
    {
        if (m_pf2Asset.ContainsKey(prefabName))
        {
            PrefabToAbData preabdata = m_pf2Asset[prefabName];
#if _DEBUG && UNITY_EDITOR
            UnityEngine.Object prefab = UnityEditor.AssetDatabase.LoadAssetAtPath<ScriptableObject>(preabdata.path);

            if (callBack != null)
            {
                callBack(prefabName, prefab);
                callBack = null;
            }
#else
            LoadAsset<ScriptableObject>(preabdata.abName, prefabName, callBack);
#endif
        }
        else
        {
            Loger.Error("not found prefabName:{0}", prefabName);
        }
    }

    // prefabName:"liYao_R"
    //"abName":"liyao_r.unity3d",  一个AB包就是一个预制件
    public void LoadAsset<T>(string abName, string assetName, ResManager.ResBackHandle callBack) where T : UObject
    {
        LoadAssetRequest request = new LoadAssetRequest()
        {
            assetType = typeof(T),
            assetName = assetName,
            luaCallBack = callBack,
        };

        ABInfo abInfo = null;
        if (abInfos.TryGetValue(abName, out abInfo))  //如果有，必定触发了Start
        {
            abInfo.Retain();
            abInfo.AddLoadRequest(request);
        }
        else
        {
            //DebugLog.Logf("ResManager load not exist abInfo:{0}", abName);
            abInfo = new ABInfo(abName, null, request);
            AppendAB(abName, abInfo);

            abInfo.Start();
        }
    }

    /// <summary>
    /// 提供给当前AB或者依赖AB
    /// </summary>
    /// <param name="name">Name.</param>
    /// <param name="info">Info.</param>
    public void AppendAB(string name, ABInfo info)
    {
        if (!abInfos.ContainsKey(name))
        {
            //DebugLog.Logf("AppendAB abName:{0}", name);
            abInfos.Add(name, info);
        }
        else
        {
            Loger.Error("AppendAB failed, already contain ab:{0}", name);
        }
    }

    /// <summary>
    /// 提供给获取依赖相关
    /// </summary>
    /// <returns>The ab.</returns>
    /// <param name="name">Name.</param>
    public ABInfo GetAB(string name)
    {
        ABInfo info = null;
        abInfos.TryGetValue(name, out info);
        return info;
    }


    public void HttpRequest(string url, Util.StringDelegate callBack)
    {
        StartCoroutine(httpRequest(url, null));
    }

    public IEnumerator httpRequest(string url, Util.StringDelegate callBack)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            yield return request.Send();

            if (request.error != null)
            {
                Loger.Error("httpRequest url:{0} err:{1}", url, request.error);
            }
            else
            {
                string result = request.downloadHandler.text;
                callBack(result);
            }
        }
    }

    /// <summary>
    /// 垃圾回收, 每5s检测一次，引用为0的，且超过延迟卸载时间的卸载
    /// 延迟卸载好处：1.实例化处理渲染需要1，2帧，如果直接unload会对资源渲染造成影响 2.防止稍后再次加载
    /// </summary>
    private void GarbageCollect()
    {
        foreach (var kv in abInfos)
        {
            var key = kv.Key;
            var value = kv.Value;
            if (value.RefCount <= 0)
            {
                if ((Time.time - value.UnloadTime) > delayInterval)
                {
                    cacheToRemove.Add(key);
                }
            }
        }

        for (int i = cacheToRemove.Count - 1; i >= 0; i--)
        {
            try
            {
                string key = cacheToRemove[i];
                ABInfo ab = abInfos[key];
                ab.Dispose(false);
                abInfos.Remove(key);

                cacheToRemove.RemoveAt(i);
            }
            catch (Exception e)
            {
                Loger.Error("GarbageCollect exception, {0}", e.Message);
            }
        }

        if (cacheToRemove.Count > 0)
        {
            Loger.Error("GarbageCollect cacheToRemove must be empty!");
        }

        //DumpABInfo();
    }


    private void DumpABInfo()
    {
        foreach (var kv in abInfos)
        {
            Loger.Info("DumpABInfo ab:{0} ref:{1}", kv.Value.Name, kv.Value.RefCount);
        }
    }

    //lua那边调度过来,切换场景时Unity会把所有对象（gameobject, ab 加载都asset)释放，只有ab本身不释放
    public void ClearNoUseRes()
    {
        foreach (var kv in abInfos)
        {
            kv.Value.DisposeRequest();
        }

        if (sceneBundle != null)
        {
            sceneBundle.Unload(false);
            sceneBundle = null;
        }

        //释放缓存的预制件
        ResUtil.ClearCache();        //卸载对象池
        //Resources.UnloadUnusedAssets(); //卸载所有没引用的Asset资源，快速切换场景会报错 ！！！！！！！ 切场景不要卸载，Unity自己会处理

        System.GC.Collect();
        System.GC.WaitForPendingFinalizers(); //挂起当前线程，直到处理终结器队列的线程晴空该队列为止
    }

    /// <summary>
    /// 销毁时移除所有AB，释放所有内存
    /// </summary>
    private void OnDestroy()
    {
        Loger.Info("ResManager OnDestroy");

        this.StopAllCoroutines();  //停止所有依赖的协程
        foreach (var kv in abInfos)
        {
            if (kv.Value.State == LoadState.Complete)     //释放所有加载OK的AB
            {
                kv.Value.Dispose(false);
            }
        }

        abInfos.Clear();
    }
}
