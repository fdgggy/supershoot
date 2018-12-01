public class BaseScene
{
    public SceneType SceneEnum 
    {
        get;
        private set;
    }
    public string MapName
    {
        get;
        private set;
    }

    protected void Init(SceneType sceneType, string mapName)
    {
        SceneEnum = sceneType;
        MapName = mapName;
    }

    virtual public void Enter()
    {

    }

    virtual public void Exit()
    {

    }
}
