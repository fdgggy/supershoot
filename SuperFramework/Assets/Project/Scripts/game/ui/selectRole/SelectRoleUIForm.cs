using System;
using System.Collections.Generic;
using SUIFW;
using UnityEngine;
using UnityEngine.UI;

public class SelectRoleUIForm : BaseUIForm
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
