using UnityEngine;
using System.Collections;
using UnityEditor;

public class PackageCheckerWin : EditorWindow
{


    static public void OpenPackageCheckWin()
    {
        PackageCheckerWin win = EditorWindow.GetWindow<PackageCheckerWin>(false, "PkgCheck", true);
        win.position = new Rect(Screen.width / 2, Screen.height / 2 - 200, 650, 670);
        win.Show();
    }



    private AssetBundle bundle;
    private bool isloop = false;
    void Update()
    {
        if (isloop == true)
        {
            if (www.progress == 1f)
            {

                isloop = false;

                bundle = www.assetBundle;
#if UNITY_5
                bundleList = bundle.LoadAllAssets();
#else
                bundleList = bundle.LoadAll();
#endif
                Repaint();
            }
            else
            {
                //Log.info("Loading progress: "+www.progress*100f+"%");
            }
        }
    }

    void OnDestroy()
    {
        if (bundle != null)
        {
            bundle.Unload(true);
            bundleList = null;
        }
    }
    //Rect rect;
    private Vector2 mScroll = Vector2.zero;
    private Object[] bundleList;
    private static string path;
    private WWW www;
    private string selectABName = string.Empty;
    void OnGUI()
    {

        bool hasSelect = false;
        if (GUILayout.Button("选择文件", GUILayout.Height(40f)))
        {
            path = EditorUtility.OpenFilePanel("打开AssetsBundle文件", path, "unity3d");
            if (path.Length != 0)
            {
                string tmpPath = path;
                int i = path.LastIndexOf("/");
                if (i != -1)
                {
                    selectABName = tmpPath.Substring(i + 1);
                }

                if (bundle != null)
                {
                    bundle.Unload(true);
                    bundleList = null;
                }
                www = new WWW("file:///" + path);
                isloop = true;
                hasSelect = true;
            }
        }
        if (!hasSelect && Selection.objects != null && Selection.objects.Length > 0)
        {
            object[] list = Selection.objects;
            if (list != null && list.Length == 1)
            {
                //当前文件夹目录
                string curPath = AssetDatabase.GetAssetPath((Object)list[0]);
                curPath = FileUtils.AssetToABSPath(curPath);
                curPath = curPath.Replace("\\", "/");
                if (curPath != path)
                {
                    if (curPath.IndexOf("unity3d") > 0 && curPath.IndexOf("meta") == -1)
                    {
                        path = curPath;
                        if (bundle != null)
                        {
                            bundle.Unload(true);
                            bundleList = null;
                        }

                        www = new WWW("file:///" + path);
                        isloop = true;
                        hasSelect = true;
                    }
                }

            }
        }


        Object[] objList = null;// = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if (bundleList != null)
        {
            objList = bundleList;
        }

        //往下12像素
        GUILayout.Space(12f);
        //设置背景为灰色
        GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        //开始横向
        GUILayout.BeginHorizontal();
        GUILayout.Space(3f);
        //GUI.changed = false;
        //使用一个toggle的样子模拟列表头
        if (objList != null && objList.Length > 0)
        {
            GUILayout.Toggle(true, "<b><size=11>AB:" + selectABName + "  FileList Count:" + objList.Length + "</size></b>", "dragtab", GUILayout.MinWidth(20f));
        }
        else
        {
            GUILayout.Toggle(true, "<b><size=11>Empty now</size></b>", "dragtab", GUILayout.MinWidth(20f));
        }
        GUILayout.Space(2f);
        GUILayout.EndHorizontal();

        GUI.backgroundColor = Color.white;
        GUILayout.BeginHorizontal();
        GUILayout.BeginVertical();
        EditorGUILayout.BeginHorizontal();

        //边框围起来
        GUI.backgroundColor = new Color(0.8f, 0.8f, 0.8f);
        GUILayout.BeginHorizontal("box", GUILayout.Height(400f), GUILayout.MaxWidth(1000f));
        GUI.backgroundColor = Color.white;
        mScroll = GUILayout.BeginScrollView(mScroll, GUILayout.Height(515f));



        bool doDelete = false;
        if (objList != null && objList.Length > 0)
        {
            for (int i = 0; i < objList.Length; i++)
            {
                if (objList[i] == null)
                {
                    continue;
                }
                GUILayout.Space(-1f);
                //开始一行凹陷造型的文字
                GUILayout.BeginHorizontal("AS TextArea", GUILayout.MinHeight(20f), GUILayout.MaxWidth(1000f));
                GUI.backgroundColor = Color.white;
                GUILayout.Label((i + 1).ToString(), GUILayout.Width(24f));
                GUILayout.Button(objList[i].name, "OL TextField", GUILayout.Height(20f), GUILayout.Width(300f));
                GUI.color = Color.green;
                GUILayout.Label(objList[i].GetType().Name, GUILayout.Width(146f));
                GUI.color = Color.white;
                //
                if (GUILayout.Button("Active", GUILayout.Width(60f)))
                {
                    Selection.activeObject = objList[i];
                    //objList[i] = null;
                    // doDelete = true;
                    break;
                }
                if (GUILayout.Button("Instantiate", GUILayout.Width(80f)))
                {
                    GameObject go = Instantiate(objList[i]) as GameObject;

                    Selection.activeObject = go;
                    //objList[i] = null;
                    // doDelete = true;
                    break;
                }
                GUILayout.EndHorizontal();
                //    } 
            }
        }
        //如果有删除，则刷新
        if (doDelete)
        {
            Selection.objects = objList;
            Repaint();
            return;
        }

        GUILayout.EndScrollView();

        GUILayout.EndHorizontal();
        //    GUILayout.Space(3f);
        GUILayout.EndHorizontal();

    }


    private void onAssetsBundleLoaded(AssetBundle pkg, Hashtable p)
    {
        //Log.info(pkg.mainAsset);
#if UNITY_5
        bundleList = pkg.LoadAllAssets();
#else
        bundleList = pkg.LoadAll();
#endif
    }





}