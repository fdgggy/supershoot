using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class ResUtil
{
    /// <summary>
    /// ab加载后都asset，获取后有些需要实例化，有些不需要(materail,texture)
    /// </summary>
    private static Dictionary<string, object> _res = new Dictionary<string, object>();


    public static void LoadPrefab(string prefabName, ResManager.ResBackHandle callBack)
    {
        if (_res.ContainsKey(prefabName))
        {
            object _obj = null;
            _res.TryGetValue(prefabName, out _obj);
            //回调
            if (callBack != null)
            {
                callBack(prefabName, _obj);
            }
        }
        else
        {   //创建对象池
            ResManager.Instance.LoadPrefab(prefabName, (string assetName, object go) =>
            {
                if (_res.ContainsKey(assetName))
                {
                    //回调
                    object _obj = null;
                    _res.TryGetValue(assetName, out _obj);
                    //回调
                    if (callBack != null)
                    {
                        callBack(assetName, _obj);
                    }

                    go = null;
                }
                else
                {
                    _res.Add(assetName, go);
                    GameObject gameObj = go as GameObject;
                    gameObj.SetActive(false);
                    //回调
                    if (callBack != null)
                    {
                        callBack(assetName, go);
                    }
                }

            });
        }
    }

    public static void ClearCache()
    {
        _res.Clear();

    }
}

