using SUIFW;
using UnityEngine;

public class BattleScene : BaseScene
{
    private LoadingUIForm loadingView = null;
    public BattleScene(string mapName)
    {
        base.Init(SceneType.Battle, mapName);
    }

    override public void Enter()
    {
        Loger.Info("BattleScene Enter");

        CoroutineTaskManager.Instance.SetCallBack(OnFinished, OnEachFinish);
        CoroutineTaskManager.Instance.LoadScene(MapName);
        CoroutineTaskManager.Instance.LoadEntity("HeroHDWeapons");

        UIManager.Instance.ShowUIForms(DemoProject.ProConst.LoadingUIForm, (BaseUIForm baseUIForms) => 
        {
            loadingView = baseUIForms as LoadingUIForm;
            loadingView.TaskAdd();
            loadingView.TaskAdd();

            CoroutineTaskManager.Instance.Run();
        });
    }

    override public void Exit()
    {
        Loger.Info("BattleScene Exit");
    }

    private void OnEachFinish()
    {
        Loger.Info("BattleScene OnEachFinish");
        if (loadingView != null)
        {
            loadingView.TaskComplete();
        }
    }

    private void OnFinished()
    {
        Loger.Info("BattleScene OnFinished");
        ResUtil.LoadPrefab("HeroHDWeapons", (string asstName, object go)=> {
            GameObject entityObj = GameObject.Instantiate(go as GameObject);
            entityObj.SetActive(true);

            loadingView.Close();
        });
    }
}
