using System.IO;
using Unity.VisualScripting;
using UnityEngine;

public static class Serializer
{
    public static void Save<T>(T value, string directory, string fileName)
    {
        if (!Directory.Exists(directory))
            Directory.CreateDirectory(directory);

        string json = JsonUtility.ToJson(value);

        using StreamWriter sw = new(Path.Combine(directory, fileName));
        sw.Write(json);
        sw.Flush();

        Debug.Log($"Save data successfully saved as {json}");
    }

    public static T Load<T>(T defaultValue, string directory, string fileName)
    {
        if (!File.Exists(Path.Combine(directory, fileName)))
        {
            Debug.Log($"Save data successfully loaded default as {JsonUtility.ToJson(defaultValue)}");
            return defaultValue;
        }

        using StreamReader sr = new(Path.Combine(directory, fileName));
        string json = sr.ReadToEnd();

        Debug.Log($"Save data successfully loaded as {json}");

        return JsonUtility.FromJson<T>(json);
    }
}