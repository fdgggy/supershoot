using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

///
/// !!! Machine generated code !!!
/// !!! DO NOT CHANGE Tabs to Spaces !!!
/// 
[System.Serializable]
public class RecordExcelInfo
{
	private List<string> ExcelClassNames = new List<string>() 
    {
		"EnglishAssetPostprocessor",

    };
    
	public void Run()
    {
        foreach(var name in ExcelClassNames)
        {
            Type type = Type.GetType(name);
            System.Object obj = System.Activator.CreateInstance(type);
            MethodInfo method = type.GetMethod("OnPostprocessAllAssets");
            method.Invoke(obj, null);
        }
    }
}