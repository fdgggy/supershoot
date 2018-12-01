using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEditor;
using UnityEngine;

public static class SearchDependencies
{
    private static string[] _resOutputPath = {  "Assets/RreloadData/preload",
                                                "Assets/RreloadData/uicommon",
                                                 "Assets/EffecRes/",
                                                 "Assets/UIData/",  //自动依赖
                                                 "Assets/MapData/",
                                                 "Assets/RoleData/" //自动依赖
                                                 };

    private static string toSearchName = string.Empty;

    private static BuildLog buildLog = null;
    public static void Search(string fileName)
    {
        buildLog = new BuildLog();
        buildLog.buildFileName = "result.log";
        toSearchName = Path.GetFileName(fileName);
        for (int i = 0; i < _resOutputPath.Length; i++)
        {
            string dirName = FileUtils.AssetToABSPath(_resOutputPath[i]);
            if (!Directory.Exists(dirName))
            {
                continue;
            }
            SearchAll(dirName);

        }
        buildLog.SaveLog();
    }

    private static void SearchAll(string parentPath)
    {
        FileUtils fl = new FileUtils();
        string appDataPath = Application.dataPath + "/";

        fl.searchFileUnder(parentPath, ".prefab|.unity", (string fileName) =>
        {
            string absFile = fileName;
            fileName = fileName.Replace("\\", "/");
            fileName = "Assets/" + fileName.Replace(appDataPath, "");
            string[] dps = AssetDatabase.GetDependencies(fileName);
            for (int i = 0; i < dps.Length; i++)
            {
                if (dps[i].IndexOf(toSearchName) >= 0)
                {
                    buildLog.Logf("find in {0}", fileName);
                }
            }
        });
    }
}

