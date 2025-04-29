using Newtonsoft.Json;
using System.IO;

namespace Tools{
    public static class JsonConverter
    {
        public static string ToJson<T>(T StorableData)
        {
            string jsonData = JsonConvert.SerializeObject(StorableData, Formatting.Indented);
            return jsonData;
        }

        public static T FromJson<T>(string filePath)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(filePath));
        }
    }
}

