using UnityEngine;
using UnityEditor;
using System.IO;
using UnityQuickSheet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class ExcelTool
{
    //all c# keywords.
    public static string[] Keywords = new string[] {
            "abstract", "as", "base", "bool", "break", "byte", "case", "catch", "char", "checked",
            "class", "const", "continue", "decimal", "default", "delegate", "do", "double", "else", "enum",
            "event", "explicit", "extern", "false", "finally", "fixed", "float", "for", "foreach", "goto",
            "if", "implicit", "in", "in", "int", "interface", "internal", "is", "lock", "long",
            "namespace", "new", "null", "object", "operator", "out", "override", "params", "private", "protected",
            "public", "readonly", "ref", "return", "sbyte", "sealed", "short", "sizeof", "stackalloc", "static",
            "string", "struct", "switch", "this", "throw", "true", "try", "typeof", "uint", "ulong", "unchecked", "unsafe",
            "ushort", "using", "virtual", "void", "volatile", "while",
        };
    public static string[] Typewords = new string[] {
        "String",
        "Short",
        "Int",
        "Long",
        "Float",
        "Double",
        "Bool",
    };

    static string sourcePath = Application.dataPath + "/ExcelData";
    static string scriptRunTimeOutPath = Application.dataPath + "/Project/Excel/Runtime";
    static string scriptEditorOutPath = Application.dataPath + "/Project/Excel/Editor";
    static string outPath = "Assets/ExcelAssets";
    static string templatePath = "3rd/QuickSheet/ExcelPlugin/Templates";
    static string excelMgrOutPath = Application.dataPath + "/Project/Scripts/framework/data";

    [MenuItem("Excel/ExcelToScriptObject First Click")]
    public static void ExcelToAsset()
    {
        List<string> excelClass = new List<string>();
        List<string> excelNames = new List<string>();

        FileUtils fileUtil = new FileUtils();
        fileUtil.searchFileUnder(sourcePath, ".xls", (string filePath) =>
        {
            filePath = filePath.Replace("\\", "/");
            filePath = "Assets/" + filePath.Replace(Application.dataPath + "/", "");

            if (string.IsNullOrEmpty(filePath))
            {
                return;
            }

            string []sheetNames = new ExcelQuery(filePath).GetSheetNames();
            if (sheetNames.Length <= 0)
            {
                return;
            }
            foreach(var sheetName in sheetNames)
            {
                string error1 = string.Empty;
                string error2 = string.Empty;
                var titles = new ExcelQuery(filePath, sheetName).GetTitle(2, ref error1);
                var titles2 = new ExcelQuery(filePath, sheetName).GetTitle(3, ref error2);
                if (titles == null || !string.IsNullOrEmpty(error1) || titles2 == null || !string.IsNullOrEmpty(error2))
                {
                    Debug.Log("Error1:" + error1);
                    Debug.Log("Error2:" + error2);
                    return;
                }
                else
                {
                    foreach (string column in titles)
                    {
                        if (!IsValidHeader(column))
                        {
                            error1 = string.Format(@"Invalid column header name {0}. Any c# keyword should not be used for column header. Note it is not case sensitive.", column);
                            Debug.Log("Error:" + error1);
                            return;
                        }
                    }
                    foreach(string column in titles2)
                    {
                        if (!IsValidValueType(column))
                        {
                            error1 = string.Format(@"Invalid column header name {0}. Any c# value type should not be used for column header.", column);
                            Debug.Log("Error:" + error1);
                            return;
                        }
                    }
                }

                List<ColumnHeader> columnHeaderList = new List<ColumnHeader>();
                List<string> titleList = titles.ToList();
                List<string> titleList2= titles2.ToList();
                if (titleList.Count != titleList2.Count)
                {
                    Debug.LogError("check the excel title !");
                    return;
                }
                int order = 0;
                for (int i = 0; i < titleList.Count; i++)
                {
                    ColumnHeader colum = new ColumnHeader()
                    {
                        type = (CellType)Enum.Parse(typeof(CellType), titleList2[i]),
                        name = titleList[i],
                        isEnable = true,
                        isArray = false,
                        OrderNO = order++,
                    };

                    columnHeaderList.Add(colum);
                }

                if (titleList.Count > 0)
                {
                    Directory.CreateDirectory(scriptRunTimeOutPath);
                    Directory.CreateDirectory(scriptEditorOutPath);

                    ScriptPrescription sp = new ScriptPrescription();
                    CreateScriptableObjectClassScript(sp, sheetName);
                    //CreateScriptableObjectEditorClassScript(sp, sheetName);
                    CreateDataClassScript(sp, sheetName, columnHeaderList);
                    CreateAssetCreationScript(sp, sheetName, filePath);

                    excelClass.Add(sheetName + "AssetPostprocessor");
                    excelNames.Add(sheetName);

                    if (sp != null)
                    {
                        Debug.Log(sheetName + " Successfully generated!");
                    }
                    else
                        Debug.LogError(sheetName + " Failed to create a script from excel.");
                }
                else
                {
                    string msg = string.Format("An empty workhheet: [{0}] ", sheetName);
                    Debug.LogWarning(msg);
                }
            }

            ScriptPrescription sp2 = new ScriptPrescription();
            string result = string.Empty;
            foreach(string value in excelClass)
            {
                result = result + "\t\t\"" + value + "\",\n";
            }

            sp2.excelClassNames = result;
            sp2.template = GetTemplate("RecordExcelClass");

            using (var writer = new StreamWriter(Path.Combine(scriptEditorOutPath, "RecordExcelInfo" + "." + "cs")))
            {
                writer.Write(new ScriptGenerator(sp2).ToString());
                writer.Close();
            }

            ScriptPrescription sp3 = new ScriptPrescription();
            string result2 = string.Empty;
            foreach (string value in excelNames)
            {
                result2 = result2 + "\t" + value + ",\n";
            }
            result2 = result2 + "\t" + "Num";

            sp3.excelNames = result2;
            sp3.template = GetTemplate("ExcelDataManagerClass");

            using (var writer = new StreamWriter(Path.Combine(excelMgrOutPath, "ExcelDataManager" + "." + "cs")))
            {
                writer.Write(new ScriptGenerator(sp3).ToString());
                writer.Close();
            }

            AssetDatabase.Refresh();
        });
    }
    /// <summary>
    /// Generate AssetPostprocessor editor script file.
    /// </summary>
    private static void CreateAssetCreationScript(ScriptPrescription sp, string workSheetName, string filePath)
    {
        sp.className = workSheetName;
        sp.dataClassName = workSheetName + "Data";
        sp.worksheetClassName = workSheetName;

        // where the imported excel file is.
        sp.importedFilePath = filePath;

        // path where the .asset file will be created.
        if (!Directory.Exists(outPath))
        {
            Directory.CreateDirectory(outPath);
        }
        
        string path = outPath;
        path += "/" + workSheetName + ".asset";
        sp.assetFilepath = path;
        sp.assetPostprocessorClass = workSheetName + "AssetPostprocessor";
        sp.template = GetTemplate("PostProcessor");

        // write a script to the given folder.
        using (var writer = new StreamWriter(Path.Combine(scriptEditorOutPath, workSheetName + "AssetPostProcessor" + "." + "cs")))
        {
            writer.Write(new ScriptGenerator(sp).ToString());
            writer.Close();
        }
    }
    /// <summary>
    /// Create a data class which describes the spreadsheet and write it down on the specified folder.
    /// </summary>
    private static void CreateDataClassScript(ScriptPrescription sp, string workSheetName, List<ColumnHeader> columnHeaderList)
    {
        // check the directory path exists
        string fullPath = Path.Combine(scriptRunTimeOutPath, workSheetName + "Data" + "." + "cs");
        string folderPath = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(folderPath))
        {
            Debug.Log("The folder for runtime script files does not exist. Check the path " + folderPath + " exists.");
            return;
        }

        List<MemberFieldData> fieldList = new List<MemberFieldData>();

        //FIXME: replace ValueType to CellType and support Enum type.
        foreach (ColumnHeader header in columnHeaderList)
        {
            MemberFieldData member = new MemberFieldData();
            member.Name = header.name;
            member.type = header.type;
            member.IsArrayType = header.isArray;

            fieldList.Add(member);
        }

        sp.className = workSheetName + "Data";
        sp.template = GetTemplate("DataClass");

        sp.memberFields = fieldList.ToArray();

        // write a script to the given folder.		
        using (var writer = new StreamWriter(fullPath))
        {
            writer.Write(new ScriptGenerator(sp).ToString());
            writer.Close();
        }
    }
    /// <summary>
    /// Create a ScriptableObject editor class and write it down on the specified folder.
    /// </summary>
    private static void CreateScriptableObjectEditorClassScript(ScriptPrescription sp, string workSheetName)
    {
        sp.className = workSheetName + "Editor";
        sp.worksheetClassName = workSheetName;
        sp.dataClassName = workSheetName + "Data";
        sp.template = GetTemplate("ScriptableObjectEditorClass");

        // check the directory path exists
        string fullPath = Path.Combine("scriptEditorOutPath", workSheetName + "Editor" + "." + "cs");
        string folderPath = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(folderPath))
        {
            Debug.Log("The folder for editor script files does not exist.Check the path " + folderPath + " exists.");
            return;
        }

        StreamWriter writer = null;
        try
        {
            // write a script to the given folder.		
            writer = new StreamWriter(fullPath);
            writer.Write(new ScriptGenerator(sp).ToString());
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        finally
        {
            if (writer != null)
            {
                writer.Close();
                writer.Dispose();
            }
        }
    }
    /// <summary>
    /// Check the given header column has valid name which should not be any c# keywords.
    /// </summary>
    private static bool IsValidHeader(string s)
    {
        string comp = s.ToLower();
        string found = Array.Find(Keywords, x => x == comp);
        if (string.IsNullOrEmpty(found))
            return true;

        return false;
    }

    private static bool IsValidValueType(string t)
    {
        string found = Array.Find(Typewords, x => x == t);
        if (!string.IsNullOrEmpty(found))
            return true;

        return false;
    }
    /// <summary>
    /// Try to parse column-header if it contains '|'. Note postfix '!' means it has array type.
    /// e.g) 'Skill|string': Skill is string type.
    ///      'MyArray | int!' : MyArray is int array type. 
    /// </summary>
    /// <param name="s">A column header string in the spreadsheet.</param>
    /// <param name="order">A order number to sort column header.</param>
    /// <returns>A newly created ColumnHeader class instance.</returns>
    private static ColumnHeader ParseColumnHeader(string columnheader, int order)
    {
        // remove all white space. e.g.) "SkillLevel | uint"
        string cHeader = new string(columnheader.ToCharArray().Where(c => !Char.IsWhiteSpace(c)).ToArray());

        CellType ctype = CellType.Undefined;
        bool bArray = false;
        if (cHeader.Contains('|'))
        {
            // retrive columnheader name.
            string substr = cHeader;
            bArray = cHeader.Contains("!");
            substr = cHeader.Substring(0, cHeader.IndexOf('|'));

            // retrieve CellType from the columnheader.
            int startIndex = cHeader.IndexOf('|') + 1;
            int length = cHeader.Length - cHeader.IndexOf('|') - (bArray ? 2 : 1);
            string strType = cHeader.Substring(startIndex, length).ToLower();
            ctype = (CellType)Enum.Parse(typeof(CellType), strType, true);

            return new ColumnHeader { name = substr, type = ctype, isArray = bArray, OrderNO = order };
        }

        return new ColumnHeader { name = cHeader, type = CellType.Undefined, OrderNO = order };
    }

    /// <summary>
    /// Create a ScriptableObject class and write it down on the specified folder.
    /// </summary>
    private static void CreateScriptableObjectClassScript(ScriptPrescription sp, string workSheetName)
    {
        sp.className = workSheetName;
        sp.dataClassName = workSheetName + "Data";
        sp.template = GetTemplate("ScriptableObjectClass");

        string fullPath = Path.Combine(scriptRunTimeOutPath, workSheetName + "." + "cs");
        string folderPath = Path.GetDirectoryName(fullPath);
        if (!Directory.Exists(folderPath))
        {
            Debug.Log("The folder for runtime script files does not exist. Check the path " + folderPath + " exists.");
            return;
        }

        StreamWriter writer = null;
        try
        {
            writer = new StreamWriter(fullPath);
            writer.Write(new ScriptGenerator(sp).ToString());
        }
        catch (System.Exception e)
        {
            Debug.LogError(e.Message);
        }
        finally
        {
            if (writer != null)
            {
                writer.Close();
                writer.Dispose();
            }
        }
    }
    /// <summary>
    /// Retrieves all ascii text in the given template file.
    /// </summary>
    private static string GetTemplate(string nameWithoutExtension)
    {
        string path = Path.Combine(GetAbsoluteCustomTemplatePath(), nameWithoutExtension + ".txt");
        if (File.Exists(path))
            return File.ReadAllText(path);

        path = Path.Combine(GetAbsoluteBuiltinTemplatePath(), nameWithoutExtension + ".txt");
        if (File.Exists(path))
            return File.ReadAllText(path);

        return "No Template file is found";
    }

    /// <summary>
    /// e.g. "Assets/QuickSheet/Templates"
    /// </summary>
    private static string GetAbsoluteCustomTemplatePath()
    {
        return Path.Combine(Application.dataPath, templatePath);
    }

    /// <summary>
    /// e.g. "C:/Program File(x86)/Unity/Editor/Data"
    /// </summary>
    private static string GetAbsoluteBuiltinTemplatePath()
    {
        return Path.Combine(EditorApplication.applicationContentsPath, templatePath);
    }

    [MenuItem("Excel/ExcelToScriptObject Second Click")]
    public static void ExcelTest()
    {
        string className = "RecordExcelInfo";
        Type type = Type.GetType(className);
        System.Object obj = System.Activator.CreateInstance(type);
        MethodInfo method = type.GetMethod("Run");
        method.Invoke(obj, null);
    }
}
