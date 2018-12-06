using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class MapAssetPostprocessor 
{
    private static readonly string filePath = "Assets/ExcelData/Map.xls";
    private static readonly string assetFilePath = "Assets/ExcelAssets/Map.asset";
    private static readonly string sheetName = "Map";
    
    public static void OnPostprocessAllAssets ()
    {
        Map data = (Map)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(Map));
        if (data == null) {
            data = ScriptableObject.CreateInstance<Map> ();
            data.SheetName = filePath;
            data.WorksheetName = sheetName;
            AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
            //data.hideFlags = HideFlags.NotEditable;
        }
            
        //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<MapData>().ToArray();		

        //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
        //EditorUtility.SetDirty (obj);

        ExcelQuery query = new ExcelQuery(filePath, sheetName);
        if (query != null && query.IsValid())
        {
            data.dataArray = query.Deserialize<MapData>().ToArray();

            ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            EditorUtility.SetDirty (obj);
        }
    }
}
