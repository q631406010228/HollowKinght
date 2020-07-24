using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Assets.CommonMethod
{
    class JsonManage<T>
    {

        string Path;

        public JsonManage(string path)
        {
            Path = path;
        }

        public static Dictionary<string, T> ReadJsonDataDictionary(string path)
        {
            string json = File.ReadAllText(path);
            if (json == "" || json == null)
                return null;
            Dictionary<string, T> jsonData = new Dictionary<string, T>();
            jsonData = JsonUtility.FromJson<Serialization<string, T>>(json).ToDictionary();
            return jsonData;
        }

        public static void SaveJsonDataDictionary(Dictionary<string, T> jsonData,string path)
        {
            string runeJson = JsonUtility.ToJson(new Serialization<string, T>(jsonData));
            File.WriteAllText(path, runeJson);
        }

        public static List<T> ReadJsonDataList(string path)
        {
            string json = File.ReadAllText(path);
            if (json == "" || json == null)
                return null;
            List<T> jsonData = new List<T>();
            jsonData = JsonUtility.FromJson<Serialization<T>>(json).ToList();
            return jsonData;
        }

        public static void SaveJsonDataList(List<T> jsonData, string path)
        {
            string runeJson = JsonUtility.ToJson(new Serialization<T>(jsonData));
            File.WriteAllText(path, runeJson);
        }

        public static T ReadJsonData(string path)
        {
            string json = File.ReadAllText(path);
            if (json == "" || json == null)
                return default;
            T jsonData;
            jsonData = JsonUtility.FromJson<T>(json);
            return jsonData;
        }

        public static void SaveJsonData(T jsonData, string path)
        {
            string runeJson = JsonUtility.ToJson(jsonData);
            File.WriteAllText(path, runeJson);
        }

    }
}
