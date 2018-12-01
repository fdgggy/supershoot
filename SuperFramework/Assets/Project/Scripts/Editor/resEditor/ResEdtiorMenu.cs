using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;
using SimpleJson;

class ResEdtiorMenu
{

    public static void Alert(string msg)
    {
        EditorUtility.DisplayDialog("notice", msg, "OK");
    }

    [MenuItem("Assets/ResMaker/查看资源依赖", false, 996)]
    public static void FindDependencies()
    {
        object[] list = Selection.GetFiltered(typeof(object), SelectionMode.Assets);
        if (list != null && list.Length == 1)
        {
            string filePath = AssetDatabase.GetAssetPath((Object)list[0]);
            string ext = Path.GetExtension(filePath);
            if (ext == ".prefab" || ext == ".unity" || ext == ".mat")
            {
                EditorUtility.DisplayProgressBar("执行中", "扫描中", 0.5f);
                SearchDependencies.Search(filePath);
                EditorUtility.ClearProgressBar();
            }
        }
    }
    /// <summary>
    /// 查看资源
    /// </summary>
    [MenuItem("Assets/ResMaker/AB包资源查看器", false, 997)]
    public static void LookRes()
    {
        PackageCheckerWin.OpenPackageCheckWin();
    }

    [MenuItem("ResEdtior/资源导出设置/★扫描预制件索引(DEBUG开发)", false, 10007)]
    public static void CreatePrefabIndex()      //只生成预制件加载路径，不生成资源依赖XML文件
    {
        ResPackerTool.ResAllForOutput(false);
        Alert("生成完毕");
    }

    [MenuItem("ResEdtior/资源导出设置/★生成预制件数据（导出资源前）", false, 10007)]    //记录在XML
    public static void CreatePrefabIndexAddtive()   //生成预制件加载路径，生成资源依赖XML文件，用于打AB包策略
    {
        ResPackerTool.ResAllForOutput(true);
        Alert("生成完毕");
    }

    [MenuItem("ResEdtior/资源导出设置/清除资源导出设置-谨慎使用", false, 10008)]
    public static void ClearResForAllOutput()
    {
        if (EditorUtility.DisplayDialog("清理资源", "确定需要清理资源吗？", "确定", "取消"))
        {
            ResPackerTool.ClearAllForOutput();
            Alert("清除资源导出设置");
        }
    }

    [MenuItem("ResEdtior/★★导出资源", false, 10015)]
    public static void ResExport()
    {
        PublishResWin.openResWin();
    }

    [MenuItem("ResEdtior/字体描边", false, 1111)]
    public static void ExportFontOutLine()
    {
        string rootPath = Application.dataPath + "/UIData";
        RecurseFolder(rootPath);

    }

    private static void RecurseFolder(string root)
    {
        string[] files = Directory.GetFiles(root);
        foreach (string file in files)
        {
            string tempFile = file;
            string exten = Path.GetExtension(tempFile);
            if (exten.EndsWith(".prefab", System.StringComparison.OrdinalIgnoreCase))
            {
                tempFile = tempFile.Replace("\\", "/");
                tempFile = "Assets/" + tempFile.Replace(Application.dataPath + "/", "");
                GameObject go = AssetDatabase.LoadAssetAtPath(tempFile, typeof(GameObject)) as GameObject;
                GameObject instance = PrefabUtility.InstantiatePrefab(go) as GameObject;
                RecurseGameObject(instance);
                PrefabUtility.ReplacePrefab(instance, go, ReplacePrefabOptions.ConnectToPrefab);
                GameObject.DestroyImmediate(instance);

                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh(ImportAssetOptions.ForceSynchronousImport);
            }
        }

        string[] folders = Directory.GetDirectories(root);
        foreach (string folder in folders)
        {
            RecurseFolder(folder);
        }
    }

    private static void RecurseGameObject(GameObject go)
    {
        //int count = go.transform.childCount;
        //if (count > 0)
        //{
        //    for (int i = 0; i < count; i++)
        //    {
        //        Transform tr = go.transform.GetChild(i);

        //        UILabel label = tr.gameObject.GetComponent<UILabel>();
        //        if (label != null)
        //        {
        //            label.effectStyle = UILabel.Effect.Outline;
        //        }

        //        RecurseGameObject(tr.gameObject);
        //    }
        //}
    }
}
