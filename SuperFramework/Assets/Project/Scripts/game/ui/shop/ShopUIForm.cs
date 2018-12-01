using System;
using System.Collections.Generic;
using SUIFW;
using UnityEngine;
using UnityEngine.UI;

public class ShopUIForm : BaseUIForm
{
    public void Start()
    {
        CurrentUIType.UIForms_ShowMode = UIFormShowMode.Normal;

        InitView();
        InitEvent();
    }
    private void InitView()
    {
        SetText("BackInfo", 11);
        SetText("GemsInfo", 2);
        SetText("ReviveInfo", 3);
        SetText("CoinsInfo", 4);

        SetText("PistolesInfo", 13);
        SetText("RiflesInfo", 14);

        SetTextEx("SubMachineInfo", 15);
        SetTextEx("RocketLauncherInfo", 16);

        SetText("MeleeInfo", 17);
        SetText("GrenadesInfo", 18);
        SetText("JetPacksInfo", 19);
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
    private void SetTextEx(string child, int index)
    {
        GameObject childInfo = GetChildObj(child);
        Text childInfoText = childInfo.GetComponent<Text>();
        if (childInfoText != null)
        {
            string result = LauguageManager.Instance.GetText(index);
            childInfoText.text = result.Replace("\\n", "\n");
        }
    }
    private void InitEvent()
    {
        RigisterButtonObjectEvent("Back", (GameObject go) => {
            CloseUIForm();
        });
    }
}
