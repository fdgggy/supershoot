using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using UnityEngine;

public class BuildLog
{
    private StringBuilder stringBuilder = new StringBuilder();

    public string buildFileName = "build_";

    public void Logf(string format, params object[] args)
    {
        stringBuilder.AppendFormat(format, args);
        stringBuilder.Append("\n");
        Debug.Log(string.Format(format, args));
    }

    public void SaveLog()
    {
        string dataTime = DateTime.Now.ToString("hh_mm_ss");
        string logPath = Application.dataPath + "/../" + buildFileName + dataTime + ".log";
        try
        {

            FileStream fs = new FileStream(logPath, FileMode.Append, FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, Encoding.Default);
            if (null == sw)
            {
                return;
            }
            sw.WriteLine(stringBuilder.ToString());
            sw.Flush();
            // 关闭读取流文件
            sw.Close();
            fs.Close();
        }
        catch (System.Exception ex)
        {
        }
    }
}

