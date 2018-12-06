using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ExcelType
{
	Ai,
	Enemy,
	English,
	Level,
	Map,
	Role,
	Weapon,
	Num
}

public class ExcelDataManager
{
    private static ExcelDataManager instance = null;
    public static ExcelDataManager Instance
    {
        get
        {
            if (instance == null)
            {
                return new ExcelDataManager();
            }

            return instance;
        }
    }

    private static Dictionary<ExcelType, ScriptableObject> m_excelData = new Dictionary<ExcelType, ScriptableObject>();

    public void Init(Util.VoidDelegate callBack)
    {
        int count = (int)ExcelType.Num;

        foreach (ExcelType ename in Enum.GetValues(typeof(ExcelType)))
        {
			if (ename != ExcelType.Num)
			{
			    ResManager.Instance.LoadScriptObject(ename.ToString(), (string assetName, object go) => {
					ScriptableObject data = go as ScriptableObject;
                
					ExcelType et = (ExcelType)Enum.Parse(typeof(ExcelType), assetName);
					m_excelData.Add(et, data);

					if (--count <= 0)
					{
						callBack();
					}
				});
			}
        }
    }

    public ScriptableObject GetExcel(ExcelType excel)
    {
        if (m_excelData.ContainsKey(excel))
        {
            return m_excelData[excel];
        }

        return null;
    }
}


