using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


[Serializable]
public class ResIncludeData
{
    public Dictionary<string, string> resDic = new Dictionary<string, string>();


    public void TryGetValue(string key, out string abName)
    {
        resDic.TryGetValue(key, out abName);
    }

    public void Add(string key, string abName)
    {
        resDic.Add(key, abName);
    }



}

