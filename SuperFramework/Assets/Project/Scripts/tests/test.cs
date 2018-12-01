using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;
using NLog.Unity;

public class test : MonoBehaviour {

	void Start () {
        StartCoroutine(LoadGameConfig());
        //Loger.Debug("aaaaaaaaaaaaaaa:{0}", 12);
        //Loger.Info("bbbbbbbbbbbbbbbbbbbb:{0}", 12);
        //a();
    }
    void a()
    {
        b();
    }
    void b()
    {
        Loger.Info("eeeeeeeeeeee {0}:{1}", 1, 2);
        Loger.Debug("eeeeeeeeeeee");
        Loger.Warn("eeeeeeeeeeee");
        Loger.Error("eeeeeeeeeeee");
    }


    private IEnumerator LoadGameConfig()
    {
        string url = string.Empty;
        if (Application.platform == RuntimePlatform.Android)
        {
            url = Application.streamingAssetsPath;
        }
        else
        {
            url = "file:///" + Application.streamingAssetsPath;
        }
        WWW www = new WWW(url +  "/game.json");
        yield return www;   

        if (www.isDone)
        {
            JSONNode jsonNode = JsonUtils.LoadJson(www.bytes);
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

                //Loger.Debug("aaaaaaaaaaaaaaa:{0}", 12);
                //Loger.Info("bbbbbbbbbbbbbbbbbbbb:{0}", 12);
                while(true)
                {
                    //Loger.Warn("aaaaaaaaaaaaaaa:{0}", 12);
                    a();
                    yield return new WaitForSeconds(1f);
                }
            }
        }

        yield return new WaitForEndOfFrame();
    }
}
