using System;
using System.Collections.Generic;

public enum LauguageType
{
    None = 0,
    Chinese,
    English,

    Num
}
public class LauguageManager
{
    private Dictionary<int, string> lauguageCache = new Dictionary<int, string>();
    private static LauguageManager instance;
    public static LauguageManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new LauguageManager();
            }
            return instance;
        }
    }
    private LauguageType lauguageType = LauguageType.English;
    private LauguageManager()
    {
        if (lauguageType == LauguageType.English)
        {
            English english = ExcelDataManager.Instance.GetExcel(ExcelType.English) as English;
            Dictionary<int, EnglishData> data = english.GetRows();
            foreach(KeyValuePair<int, EnglishData>kv in data)
            {
                lauguageCache.Add(kv.Value.Id, kv.Value.Name);
            }
        }
    }
    public string GetText(int id)
    {
        if (lauguageCache.ContainsKey(id))
        {
            return lauguageCache[id];
        }

        return string.Empty;
    }
}
