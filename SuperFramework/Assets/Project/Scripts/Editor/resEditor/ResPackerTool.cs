using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using System.IO;
using UnityEngine;
using SimpleJSON;

public class AssetBundleSetInfo
{
    public string rootDir;
    public string assetBundleName;
    public bool isCreate = false;
    public void updateAssetBundleName()
    {
        this.loadPrefabs(rootDir);
    }
    private string appDataPath
    {
        get { return Application.dataPath + "/"; }
    }
    //读取所有的prefab
    private void loadPrefabs(string parentPath)
    {
        FileUtils fl = new FileUtils();
        List<string> files = new List<string>();
        string findStr = ".prefab";
        int index = parentPath.IndexOf("MapData");
        int index2 = parentPath.IndexOf("asingle");
        if (index > 0 && index2 < 0)
        {
            findStr = ".unity";
        }
        //.prefab 预制件
        //.unity 是 场景文件
        //.asset 是 导出路径
        //.unity3d 是 打包的压缩包 .unity3d|.asset
        fl.searchFileUnder(parentPath, findStr, (string fileName) =>
        {

            string absFile = fileName;
            fileName = fileName.Replace("\\", "/");
            fileName = "Assets/" + fileName.Replace(appDataPath, "");
            //if (fileName.IndexOf("ATLAS")>=0)   //先不主动打图集...........注意：图集还是要单独打
            //{
            //    return;
            //}
            if (fileName.IndexOf("NoCheck") != -1)
            {
                return;
            }

            string ext = Path.GetExtension(fileName);
            string fName = Path.GetFileNameWithoutExtension(fileName);

            string abName = null;
            string md5Str = Md5Utils.GetFileMd5(absFile);
            EditorUtility.DisplayProgressBar("载入对象", fileName, 0.2f);
            if (ext == ".unity")  //场景
            {
                //字符串md5
                abName = (fName + "_scene.unity3d").ToLower();
                if (isCreate)
                {
                    string oldAbName = null;
                    ResPackerTool.resJSON.TryGetValue(fileName, out oldAbName);

                    //资源的依赖关系
                    if (ResPackerTool.resJSON != null && string.IsNullOrEmpty(oldAbName) && fileName != null)
                    {
                        AssetBundleBuild assetBuild = new AssetBundleBuild();
                        assetBuild.assetBundleName = abName;
                        string[] depends = { fileName };
                        ResPackerTool.resJSON.Add(fileName, abName);

                        assetBuild.assetNames = depends;
                        ResPackerTool.assetBundleXML.assetBundleBuilds.Add(assetBuild);

                        //场景资源单独打包
                        //string sceneResAbName = (fName + "_sceneRes.unity3d").ToLower();
                        //AssetBundleBuild assetBuildRes = new AssetBundleBuild();
                        //assetBuildRes.assetBundleName = sceneResAbName;
                        //string[] assetRes = createDependencieScene(depends, sceneResAbName);
                        //assetBuildRes.assetNames = assetRes;
                        //ResPackerTool.assetBundleXML.assetBundleBuilds.Add(assetBuildRes);
                    }
                }
            }
            else
            {
                //字符串md5
                abName = (fName + ".unity3d").ToLower();
                if (isCreate)
                {
                    string oldAbName = null;
                    ResPackerTool.resJSON.TryGetValue(fileName, out oldAbName);

                    //资源的依赖关系
                    if (ResPackerTool.resJSON != null && string.IsNullOrEmpty(oldAbName) && fileName != null)
                    {
                        string[] depends = { fileName };
                        AssetBundleBuild assetBuild = new AssetBundleBuild();
                        assetBuild.assetBundleName = abName;
                        ResPackerTool.resJSON.Add(fileName, abName);
                        assetBuild.assetNames = getDependencies(depends, abName);
                        ResPackerTool.assetBundleXML.assetBundleBuilds.Add(assetBuild);
                    }
                    else
                    {
                        ResPackerTool.buildLog.Logf("[error prefab name:]:{0},oldAbName:{1} ", fileName, oldAbName);
                    }
                }
            }

            //建立一个预制件的对应关系资源表
            if (ResPackerTool.prefabJSON != null && abName != null)
            {
                JSONClass jsonObj = new JSONClass();
                jsonObj["abName"] = abName;
                jsonObj["md5"] = md5Str;
                jsonObj["path"] = fileName;
                //DebugLog.Log(fName + "==>" + abName);
                ResPackerTool.prefabJSON[fName] = jsonObj;
            }
            else
            {
                Debug.Log("the save prefabName:" + fName);
            }
        });
    }

