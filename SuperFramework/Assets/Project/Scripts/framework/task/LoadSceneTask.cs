using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class LoadSceneTask : ICoTask
{
    private string sceneName = string.Empty;
    private AsyncOperation aysnOp = null;
    public LoadSceneTask(string name)
    {
        sceneName = name;
    }

    public void Enter()
    {
        ResManager.Instance.LoadScene(sceneName, (string assetName, object obj) => {
#if UNITY_EDITOR
            string path = obj as string;
            aysnOp = EditorApplication.LoadLevelAsyncInPlayMode(path);
#else
            aysnOp = SceneManager.LoadSceneAsync(this.sceneName);
#endif
        });
    }
    public bool Excuse()
    {
        if (aysnOp != null && aysnOp.isDone)
        {
            return true;
        }

        return false;
    }

    public void Exit()
    {
        aysnOp = null;
#if !UNITY_EDITOR
        ResManager.Instance.UnLoadSceneAB();
#endif
    }
}
