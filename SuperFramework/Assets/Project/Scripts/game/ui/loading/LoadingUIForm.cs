using UnityEngine;
using UnityEngine.UI;
using SUIFW;
using System.Collections;

public class LoadingUIForm : BaseUIForm
{
    private int compCount = 0;
    private int taskCount = 0;
    public Text progress;

    public void TaskAdd()
    {
        taskCount++;
    }

    public void TaskComplete()
    {
        compCount++;
        Loger.Info("compCount:{0}", compCount);
    }

    public void Start()
    {
        CurrentUIType.UIForms_ShowMode = UIFormShowMode.HideOther;
    }

    private void Update()
    {
        float result = compCount*1f / taskCount;
        ProgressChange(System.Math.Round(result, 2));
    }

    public void ProgressChange(double progress)
    {
        this.progress.text = string.Format("{0}%", progress*100);
    }

    public void Close()
    {
        StartCoroutine(CloseForm());
    }

    IEnumerator CloseForm()
    {
        yield return null;
        CloseUIForm();
    }
}
