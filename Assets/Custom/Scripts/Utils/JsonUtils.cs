using System;
using System.Collections.Generic;
using System.Linq;

namespace com.terranovita.botretreat
{
    public static class JsonUtils
    {

        public static string getStringValue(this JSONObject json, string fieldName)
        {
            var fieldJson = json.GetField(fieldName);
            if (fieldJson != null && fieldJson.IsString && fieldJson.str != null)
            {
                return fieldJson.str;
            }
            return null;
        }
        public static bool getBoolValue(this JSONObject json, string fieldName, bool defaultVal = false)
        {
            var fieldJson = json.GetField(fieldName);
            if (fieldJson != null && fieldJson.IsBool)
            {
                return fieldJson.b;
            }
            return defaultVal;
        }
        public static int getIntValue(this JSONObject json, string fieldName, int defaultVal = 0)
        {
            var fieldJson = json.GetField(fieldName);
            if (fieldJson != null && fieldJson.IsNumber)
            {
                return (int)fieldJson.i;
            }
            return defaultVal;
        }
        public static TEnum getEnumValue<TEnum>(this JSONObject json, string fieldName, TEnum defaultVal = default(TEnum)) where TEnum : struct
        {
            var fieldJson = json.GetField(fieldName);
            if (fieldJson != null && fieldJson.IsNumber)
            {
                return (TEnum)Enum.ToObject(typeof(TEnum), fieldJson.i);
            }
            return defaultVal;
        }

        public static T GetValue<T>(this JSONObject json, String fieldName) where T : ICreatableFromJson<T>, new()
        {
            var fieldJson = json.GetField(fieldName);
            return fieldJson != null ? new T().FromJson(fieldJson) : default(T);
        }

        public static List<T> GetValues<T>(this JSONObject json) where T : ICreatableFromJson<T>, new()
        {
            return json.list.Select(jsonObject => new T().FromJson(jsonObject)).ToList();
        }

        public static List<T> GetValues<T>(this JSONObject json, String fieldName) where T : ICreatableFromJson<T>, new()
        {
            var fieldJson = json.GetField(fieldName);
            return fieldJson.list.Select(jsonObject => new T().FromJson(jsonObject)).ToList();
        }
    }
}