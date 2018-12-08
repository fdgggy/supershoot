using UnityEditor;
using UnityEngine;
using System.IO;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using SimpleJSON;
using System;

public class PublishResWin : EditorWindow
{
    static public void openResWin()
    {
        PublishResWin win = EditorWindow.GetWindow<PublishResWin>(false, "ResPacker", true);
        win.position = new Rect(Screen.width / 2, Screen.height / 2 - 200, 600, 300);
        win.Show();
    }

    //不包含类型信息
    private bool b_DisableWriteTypeTree = true;
    private bool b_DeterministicAssetBundle = true;
    private bool b_UncompressedAssetBundle = false;

#if UNITY_ANDROID
    private int platformID = 0;
#elif UNITY_IPHONE
private int platformID = 1;
#else
    private int platformID = 2;
#endif

    private bool b_needClear = false;
    private static JSONClass abRecord = null;
    private static List<string> files = new List<string>();
    void OnGUI()
    {
        EditorGUILayout.Space();
        //绘制一个粗体的标题栏  
        GUILayout.Label("target platform 目标平台", EditorStyles.boldLabel);

        string[] list = new string[] { "Android", "iOS Device", "Windows64", "Window", "Mac" };
        platformID = EditorGUILayout.Popup("Platform", platformID, list);

        b_needClear = EditorGUILayout.Toggle("是否清理旧资源", b_needClear);

        if (GUILayout.Button("资源打包", GUILayout.Height(40f)))
        {
            //string outputPath = BuildVer.GetOutputPath(EditorUserBuildSettings.activeBuildTarget);
            string outputPath = Application.streamingAssetsPath + "/";
            if (b_needClear)
            {
                FileUtil.DeleteFileOrDirectory(outputPath);
            }

            if (!Directory.Exists(outputPath))
            {
                Directory.CreateDirectory(outputPath);
            }

            //目前还是切换资源的版本来做
            if (EditorUserBuildSettings.activeBuildTarget != curBuildTarget)
            {
                EditorUtility.DisplayDialog("提示", "需要切换到对应的平台再打包", "确定");
                return;
            }

            ObjectData2XmlSerializer<AssetBundlesXML> xmlSer = new ObjectData2XmlSerializer<AssetBundlesXML>();
            AssetBundlesXML aassetBundleXML = xmlSer.Deserialize(Application.dataPath + "/AssetBundleXML.xml");
            AssetBundleBuild[] allBuilds = aassetBundleXML.GetAllBundles();

            BuildPipeline.BuildAssetBundles(outputPath, allBuilds, BuildAssetBundleOptions.DeterministicAssetBundle, EditorUserBuildSettings.activeBuildTarget);

            abRecord = new JSONClass();
            string newFilePath = outputPath + "abRecord.json";
            if (File.Exists(newFilePath)) File.Delete(newFilePath);

            files.Clear();
            Recursive(outputPath);

            for (int i = 0; i < files.Count; i++)
            {
                string file = files[i];
                string ext = Path.GetExtension(file);
                if (file.EndsWith(".meta")
                    || file.Contains(".DS_Store")
                    || file.Contains("launch.json")
                    || file.Contains("settings.json")
                    || file.Contains(".manifest")
                    ) continue;

                string md5 = Md5Utils.GetFileMd5(file);
                int size = FileUtils.GetFileSize(file);
                string value = file.Replace(outputPath, string.Empty);

                if (string.IsNullOrEmpty(value) == false)
                {
                    JSONClass jsonObj = new JSONClass();
                    jsonObj["abName"] = value;
                    jsonObj["md5"] = md5;
                    jsonObj["size"] = size.ToString();
                    abRecord.Add(value, jsonObj);
                }
            }

            string jsonData = abRecord.ToString();
            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
            FileUtils.SaveFileBytes(newFilePath, bytes);

            //string tarPath = BuildVer.GetOutputPath(EditorUserBuildSettings.activeBuildTarget);
            //_CopyDirectory(outputPath, tarPath);


            AssetDatabase.Refresh();
            this.Close();
        }

    }
    private void _CopyDirectory(string sourceDirName, string destDirName)
    {
        try
        {
            FileUtil.DeleteFileOrDirectory(destDirName);
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
                File.SetAttributes(destDirName, FileAttributes.Normal);
            }

            if (destDirName[destDirName.Length - 1] != Path.DirectorySeparatorChar)
                destDirName = destDirName + Path.DirectorySeparatorChar;

            string[] files = Directory.GetFiles(sourceDirName);
            foreach (string file in files)
            {
                if (IsContainMeta(file)) continue;
                if (file.LastIndexOf(".manifest") != -1) continue;

                if (File.Exists(destDirName + Path.GetFileName(file))) continue;
                File.Copy(file, destDirName + Path.GetFileName(file), true);
                File.SetAttributes(destDirName + Path.GetFileName(file), FileAttributes.Normal);

            }

            string[] dirs = Directory.GetDirectories(sourceDirName);
            foreach (string dir in dirs)
            {
                _CopyDirectory(dir, destDirName + Path.GetFileName(dir));
            }
        }
        catch (Exception ex)
        {
            UnityEngine.Debug.Log(ex.Message);
        }
    }
    static bool IsContainMeta(string file)
    {
        return file.LastIndexOf(".meta") != -1;
    }
    /// <summary>
    /// 遍历目录及其子目录
    /// </summary>
    static void Recursive(string path)
    {
        string[] names = Directory.GetFiles(path);
        string[] dirs = Directory.GetDirectories(path);
        foreach (string filename in names)
        {
            string ext = Path.GetExtension(filename);
            if (ext.Equals(".meta")) continue;
            files.Add(filename.Replace('\\', '/'));
        }
        foreach (string dir in dirs)
        {
            //paths.Add(dir.Replace('\\', '/'));
            Recursive(dir);
        }
    }
    private BuildTarget GetBuildTarget()
    {
        if (platformID == 2)
        {
            return BuildTarget.StandaloneWindows64;
        }
        if (platformID == 0)
        {
            return BuildTarget.Android;
        }
        if (platformID == 1)
        {
            return BuildTarget.iOS;
        }
        return BuildTarget.Android;
    }


    private BuildTarget curBuildTarget
    {
        get
        {
            BuildTarget platform;
            switch (platformID)
            {
                case 0:
                    platform = BuildTarget.Android;
                    break;
                case 1:
                    platform = BuildTarget.iOS;
                    break;
                case 2:
                    platform = BuildTarget.StandaloneWindows64;
                    break;
                case 3:
                    platform = BuildTarget.StandaloneWindows;
                    break;
                case 4:
                    platform = BuildTarget.StandaloneOSXIntel64;
                    break;
                default:
                    platform = BuildTarget.StandaloneWindows;
                    break;
            }
            return platform;
        }
    }
}



