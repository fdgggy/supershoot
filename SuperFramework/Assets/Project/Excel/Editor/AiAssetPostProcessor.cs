using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class AiAssetPostprocessor 
{
    private static readonly string filePath = "Assets/ExcelData/AI.xls";
    private static readonly string assetFilePath = "Assets/ExcelAssets/Ai.asset";
    private static readonly string sheetName = "Ai";
    
    public static void OnPostprocessAllAssets ()
    {
        Ai data = (Ai)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(Ai));
        if (data == null) {
            data = ScriptableObject.CreateInstance<Ai> ();
            data.SheetName = filePath;
            data.WorksheetName = sheetName;
            AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
            //data.hideFlags = HideFlags.NotEditable;
        }
            
        //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<AiData>().ToArray();		

        //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
        //EditorUtility.SetDirty (obj);

        ExcelQuery query = new ExcelQuery(filePath, sheetName);
        if (query != null && query.IsValid())
        {
            data.dataArray = query.Deserialize<AiData>().ToArray();

            ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            EditorUtility.SetDirty (obj);
        }
    }
}