    //过滤的后缀
    private string[] __exCludeName = { ".cs", ".meta", ".prefab", ".unity" };
    private bool IsExcludeFile(string fileName)
    {
        string ext = Path.GetExtension(fileName);
        for (int i = 0; i < __exCludeName.Length; i++)
        {
            if (ext.ToLower() == __exCludeName[i])
            {
                return true;
            }
        }
        return false;
    }
    private bool IsResResources(string fileName)
    {
        string ext = Path.GetExtension(fileName);
        if (fileName.IndexOf("Assets/Resources") >= 0)
        {
            return true;
        }
        return false;
    }
    private bool IsShader(string fileName)
    {
        string ext = Path.GetExtension(fileName);
        if (ext.ToLower() == ".shader")
        {
            return true;
        }
        return false;
    }

    private bool IsPrefab(string fileName)
    {
        string ext = Path.GetExtension(fileName);
        if (ext.ToLower() == ".prefab")
        {
            return true;
        }
        return false;

    }

    private void AddInShaderAsset(string fileName)
    {
        if (fileName.IndexOf("Resources") == -1)
        {
            //shader添加进去
            ResPackerTool.shaderAssetList.Add(fileName);
        }
    }

    private bool IsPreload(string fileName)
    {
        if (fileName.IndexOf("RreloadData") >= 0 || fileName.EndsWith(".shader") || fileName.IndexOf("Resources") >= 0)
        {
            return true;
        }
        return false;
    }


    //单独把公用的资源打成AB包
    private void AddToSingleAssetBundle(string fileName, string oldAbName)
    {
        string fname = Path.GetFileName(fileName);
        string abName = (fname /*+ "_" + Md5Utils.GetMd5(fileName).Substring(0, 8)*/ + ".unity3d").ToLower();
        if (abName != oldAbName)
        {
            //删除老的ab包的记录
            AssetBundleBuild build = ResPackerTool.assetBundleXML.GetAssetBundle(oldAbName);
            if (build.assetBundleName == oldAbName)
            {
                List<string> olist = build.assetNames.ToList<string>();
                for (int i = olist.Count - 1; i >= 0; i--)
                {
                    if (olist[i] == fileName)
                    {
                        olist.RemoveAt(i);
                        ResPackerTool.buildLog.Logf("[remove res fileName]:{0} from {1}", fileName, oldAbName);
                        break;
                    }
                }
                if (olist.Count == 0)
                {
                    ResPackerTool.buildLog.Logf("[Remove AssetBundle]:{0}", oldAbName);
                    ResPackerTool.assetBundleXML.RemoveAssetBundle(oldAbName);
                }
                else
                {
                    build.assetNames = olist.ToArray<string>();
                    ResPackerTool.buildLog.Logf("[AddToSingleBundle]:{0},from oldAbName:{1} to newAbName:{2}", fileName, oldAbName, abName);
                    //刷新
                    ResPackerTool.assetBundleXML.SetAssetBundle(oldAbName, build);
                }

            }
            //生成新的包
            AssetBundleBuild newBuild = new AssetBundleBuild();
            newBuild.assetBundleName = abName;
            string[] res = { fileName };
            newBuild.assetNames = res;
            //更新缓存
            ResPackerTool.resJSON.resDic[fileName] = abName;
            ResPackerTool.assetBundleXML.assetBundleBuilds.Add(newBuild);
        }
    }

    //场景的依赖文件
    public string[] createDependencieScene(string[] abPaths, string curAbName)
    {
        List<string> result = new List<string>();
        string[] dps = AssetDatabase.GetDependencies(abPaths);
        foreach (string fileName in dps)
        {
            if (IsExcludeFile(fileName))
            {
                continue;
            }
            if (IsResResources(fileName))
            {
                continue;
            }
            string oldAbName = null;
            ResPackerTool.resJSON.TryGetValue(fileName, out oldAbName);


            //资源的依赖关系
            if (string.IsNullOrEmpty(oldAbName) && fileName != null)
            {

                if (IsShader(fileName))
                {

                    ResPackerTool.resJSON.Add(fileName, curAbName);
                    AddInShaderAsset(fileName);
                }
                else
                {
                    ResPackerTool.resJSON.Add(fileName, curAbName);
                    result.Add(fileName);
                }
            }
            else
            {
                //不是预加载的图，就直接打包进去
                if (!IsPreload(fileName))
                {
                    if (!string.IsNullOrEmpty(oldAbName))
                    {

                        this.AddToSingleAssetBundle(fileName, oldAbName);
                        continue;
                    }
                }
            }
        }
        return result.ToArray();
    }

