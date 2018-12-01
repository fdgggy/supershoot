using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class SingletonHolder<T> where T:new()
{
    private static T _instance=default(T);

    public static T GetInstance()
    {
             if(_instance==null){
                        _instance=new T();
                 }
                return _instance;
    }
}


public class TComponent<T> where T : MonoBehaviour
{
    public static T Instance()
    {
        string name = typeof(T).ToString();
        GameObject go = new GameObject();
        go.name = name;
#if UNITY_EDITOR
        go.hideFlags = HideFlags.NotEditable;
#else
        go.hideFlags = HideFlags.NotEditable | HideFlags.DontSave;
#endif
       UnityEngine.Object.DontDestroyOnLoad(go);
       T comp = go.AddComponent<T>();
       return comp;
    } 
}

//单独的一个控件脚本挂入
public class SingleComponent<T> where T : MonoBehaviour
{
    private static T _instance = null;

    public static T Instance()
    {
        if (_instance == null)
        {
            string name = typeof(T).ToString();
            GameObject go = GameObject.Find(name);
            if (go == null)
            {
                go = new GameObject();
                go.name = name;
#if UNITY_EDITOR
                //go.hideFlags = HideFlags.NotEditable;
#else
                go.hideFlags = HideFlags.NotEditable | HideFlags.DontSave;
#endif
                UnityEngine.Object.DontDestroyOnLoad(go);
            }
            else
            {
            }
            T comp = go.GetComponent<T>();
            if (comp == null)
            {
                comp = go.AddComponent<T>();
            }
            _instance = comp;
        }
        return _instance;
    }

}