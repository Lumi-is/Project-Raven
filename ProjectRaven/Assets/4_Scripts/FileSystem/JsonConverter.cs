using UnityEngine;
using Newtonsoft.Json;
using System.IO;

public class JsonConverter : MonoBehaviour
{
    protected string ToJson<T>(T StorableData)
    {
        string jsonData = JsonConvert.SerializeObject(StorableData, Formatting.Indented);
        return jsonData;
    }

    protected T FromJson<T>(string filePath)
    {
        return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
    }
}
