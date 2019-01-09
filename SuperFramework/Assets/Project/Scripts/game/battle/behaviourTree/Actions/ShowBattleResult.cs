using BehaviorDesigner.Runtime;
using BehaviorDesigner.Runtime.Tasks;
using SUIFW;

public class ShowBattleResult : Action
{
    public SharedBattleResultType battleResult;
    private bool showOK = false;

    public override void OnStart()
    {
        if ((BattleResultType)battleResult.GetValue() == BattleResultType.None)
        {
            Loger.Error("ShowBattleResult set result type error !");
            return;
        }

        UIManager.Instance.ShowUIForms(DemoProject.ProConst.BattleResultUIForm, (BaseUIForm baseUIForms) =>
        {
            BattleResultUIForm resultUIForm = baseUIForms as BattleResultUIForm;
            resultUIForm.SetResult((BattleResultType)battleResult.GetValue());

            showOK = true;
        });
    }

    public override TaskStatus OnUpdate()
    {
        if (showOK)
        {
            return TaskStatus.Success;
        }

        return TaskStatus.Running;
    }
}
