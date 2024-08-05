using Newtonsoft.Json;
using UnityEngine;

namespace Source.Settings
{
    public static class KeyValueStorage
    {
        public static bool TryGetOrDefault<T>(string key, out T value) where T : new()
        {
            var json = PlayerPrefs.GetString(key, null);
            if (string.IsNullOrEmpty(json))
            {
                value = new T();
                return true;
            }

            value = JsonConvert.DeserializeObject<T>(json);
            return value != null;
        }

        public static void Set(string key, object value)
        {
            var json = JsonConvert.SerializeObject(value);
            PlayerPrefs.SetString(key, json);
            PlayerPrefs.Save();
        }
    }
}