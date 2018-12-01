public class LoginScene : BaseScene
{
    public LoginScene(string mapName)
    {
        base.Init(SceneType.Login, mapName);
    }

    override public void Enter()
    {
        Loger.Info("LoginScene Enter");
    }
    override public void Exit()
    {
        Loger.Info("LoginScene Exit");
    }
}
