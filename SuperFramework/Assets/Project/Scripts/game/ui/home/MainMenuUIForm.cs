using System;
using System.Collections.Generic;
using SUIFW;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUIForm : BaseUIForm
{
    public override UIType GetUIType()
    {
        //窗体的性质
        CurrentUIType.UIForms_ShowMode = UIFormShowMode.HideOther;
        return CurrentUIType;
    }
    public void Start()
    {
        InitView();
        InitEvent();
    }
    private void InitView()
    {
        SetText("HighScoreInfo", 0);
        SetText("JetPacksInfo", 1);
        SetText("GemsInfo", 2);
        SetText("ReviveInfo", 3);
        SetText("CoinsInfo", 4);
        SetText("MeInfo", 6);
        SetText("ShopInfo", 7);
        SetText("TopSkillInfo", 8);
        SetText("InventoryInfo", 9);

        GameObject enterInfo = GetChildObj("EnterInfo");
        Text enterText = enterInfo.GetComponent<Text>();
        if (enterText != null)
        {
            string result = LauguageManager.Instance.GetText(10);
            enterText.text = result.Replace("\\n", "\n");
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
        RigisterButtonObjectEvent("Enter", (GameObject go)=> {
            OpenUIForm(DemoProject.ProConst.LevelUIForm);
        });

        RigisterButtonObjectEvent("Me", (GameObject go) => {
            OpenUIForm(DemoProject.ProConst.SelectRoleUIForm);
        });

        RigisterButtonObjectEvent("Shop", (GameObject go) => {
            OpenUIForm(DemoProject.ProConst.ShopUIForm);
        });
    }
}
