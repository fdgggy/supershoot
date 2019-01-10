using SUIFW;
using UnityEngine;
public class MainScene : BaseScene
{
    public MainScene(string mapName)
    {
        base.Init(SceneType.Main, mapName);
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
