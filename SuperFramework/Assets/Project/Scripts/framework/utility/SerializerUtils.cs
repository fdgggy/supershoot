using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Xml.Serialization;


public class ObjectData2XmlSerializer<T>
{
    public void Serialize(T data, string path)
    {
        try
        {

            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                FileUtils.existPathDirectory(path);
            }
            FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
            XmlSerializer s = new XmlSerializer(typeof(T));
            s.Serialize(fileStream, data);
            fileStream.Close();
        }
        catch (System.Exception ex)
        {
            //Debug.Log(ex.ToString());
            throw (ex);
        }
    }

    public T Deserialize(string xml)
    {
        if (!File.Exists(xml))
        {
            return default(T);
        }
        try
        {
            XmlSerializer s = new XmlSerializer(typeof(T));
            T data = (T)s.Deserialize(File.OpenRead(xml));
            return data;
        }
        catch (System.Exception ex)
        {
            //Debug.Log(ex.ToString());
            throw (ex);
        }
    }
}

/************************************************************************/
/* 类数据和binary转换                                                     */
/************************************************************************/
public class ObjectData2BinSerializer<T>
{
    public void Serialize(T data, string path)
    {
        try
        {
            if (File.Exists(path))
            {
                File.Delete(path);
            }
            else
            {
                FileUtils.existPathDirectory(path);
            }
            FileStream fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
            BinaryFormatter b = new BinaryFormatter();
            b.Serialize(fileStream, data);
            fileStream.Close();
        }
        catch (System.Exception ex)
        {
            //Debug.Log(ex.ToString());
            throw (ex);
        }
    }

    public T Deserialize(string path)
    {
        if (!File.Exists(path))
        {
            return default(T);
        }
        try
        {
            FileStream fileStream = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read);
            BinaryFormatter b = new BinaryFormatter();
            T data;
            data = (T)b.Deserialize(fileStream);
            fileStream.Close();
            return data;
        }
        catch (System.Exception ex)
        {
            //Debug.Log(ex.ToString());
            //return default(T);
            throw (ex);
        }
    }

    public T DeserializeBinary(byte[] bytes)
    {
        if (null == bytes)
        {
            return default(T);
        }

        try
        {
            Stream steam = new MemoryStream(bytes);
            BinaryFormatter b = new BinaryFormatter();
            T data = (T)b.Deserialize(steam);
            steam.Close();
            return data;
        }
        catch (System.Exception ex)
        {
            //Debug.Log(ex.ToString());
            //return default(T);
            throw (ex);
        }
    }
}
