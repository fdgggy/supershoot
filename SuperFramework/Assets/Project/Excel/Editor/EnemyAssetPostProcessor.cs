using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class EnemyAssetPostprocessor 
{
    private static readonly string filePath = "Assets/ExcelData/Enemy.xls";
    private static readonly string assetFilePath = "Assets/ExcelAssets/Enemy.asset";
    private static readonly string sheetName = "Enemy";
    
    public static void OnPostprocessAllAssets ()
    {
        Enemy data = (Enemy)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(Enemy));
        if (data == null) {
            data = ScriptableObject.CreateInstance<Enemy> ();
            data.SheetName = filePath;
            data.WorksheetName = sheetName;
            AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
            //data.hideFlags = HideFlags.NotEditable;
        }
            
        //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<EnemyData>().ToArray();		

        //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
        //EditorUtility.SetDirty (obj);

        ExcelQuery query = new ExcelQuery(filePath, sheetName);
        if (query != null && query.IsValid())
        {
            data.dataArray = query.Deserialize<EnemyData>().ToArray();

            ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            EditorUtility.SetDirty (obj);
        }
    }
}