    public string[] getDependencies(string[] abPaths, string curAbName)
    {
        List<string> result = new List<string>();
        //把自己加入
        result.Add(abPaths[0]);
        string[] dps = AssetDatabase.GetDependencies(abPaths);
        foreach (string fileName in dps)
        {
            if (IsExcludeFile(fileName))
            {
                continue;
            }
            if (IsResResources(fileName))
            {
                continue;
            }
            string oldAbName = null;
            ResPackerTool.resJSON.TryGetValue(fileName, out oldAbName);


            //资源的依赖关系
            if (string.IsNullOrEmpty(oldAbName) && fileName != null)
            {

                if (IsShader(fileName))
                {

                    ResPackerTool.resJSON.Add(fileName, curAbName);
                    AddInShaderAsset(fileName);
                }
                else
                {

                    ResPackerTool.resJSON.Add(fileName, curAbName);
                    result.Add(fileName);
                }
            }
            else
            {
                //不是预加载的图，就直接打包进去
                if (!IsPreload(fileName))
                {

                    if (!string.IsNullOrEmpty(oldAbName))
                    {

                        this.AddToSingleAssetBundle(fileName, oldAbName);
                        continue;
                    }
                }
            }
        }
        return result.ToArray();
    }
}

public class ResPackerTool
{
    private static string[] _resOutputPath = {
                                                //"Assets/RreloadData/preload",
                                                //"Assets/RreloadData/uicommon",
                                                // "Assets/EffecRes/",
                                                "Assets/ResData/MapData/Scenes",
                                                //"Assets/MapData/asingle/",  //场景修改为单个场景打包，不做依赖检测,这个单独打
                                                 "Assets/ResData/UIData/",  //自动依赖
                                                 "Assets/ResData/RoleData/prefab/",
                                             };

    //记录prefab对应的unity3d
    public static JSONClass prefabJSON = null;

    public static ResIncludeData resJSON = null;

    public static AssetBundlesXML assetBundleXML = null;


    public static List<string> shaderAssetList = null;


    public static BuildLog buildLog = new BuildLog();
    //全部资源导出
    /// <param name="isCreate"> true 的时候，设定资源在XML，false的时候，只扫描预制件</param>
    /// <returns></returns>
    public static void ResAllForOutput(bool isCreate)
    {
        buildLog = new BuildLog();
        buildLog.buildFileName = "ResAllForOutput";
        prefabJSON = new JSONClass();   //预制件的存储数据

        resJSON = new ResIncludeData();

        assetBundleXML = new AssetBundlesXML();

        shaderAssetList = new List<string>();
        //要做自动依赖
        for (int i = 0; i < _resOutputPath.Length; i++)
        {

            string dirName = FileUtils.AssetToABSPath(_resOutputPath[i]);
            if (!Directory.Exists(dirName))
            {
                continue;
            }
            if (dirName.IndexOf("RreloadData") >= 0
                )
            {
                ResPackerTool.ResAssetBundleOutput(dirName, isCreate);
            }
            else
            {
                ResPackerTool.ResAssetBundleOutput(dirName, isCreate);
            }

        }

        if (isCreate)
        {
            //assetBundleXML.shaderBundle.assetBundleName = "AllShaderAsset.unity3d";
            //assetBundleXML.shaderBundle.assetNames = shaderAssetList.ToArray<string>();

            //SoundBundleForOutput(assetBundleXML.soundBundles);
        }
        ScriptObjectForOutPut(assetBundleXML.soundBundles);

        string jsonData = prefabJSON.ToString();
        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(jsonData);
        FileUtils.saveFileBytes(Application.dataPath + "/Resources/" + "assetInfo.json", bytes);
        if (isCreate)
        {
            ObjectData2XmlSerializer<AssetBundlesXML> xmlSer = new ObjectData2XmlSerializer<AssetBundlesXML>();
            xmlSer.Serialize(assetBundleXML, Application.dataPath + "/assetBundleXML.xml");
        }

        resJSON = null;

        EditorUtility.ClearProgressBar();
        //刷新缓存数据
        //ResManager.getInstance().loadPrefabJson(Application.dataPath + "/../lua/" + "prefab.json");
        AssetDatabase.RemoveUnusedAssetBundleNames();
        buildLog.SaveLog();
        buildLog = null;
        AssetDatabase.Refresh();
    }

    //清理导出
    public static void ClearAllForOutput()
    {
        string[] bundleNames = AssetDatabase.GetAllAssetBundleNames();
        for (int i = 0; i < bundleNames.Length; i++)
        {
            ClearBundle(bundleNames[i]);
        }
        AssetDatabase.RemoveUnusedAssetBundleNames();
        EditorUtility.ClearProgressBar();
    }



