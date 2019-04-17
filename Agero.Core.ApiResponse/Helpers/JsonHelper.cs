using Agero.Core.Checker;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;

namespace Agero.Core.ApiResponse.Helpers
{
    internal static class JsonHelper
    {
        public static string MaskPassword(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
                return json;

            try
            {
                var obj = JsonConvert.DeserializeObject(json);
                switch (obj)
                {
                    case null:
                        return json;
                    case JObject jObj:
                        MaskPassword(jObj);
                        return JsonConvert.SerializeObject(jObj);
                    default:
                        return json;
                }
            }
            catch (JsonReaderException)
            {
                return json;
            }
        }

        private static void MaskPassword(JObject obj)
        {
            Check.ArgumentIsNull(obj, "obj");

            foreach (var token in obj)
            {
                if (token.Value is JObject propertyObject)
                {
                    MaskPassword(propertyObject);
                    continue;
                }

                if (!string.Equals(token.Key, "Password", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                var propertyValue = token.Value as JValue;
                if (propertyValue == null)
                    continue;

                var value = propertyValue.Value as string;
                if (value == null)
                    continue;

                propertyValue.Value = "*****";
            }
        }
    }
}
