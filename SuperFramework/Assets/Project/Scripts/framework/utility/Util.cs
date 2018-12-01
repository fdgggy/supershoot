using UnityEngine;

public static class Util
{
    public delegate void VoidDelegate();
    public delegate void StringDelegate(string result);
    public delegate void CommonDelegate<T>(T t);

    public static string PersistencePath
    {
        get { return Application.persistentDataPath; }
    }

    public static string ABInterLoadWWWPath
    {
        get
        {
            if (Application.platform == RuntimePlatform.Android)
            {
                return Application.streamingAssetsPath;
            }

            return "file:///" + Application.streamingAssetsPath;
        }
    }

    public static string ABOuterLoadNOWWWPath
    {
        get
        {
            return PersistencePath;
        }
    }
}
