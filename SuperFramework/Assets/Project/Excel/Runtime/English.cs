using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

///
/// !!! Machine generated code !!!
///
/// A class which deriveds ScritableObject class so all its data 
/// can be serialized onto an asset data file.
/// 
[System.Serializable]
public class English : ScriptableObject 
{	
    [HideInInspector] [SerializeField] 
    public string SheetName = "";
    
    [HideInInspector] [SerializeField] 
    public string WorksheetName = "";
    
    // Note: initialize in OnEnable() not here.
    public EnglishData[] dataArray;
    
    private Dictionary<int, EnglishData> m_data = new Dictionary<int, EnglishData>();
    
      
    private void Init()
    {
        if (m_data.Count <= 0)
        {
            foreach(var row in dataArray)
            {
                if (m_data.ContainsKey(row.Id))
                {
                    Debug.Log("EnglishData has already contain the Id:" + row.Id);
                    return;
                }
                m_data.Add(row.Id, row);
            }
        }
    }

    public EnglishData QueryByID(int Id)
    {
        Init();
        
        if (m_data.ContainsKey(Id))
        {
            return m_data[Id];
        }

        return null;
    }

	public Dictionary<int, EnglishData> GetRows()
    {
        Init();
        return m_data;
    }
}
