using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Xml.Serialization;
using System.IO;

public class FileSave
{
 
    public void WriteXml<T>(T model)
    {
        XmlSerializer writer = new XmlSerializer(typeof(T));
        using (FileStream fileStream = new FileStream(typeof(T) + ".xml", FileMode.OpenOrCreate))
        {
            writer.Serialize(fileStream, model);
        }
    }

    public T ReadXml<T>()
    {
        XmlSerializer reader = new XmlSerializer(typeof(GameModel));
        T value;
        using (FileStream fileStream = new FileStream(typeof(T) + ".xml", FileMode.OpenOrCreate))
        {
            value = (T)reader.Deserialize(fileStream);
        }
        return value;
    }
}
