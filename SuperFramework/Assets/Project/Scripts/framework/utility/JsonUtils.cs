using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SimpleJSON;

public static class JsonUtils
{
    public static JSONNode LoadJson(byte[] bt)
    {
        if (bt == null) return null;
        string str = System.Text.Encoding.UTF8.GetString(bt);
        return JSON.Parse(str);
    }

    public static JSONNode LoadJson(string path)
    {
        byte[] bt = FileUtils.readFileBytes(path);
        if (bt == null) return null;
        string str = System.Text.Encoding.UTF8.GetString(bt);
        return JSON.Parse(str);
    }

    public static void SaveJson(JSONClass jsonObj,string fileName)
    {
        string str=jsonObj.ToString();
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(str);
        FileUtils.saveFileBytes(fileName, bytes);
    }
}

