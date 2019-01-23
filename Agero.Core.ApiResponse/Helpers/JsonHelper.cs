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

                if (obj == null)
                    return json;

                if (obj is JObject jObj)
                {
                    MaskPassword(jObj);
                    return JsonConvert.SerializeObject(jObj);
                }

                return json;
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
                if (token.Value is JObject jObj)
                {
                    MaskPassword(jObj);
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
