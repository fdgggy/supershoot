using System;
using System.Collections.Generic;
using SUIFW;
using UnityEngine;
using UnityEngine.UI;

public class BattleUIForm : BaseUIForm
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

        //Button btn = this.GetComponentInChildren<Button>();
        //btn.onClick.AddListener(OnClick);
    }
    //private void OnClick()
    //{
    //    Debug.Log("Button Clicked. ClickHandler.");
    //}


    private void InitView()
    {
        SetText("BackToHomeInfo", 11);
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
        RigisterButtonObjectEvent("BackToHome", (GameObject go) => {
            //OpenUIForm(DemoProject.ProConst.MainMenuUIForm);
        });
    }
}
