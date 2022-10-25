using System.Collections.Generic;
using System.Dynamic;
using System.Text.Json;

namespace SystemReport.WebAPI.Extensions
{
    public static class JsonExtensions
    {
        public static object ToObject(this JsonDocument jsonDoc)
        {
            return Convert(jsonDoc.RootElement);
        }

        private static object Convert(JsonElement json)
        {

            if (json.ValueKind != JsonValueKind.Array && json.ValueKind != JsonValueKind.Object)
            {
                return json;
            }

            var serialized = JsonSerializer.Deserialize<ExpandoObject>(json.GetRawText());
            var output = new ExpandoObject();
            var outputDict = (IDictionary<string, object>)output;

            foreach (var prop in (IDictionary<string, object>)serialized)
            {
                var propValue = (JsonElement)prop.Value;

                switch (propValue.ValueKind)
                {
                    case JsonValueKind.Array:
                        var list = new List<object>();
                        foreach (var item in propValue.EnumerateArray())
                        {
                            list.Add(Convert(item));
                        }
                        outputDict[prop.Key] = list.ToArray();
                        break;

                    case JsonValueKind.Object:
                        ExpandoObject val = new ExpandoObject();
                        var node = (IDictionary<string, object>)val;
                        foreach (var objElement in propValue.EnumerateObject())
                        {
                            var v = Convert(objElement.Value);
                            node.Add(objElement.Name, v);
                        }
                        outputDict[prop.Key] = val;
                        break;

                    default:
                        outputDict[prop.Key] = prop.Value;
                        break;
                }
            }

            return output;
        }
    }
}
