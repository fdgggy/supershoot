using SimpleJSON;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEngine;

public class BuildPublish
{
    [MenuItem("publish/GooglePlay", false, 10022)]
    public static void ForGoogleSdk()
    {
        BuildTarget platform = EditorUserBuildSettings.activeBuildTarget;

        switch (platform)
        {
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneWindows:
                string symbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbolStr + ";GooglePlay");
                break;
            case BuildTarget.Android:
                string symbolStrA = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbolStrA + ";GooglePlay");
                break;
            case BuildTarget.iOS:
                string symbolStrI = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, symbolStrI + ";GooglePlay");
                break;
        }
        setcfg("GooglePlay");
    }

    [MenuItem("publish/AnySDK", false, 10021)]
    public static void ForAnySdk()
    {
        BuildTarget platform = EditorUserBuildSettings.activeBuildTarget;

        switch (platform)
        {
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneWindows:
                string symbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbolStr + ";AnySDK");
                break;
            case BuildTarget.Android:
                string symbolStrA = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbolStrA + ";AnySDK");
                break;
            case BuildTarget.iOS:
                string symbolStrI = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, symbolStrI + ";AnySDK");
                break;
        }
        setcfg("AnySDK");
    }

    [MenuItem("publish/猕猴桃SDK", false, 10020)]
    public static void ForMHT()
    {
        BuildTarget platform = EditorUserBuildSettings.activeBuildTarget;

        switch (platform)
        {
            case BuildTarget.StandaloneWindows64:
            case BuildTarget.StandaloneWindows:
                string symbolStr = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, symbolStr + ";MHTSDK");
                break;
            case BuildTarget.Android:
                string symbolStrA = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, symbolStrA + ";MHTSDK");
                break;
            case BuildTarget.iOS:
                string symbolStrI = PlayerSettings.GetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS);
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, symbolStrI + ";MHTSDK");
                break;
        }
        setcfg("MHTSDK");
    }

    [MenuItem("publish/设置为(Debug)开发模式", false, 10012)]
    public static void ForDebug()
    {
        BuildTarget platform = EditorUserBuildSettings.activeBuildTarget;
        switch (platform)
        {
            case BuildTarget.StandaloneWindows64:
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "_DEBUG;");
                break;
            case BuildTarget.StandaloneWindows:
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "_DEBUG;");
                break;
            case BuildTarget.Android:
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "_DEBUG;");
                break;
            case BuildTarget.iOS:
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "_DEBUG;");
                break;
        }
        setcfg("Default");
    }

    [MenuItem("publish/设置为(Release)发布模式", false, 10013)]
    public static void ForRelease()
    {
        BuildTarget p = EditorUserBuildSettings.activeBuildTarget;
        switch (p)
        {
            case BuildTarget.StandaloneWindows64:
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "_RELEASE;");
                break;
            case BuildTarget.StandaloneWindows:
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Standalone, "_RELEASE;");
                break;
            case BuildTarget.Android:
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.Android, "_RELEASE;");
                break;
            case BuildTarget.iOS:
                PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "_RELEASE;");
                break;
        }
        setcfg("Default");
    }

    private static void setcfg(string plamform)
    {
        //string path = Application.streamingAssetsPath + "/game.json";
        //JSONNode jsonNode = JsonUtils.LoadJson(path);
        //jsonNode["plamform"].Value = plamform;

        //if (File.Exists(path)) File.Delete(path);

        //byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonNode.ToString());
        //FileUtils.saveFileBytes(path, bytes);
    }

    //[MenuItem("publish/压缩资源", false, 10009)]
    //public static void CompressRes()
    //{
    //    if (EditorUtility.DisplayDialog("notice", "是否要压缩资源?", "确定", "取消"))
    //    {
    //        BuildVer bv = new BuildVer();
    //        bv.CompressRes();
    //    }
    //}

    //[MenuItem("publish/安卓发布", false, 10014)]
    //public static void BuildAndroid()
    //{
    //    if (EditorUtility.DisplayDialog("notice", "是否要发布安卓版本?", "确定", "取消"))
    //    {
    //        BuildVer bv = new BuildVer();
    //        bv.BuildAndroid();
    //    }
    //}

    //[MenuItem("publish/IOS发布,导出XCODE", false, 10018)]
    //public static void BuildIOS()
    //{
    //    if (EditorUtility.DisplayDialog("notice", "是否要发布IOS?", "确定", "取消"))
    //    {
    //        BuildVer bv = new BuildVer();
    //        bv.BuildIOS();
    //    }
    //}
}

