using SUIFW;
using UnityEngine;

public class BattleScene : BaseScene
{
    private LoadingUIForm loadingView = null;
    private LevelData leveData = null;
    public BattleScene(string mapName, LevelData leveData)
    {
        base.Init(SceneType.Battle, mapName);

        this.leveData = leveData;
    }

    override public void Enter()
    {
        Loger.Info("BattleScene Enter");

        CoroutineTaskManager.Instance.SetCallBack(OnFinished, OnEachFinish);
        CoroutineTaskManager.Instance.LoadScene(MapName);
        //CoroutineTaskManager.Instance.LoadEntity("HeroHDWeapons");

        UIManager.Instance.ShowUIForms(DemoProject.ProConst.LoadingUIForm, (BaseUIForm baseUIForms) => 
        {
            loadingView = baseUIForms as LoadingUIForm;
            loadingView.TaskAdd();
            //loadingView.TaskAdd();

            CoroutineTaskManager.Instance.Run();
        });
    }

    override public void Exit()
    {
        Loger.Info("BattleScene Exit");
        AudioManager.Instance.ClearBattleAudio();
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

        Role role = ExcelDataManager.Instance.GetExcel(ExcelType.Role) as Role;
        RoleData roleData = role.QueryByID(1);

        EntityInfo entityInfo = new EntityInfo()
        {
            PrefabName = roleData.Prefabname,
            EntityId = EntityManager.Instance.EntityId,
            Camp = CampType.Player,
            WeaponIds = roleData.Weaponids,
        };

        EntityManager.Instance.CreateEntity(entityInfo, new Vector3(0, -18, 0), Quaternion.Euler(0, 180, 0), (Entity go) =>
        {
            go.Active(true);

            if (leveData == null)
            {
                Loger.Error("BattleScene OnFinished, but the leveData is null, please check !");
                return;
            }

            LevelManager.Instance.Init(leveData.Levelai);

            UIManager.Instance.ShowUIForms(DemoProject.ProConst.BattleUIForm, (BaseUIForm baseUIForms) =>
            {

            });
        });
    }
}
