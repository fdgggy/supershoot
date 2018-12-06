using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class RoleAssetPostprocessor 
{
    private static readonly string filePath = "Assets/ExcelData/Role.xls";
    private static readonly string assetFilePath = "Assets/ExcelAssets/Role.asset";
    private static readonly string sheetName = "Role";
    
    public static void OnPostprocessAllAssets ()
    {
        Role data = (Role)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(Role));
        if (data == null) {
            data = ScriptableObject.CreateInstance<Role> ();
            data.SheetName = filePath;
            data.WorksheetName = sheetName;
            AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
            //data.hideFlags = HideFlags.NotEditable;
        }
            
        //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<RoleData>().ToArray();		

        //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
        //EditorUtility.SetDirty (obj);

        ExcelQuery query = new ExcelQuery(filePath, sheetName);
        if (query != null && query.IsValid())
        {
            data.dataArray = query.Deserialize<RoleData>().ToArray();

            ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            EditorUtility.SetDirty (obj);
        }
    }
}
