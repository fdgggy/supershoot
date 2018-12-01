using System;
using System.Collections.Generic;
using SUIFW;
using UnityEngine;
using UnityEngine.UI;

struct LevelInfo
{
    public int Id;
    public string Name;
}

public class LevelUIForm : BaseUIForm
{
    private List<LevelInfo> m_Levels;
    public void Start()
    {
        CurrentUIType.UIForms_ShowMode = UIFormShowMode.Normal;

        m_Levels = new List<LevelInfo>()
        {
            new LevelInfo(){ Id = 1, Name = "Level_1"},
            new LevelInfo(){ Id = 2, Name = "Level_2"},
            new LevelInfo(){ Id = 3, Name = "Level_3"},
            new LevelInfo(){ Id = 4, Name = "Level_4"},
        };
        InitView();
        InitEvent();
    }
    private void InitView()
    {
        SetText("BackInfo", 11);

        GameObject levelObj = GetChildObj("levelItem");
        if (levelObj == null)
        {
            Loger.Error("LevelUIForm::InitView don't find the levelItem !");
            return;
        }

        GameObject parentObj = GetChildObj("levelGroup");
        foreach (var level in m_Levels)
        {
            GameObject go = GameObject.Instantiate<GameObject>(levelObj);
            Transform info = go.transform.Find("levelInfo");
            if (info != null)
            {
                Text infoText = info.GetComponent<Text>();
                infoText.text = level.Name;
            }
            go.name = level.Name;
            go.transform.SetParent(parentObj.transform);
            go.transform.localScale = Vector3.one;

            RigisterButtonByObject(go, LevelButtonCallBack);
            go.SetActive(true);
        }
    }

    private void LevelButtonCallBack(GameObject go)
    {
        Loger.Info("go.name:{0}", go.name);
        SceneMgr.Instance.EnterScene(SceneType.Battle, "Map_001");
    }

    private void SetText(string child, int index)
    {
        GameObject childInfo = GetChildObj(child);
        Text enterText = childInfo.GetComponent<Text>();
        if (enterText != null)
        {
            string result = LauguageManager.Instance.GetText(index);
            enterText.text = result;
        }
    }

    private void InitEvent()
    {
        RigisterButtonObjectEvent("Back", (GameObject go) => {
            CloseUIForm();
        });
    }
}
