using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using UnityEngine;
using System.Text.RegularExpressions;

public class FileUtils
{
    private bool EqualExt(string ext)
    {
        int length = _extList.Length;
        for (int i = 0; i < length; i++)
        {
            if (_extList[i].Equals(ext))
            {
                return true;
            }
        }
        return false;
    }

    public delegate void FileCallBack(string file);
    public delegate void DirCallBack(string dir);
    public int fileDeep = -1;
    public int limitDeep = 9999;
    private string[] _extList;
    public void searchFileUnder(string rootDir, string fType = "*", FileCallBack fileCall = null, DirCallBack dirCall = null)
    {
        fileDeep++;
        string[] dirs = Directory.GetDirectories(rootDir);
        string[] files = Directory.GetFiles(rootDir);
        _extList = fType.Split(new char[] { '|' });
        //string dir = Path.GetDirectoryName(rootDir);
        foreach (string f in files)
        {
            if (EqualExt("*"))
            {
                if (fileCall != null)
                {
                    fileCall(f);
                }
            }
            else
            {
                string ext = Path.GetExtension(f).ToLower();
                if (EqualExt(ext))
                {
                    if (fileCall != null)
                    {
                        fileCall(f);
                    }
                }
            }

        }

        if (this.fileDeep <= this.limitDeep)
        {
            foreach (string dir in dirs)
            {
                if (dirCall != null)
                {
                    dirCall(dir);
                }
                this.searchFileUnder(dir, fType, fileCall, dirCall);
            }
        }

    }

    public static void existPathDirectory(string path)
    {
        string directory = Path.GetDirectoryName(path);
        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }
    }

    public static string getFileName(string filePath)
    {
        return Path.GetFileName(filePath);
    }

    /// <summary>
    /// 获取文件的名字包含路径
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string getBaseNameIncludePath(string filePath)
    {
        int endPos = filePath.LastIndexOf(".");
        if (endPos == -1)
        {
            endPos = filePath.Length;
        }
        return filePath.Substring(0, endPos);
    }

    /// <summary>
    ///  获取的是文件的名字，不包含后缀
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string getBaseName(string filePath)
    {
        int endPos = filePath.LastIndexOf(".");
        if (endPos < 0)
        {
            endPos = filePath.Length;
        }
        int beginPos = filePath.LastIndexOf("/");
        if (beginPos < 0)
        {
            beginPos = filePath.LastIndexOf("\\");
        }
        //纯文件名
        return filePath.Substring(beginPos + 1, endPos - beginPos - 1);
    }

    public static string getPathToPrefabName(string filePath)
    {
        string inPathName = getBaseNameIncludePath(filePath);
        inPathName = inPathName.Replace("/", "&");
        return inPathName;
    }

    /// <summary>
    ///  获取文件的路径名，不包含文件名字,也可以获取当前文件名的父节点的名字
    /// </summary>
    /// <param name="filePath"></param>
    /// <returns></returns>
    public static string getParentDir(string filePath)
    {
        int endPos = filePath.LastIndexOf(".");
        if (endPos < 0)
        {
            endPos = filePath.Length - 1;
        }
        int beginPos = filePath.LastIndexOf("/");
        if (beginPos < 0)
        {
            beginPos = filePath.LastIndexOf("\\");
        }

        return filePath.Substring(0, beginPos);
    }

    //获取文件夹名字
    public static string getBaseDirName(string filePath)
    {
        return getBaseName(getParentDir(filePath));
    }

    public static string AssetToABSPath(string assetPath)
    {
        string root = Application.dataPath + "/";
        Regex r = new Regex(@"^Assets/");
        assetPath = r.Replace(assetPath, "");
        assetPath = root + assetPath;
        assetPath = assetPath.Replace("\\", "/");
        return assetPath;
    }



    public static string ABSPathToAsset(string absPath)
    {
        string root = Application.dataPath + "/";
        absPath = absPath.Replace("\\", "/");
        absPath = "Assets/" + absPath.Replace(root, "");
        return absPath;
    }


    public static string readFileUTF8(string path)
    {
        if (File.Exists(path))
        {
            string str = File.ReadAllText(path, System.Text.Encoding.UTF8);
            return str;
        }

        return "";
    }

    //加载加密的文件
    public static string readEncodeFileUTF8(string path)
    {
        byte[] bt = readFileBytes(path);
        if (bt == null)
        {
            return string.Empty;
        }
        else
        {
            string reStr = System.Text.Encoding.UTF8.GetString(bt);
            return reStr;
        }
    }

    public static byte[] readFileBytes(string path)
    {
        if (File.Exists(path))
        {
            byte[] re = File.ReadAllBytes(path);
            return re;
        }

        return null;
    }

    public static void saveFileBytes(string fileName, byte[] data)
    {
        try
        {
            File.WriteAllBytes(fileName, data);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }

    public static void saveFileUTF8(string fileName, string data)
    {
        try
        {
            StreamWriter sw = new StreamWriter(fileName, false, Encoding.UTF8);
            sw.Write(data);
            sw.Close();
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }

    }
    /// <summary>
    /// 暂时不弄加密
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string ReadEncodeFileUTF8(string path)
    {
        byte[] bt = ReadFileBytes(path);

        string reStr = string.Empty;
        if (bt != null)
        {
            reStr = System.Text.Encoding.UTF8.GetString(bt);
        }

        return reStr;
    }
    public static string ReadEncodeFileUTF8(byte[] bt)
    {
        return System.Text.Encoding.UTF8.GetString(bt);
    }
    public static byte[] ReadFileBytes(string path)
    {
        if (File.Exists(path))
        {
            byte[] re = File.ReadAllBytes(path);
            return re;
        }

        return null;
    }

    public static int GetFileSize(string path)
    {
        byte[] bt = ReadFileBytes(path);
        return bt.Length;
    }

    public static void SaveFileBytes(string fileName, byte[] data)
    {
        try
        {
            File.WriteAllBytes(fileName, data);
        }
        catch (Exception e)
        {
            Debug.Log(e.Message);
        }
    }
}

