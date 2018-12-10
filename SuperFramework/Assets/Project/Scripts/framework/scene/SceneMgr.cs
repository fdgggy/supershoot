using UnityEngine;

public enum SceneType
{
    None = 0,
    Login,
    CreateRole,
    Battle,

    Num,
}
public class SceneMgr : MonoBehaviour
{
    private BaseScene curScene = null;
    public static SceneMgr Instance
    {
        get
        {
            return SingleComponent<SceneMgr>.Instance();
        }
    }
    private bool IsValid(SceneType sceneType)
    {
        if (sceneType > SceneType.None && sceneType < SceneType.Num)
        {
            return true;
        }

        return false;
    }

    public void EnterScene(SceneType sceneType, string mapName, LevelData leveData = null)
    {
        if (!IsValid(sceneType))
        {
            Loger.Error("sceneType:{0} is invalid", sceneType);
            return;
        }

        if (curScene != null)
        {
            if (curScene.SceneEnum == sceneType)
            {
                Loger.Error("already in this scene:{0}", sceneType);
                return;
            }

            curScene.Exit();
        }

        if (sceneType == SceneType.Login)
        {
            curScene = new LoginScene(mapName);
        }
        else if (sceneType == SceneType.Battle)
        {
            curScene = new BattleScene(mapName, leveData);
        }
        curScene.Enter();
    }
}
