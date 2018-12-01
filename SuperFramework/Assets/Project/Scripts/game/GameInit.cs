using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using NLog.Unity;
using SUIFW;

public class GameInit : MonoBehaviour {

	void Awake () 
    {
        DontDestroyOnLoad(this.gameObject);
        Init();
    }

    private void Init()
    {
        LoadConfig();
        ResManager.Instance.Init(() =>
        {
            ExcelDataManager.Instance.Init(() =>
            {
                Debug.Log("ExcelDataManager load over!");
                English exd = ExcelDataManager.Instance.GetExcel(ExcelType.English) as English;
                EnglishData d = exd.QueryByID(8);
                Debug.Log("Id:" + d.Id + " Name:" + d.Name);
                UIManager.Instance.ShowUIForms(DemoProject.ProConst.MainMenuUIForm);
            });
        });
    }

    IEnumerator testLog()
    {
        while(true)
        {
            Loger.Info("aaaaa");
            Loger.Debug("aaaaa");
            Loger.Warn("aaaaa");
            Loger.Error("aaaaa");

            yield return new WaitForSeconds(1f);
        }

    }

    private void LoadConfig()
    {
        TextAsset configInfo = Resources.Load<TextAsset>("game");
        JSONNode jsonNode = JsonUtils.LoadJson(configInfo.bytes);
        if (jsonNode != null)
        {
            this.gameObject.AddComponent<FileAppender>();
            this.gameObject.AddComponent<DebugLogAppender>();

            int push = jsonNode["pushLog"].AsInt;
            if (push == 1)
            {
                string host = jsonNode["logHost"].Value;
                var hs = host.Split(':');
                ClientSocketAppender socket = this.gameObject.AddComponent<ClientSocketAppender>();
                socket.OnConnect(hs[0], int.Parse(hs[1]));
            }
        }
    }

    void Update () {
		
	}
}
