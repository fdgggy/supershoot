using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class CampAssetPostprocessor 
{
    private static readonly string filePath = "Assets/ExcelData/AI.xls";
    private static readonly string assetFilePath = "Assets/ExcelAssets/Camp.asset";
    private static readonly string sheetName = "Camp";
    
    public static void OnPostprocessAllAssets ()
    {
        Camp data = (Camp)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(Camp));
        if (data == null) {
            data = ScriptableObject.CreateInstance<Camp> ();
            data.SheetName = filePath;
            data.WorksheetName = sheetName;
            AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
            //data.hideFlags = HideFlags.NotEditable;
        }
            
        //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<CampData>().ToArray();		

        //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
        //EditorUtility.SetDirty (obj);

        ExcelQuery query = new ExcelQuery(filePath, sheetName);
        if (query != null && query.IsValid())
        {
            data.dataArray = query.Deserialize<CampData>().ToArray();

            ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            EditorUtility.SetDirty (obj);
        }
    }
}
