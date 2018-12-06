using System;
using System.Collections.Generic;
using SUIFW;
using UnityEngine;
using UnityEngine.UI;

public class LevelUIForm : BaseUIForm
{
    private Level level = null;

    public void Start()
    {
        CurrentUIType.UIForms_ShowMode = UIFormShowMode.Normal;

        level = ExcelDataManager.Instance.GetExcel(ExcelType.Level) as Level;
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

        foreach(KeyValuePair<int, LevelData> kv in level.GetRows())
        {
            GameObject go = GameObject.Instantiate<GameObject>(levelObj);
            Transform info = go.transform.Find("levelInfo");
            if (info != null)
            {
                Text infoText = info.GetComponent<Text>();
                infoText.text = kv.Value.Levelname;
            }
            go.name = kv.Value.Id.ToString();
            go.transform.SetParent(parentObj.transform);
            go.transform.localScale = Vector3.one;

            RigisterButtonByObject(go, LevelButtonCallBack);
            go.SetActive(true);
        }
    }

    private void LevelButtonCallBack(GameObject go)
    {
        if (level != null)
        {
            LevelData leveData = level.QueryByID(int.Parse(go.name));
            if (leveData == null)
            {
                Loger.Warn("LevelButtonCallBack levelId:{0} data is null", go.name);
                return;
            }

            Map map = ExcelDataManager.Instance.GetExcel(ExcelType.Map) as Map;
            if (map != null)
            {
                MapData mapData = map.QueryByID(leveData.Mapid);
                if (mapData == null)
                {
                    Loger.Warn("LevelButtonCallBack mapId:{0} data is null", leveData.Mapid);
                    return;
                }

                SceneMgr.Instance.EnterScene(SceneType.Battle, mapData.Mapname);
            }
        }
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
