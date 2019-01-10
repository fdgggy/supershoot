using SUIFW;
using UnityEngine;
using UnityEngine.UI;

public class BattleResultUIForm : BaseUIForm
{
    public override UIType GetUIType()
    {
        //窗体的性质
        CurrentUIType.UIForms_ShowMode = UIFormShowMode.HideOther;
        return CurrentUIType;
    }

    public void Start()
    {
        //InitView();
        InitEvent();
    }

    private void InitEvent()
    {
        RigisterButtonObjectEvent("BackToHome", (GameObject go) => {
            //CloseUIForm();
            //OpenUIForm(DemoProject.ProConst.MainMenuUIForm);
        });
    }

    public void SetResult(BattleResultType resultType)
    {
        if (resultType == BattleResultType.Win)
        {
            SetText("ResultInfo", 20);
        }
        else if (resultType == BattleResultType.Failed)
        {
            SetText("ResultInfo", 21);
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
}
