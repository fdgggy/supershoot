using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using UnityQuickSheet;

///
/// !!! Machine generated code !!!
///
public class WeaponAssetPostprocessor 
{
    private static readonly string filePath = "Assets/ExcelData/Weapon.xls";
    private static readonly string assetFilePath = "Assets/ExcelAssets/Weapon.asset";
    private static readonly string sheetName = "Weapon";
    
    public static void OnPostprocessAllAssets ()
    {
        Weapon data = (Weapon)AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(Weapon));
        if (data == null) {
            data = ScriptableObject.CreateInstance<Weapon> ();
            data.SheetName = filePath;
            data.WorksheetName = sheetName;
            AssetDatabase.CreateAsset ((ScriptableObject)data, assetFilePath);
            //data.hideFlags = HideFlags.NotEditable;
        }
            
        //data.dataArray = new ExcelQuery(filePath, sheetName).Deserialize<WeaponData>().ToArray();		

        //ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
        //EditorUtility.SetDirty (obj);

        ExcelQuery query = new ExcelQuery(filePath, sheetName);
        if (query != null && query.IsValid())
        {
            data.dataArray = query.Deserialize<WeaponData>().ToArray();

            ScriptableObject obj = AssetDatabase.LoadAssetAtPath (assetFilePath, typeof(ScriptableObject)) as ScriptableObject;
            EditorUtility.SetDirty (obj);
        }
    }
}
