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
public class Enemy : ScriptableObject 
{	
    [HideInInspector] [SerializeField] 
    public string SheetName = "";
    
    [HideInInspector] [SerializeField] 
    public string WorksheetName = "";
    
    // Note: initialize in OnEnable() not here.
    public EnemyData[] dataArray;
    
    private Dictionary<int, EnemyData> m_data = new Dictionary<int, EnemyData>();
    
      
    private void Init()
    {
        if (m_data.Count <= 0)
        {
            foreach(var row in dataArray)
            {
                if (m_data.ContainsKey(row.Id))
                {
                    Debug.Log("EnemyData has already contain the Id:" + row.Id);
                    return;
                }
                m_data.Add(row.Id, row);
            }
        }
    }

    public EnemyData QueryByID(int Id)
    {
        Init();
        
        if (m_data.ContainsKey(Id))
        {
            return m_data[Id];
        }

        return null;
    }

	public Dictionary<int, EnemyData> GetRows()
    {
        Init();
        return m_data;
    }
}