    private static void ClearBundle(string bundleName)
    {
        string[] resList = AssetDatabase.GetAssetPathsFromAssetBundle(bundleName);
        for (int j = 0; j < resList.Length; j++)
        {
            float percent = j / resList.Length;
            EditorUtility.DisplayProgressBar("清理中", resList[j], percent);

            AssetImporter importer = AssetImporter.GetAtPath(resList[j]);
            importer.assetBundleName = null;
        }
    }

    //导出资源
    public static void ResAssetBundleOutput(string path, bool isAddtive = false)
    {
        if (!string.IsNullOrEmpty(path))
        {
            AssetBundleSetInfo abinfo = new AssetBundleSetInfo();
            abinfo.isCreate = isAddtive;
            //abinfo.isSetDependencies = isSetDependencies;
            abinfo.rootDir = path;
            abinfo.updateAssetBundleName();
        }
    }

    private static string[] _soundResOutput = { "Assets/Sound/Battle", "Assets/Sound/Story", "Assets/Sound/UI" };
    private static string _soundResBGM = "Assets/Sound/BGM";
    public static void SoundBundleForOutput(List<AssetBundleBuild> assetBundleList)
    {
        //正常音乐的打包配置
        for (int i = 0; i < _soundResOutput.Length; i++)
        {
            string dirName = FileUtils.AssetToABSPath(_soundResOutput[i]);

            float percent = i / _soundResOutput.Length;
            EditorUtility.DisplayProgressBar("打包中", _soundResOutput[i], percent);

            AssetBundleBuild abSet = new AssetBundleBuild();
            string abName = null;
            List<string> assetList = null;
            ResPackerTool.ResSoundGroupBundle(dirName, out abName, out assetList);
            abSet.assetBundleName = abName;
            abSet.assetNames = assetList.ToArray<string>();
            assetBundleList.Add(abSet);
        }

        string abName1 = string.Empty;
        //背景音乐的打包配置
        FileUtils fl = new FileUtils();
        fl.searchFileUnder(FileUtils.AssetToABSPath(_soundResBGM), ".ogg|.mp3|.wav", (string fileName) =>
        {
            fileName = fileName.Replace("\\", "/");
            fileName = "Assets/" + fileName.Replace(Application.dataPath + "/", "");
            //AssetImporter importer = AssetImporter.GetAtPath(fileName);
            abName1 = FileUtils.getBaseName(fileName) + "_bgm.unity3d";
            AssetBundleBuild abSet1 = new AssetBundleBuild();
            abSet1.assetBundleName = abName1;
            string[] assets = new string[1];
            assets[0] = fileName;
            abSet1.assetNames = assets;
            assetBundleList.Add(abSet1);
        });

    }

    //声音组
    private static void ResSoundGroupBundle(string path, out string abName, out List<string> assetList)
    {
        abName = FileUtils.getBaseName(path) + "_audio.unity3d";
        List<string> tassetList = new List<string>();
        FileUtils fl = new FileUtils();
        fl.searchFileUnder(path, ".ogg|.mp3|.wav", (string fileName) =>
        {
            fileName = fileName.Replace("\\", "/");
            fileName = "Assets/" + fileName.Replace(Application.dataPath + "/", "");
            tassetList.Add(fileName);
        });
        assetList = tassetList;
    }

    private static string scriptObjectPath = "Assets/ExcelAssets";
    private static void ScriptObjectForOutPut(List<AssetBundleBuild> assetBundleList)
    {
        string abName = string.Empty;
        FileUtils fl = new FileUtils();
        fl.searchFileUnder(FileUtils.AssetToABSPath(scriptObjectPath), ".asset", (string fileName) =>
        {
            string md5Str = Md5Utils.GetFileMd5(fileName);
            fileName = fileName.Replace("\\", "/");
            fileName = "Assets/" + fileName.Replace(Application.dataPath + "/", "");
            abName = Path.GetFileNameWithoutExtension(fileName) + ".unity3d";
            AssetBundleBuild abSet = new AssetBundleBuild();
            abSet.assetBundleName = abName;
            string[] assets = new string[1];
            assets[0] = fileName;
            abSet.assetNames = assets;
            assetBundleList.Add(abSet);

            //建立一个预制件的对应关系资源表
            if (ResPackerTool.prefabJSON != null)
            {
                JSONClass jsonObj = new JSONClass();
                jsonObj["abName"] = abName;
                jsonObj["md5"] = md5Str;
                jsonObj["path"] = fileName;
                ResPackerTool.prefabJSON[Path.GetFileNameWithoutExtension(fileName)] = jsonObj;
            }
            else
            {
                Loger.Error("ResPackerTool.prefabJSON is null");
            }
        });
    }
}

