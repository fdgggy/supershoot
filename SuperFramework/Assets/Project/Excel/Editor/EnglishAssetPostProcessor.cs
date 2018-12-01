using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class EnglishAssetPostprocessor 
{
    private static readonly string filePath = "Assets/ExcelData/Language.xls";
    private static readonly string assetFilePath = "Assets/ExcelAssets/English.asset";
    private static readonly string sheetName = "English";
    
    public static void OnPostprocessAllAssets ()
    {
        English data = (English)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(English));
        if (data == null) {
            data = ScriptableObject.CreateInstance<English> ();
            data.SheetName = filePath;
            data.WorksheetName = sheetName;
            AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
            //data.hideFlags = HideFlags.NotEditable;
        }
            
        //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<EnglishData>().ToArray();		

        //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
        //EditorUtility.SetDirty (obj);

        ExcelQuery query = new ExcelQuery(filePath, sheetName);
        if (query != null && query.IsValid())
        {
            data.dataArray = query.Deserialize<EnglishData>().ToArray();

            ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            EditorUtility.SetDirty (obj);
        }
    }
}
