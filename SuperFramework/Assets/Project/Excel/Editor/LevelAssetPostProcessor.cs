using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class LevelAssetPostprocessor 
{
    private static readonly string filePath = "Assets/ExcelData/Level.xls";
    private static readonly string assetFilePath = "Assets/ExcelAssets/Level.asset";
    private static readonly string sheetName = "Level";
    
    public static void OnPostprocessAllAssets ()
    {
        Level data = (Level)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(Level));
        if (data == null) {
            data = ScriptableObject.CreateInstance<Level> ();
            data.SheetName = filePath;
            data.WorksheetName = sheetName;
            AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
            //data.hideFlags = HideFlags.NotEditable;
        }
            
        //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<LevelData>().ToArray();		

        //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
        //EditorUtility.SetDirty (obj);

        ExcelQuery query = new ExcelQuery(filePath, sheetName);
        if (query != null && query.IsValid())
        {
            data.dataArray = query.Deserialize<LevelData>().ToArray();

            ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            EditorUtility.SetDirty (obj);
        }
    }
}
