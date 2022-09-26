using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Text;

namespace QuickLauncher.Utils
{
    // Version 1.0.0
    // Mon 09/26/2022 10:24:15.92
    internal class SimpleConfigUtils
    {
        const string CONFIG = "config.txt";
        const char SPLITER = ',';
        const string TRUE = "true";
        const string FALSE = "false";

        static readonly List<string> ConfigStrings = new List<string>
        {
            "tf_executable_path",
            "collection_url",
            "workspace",
            "user_name",
            "password",
            "project_path",
            "tfs_path",
            "drag_and_drop_to_checkout",
            "default_text_editor"
        };

        public static StringDictionary ReadConfigs()
        {
            StringDictionary configs = new StringDictionary();
            if (File.Exists(CONFIG))
            {
                List<string> configStrings = File.ReadAllLines(CONFIG).ToList();
                configStrings.Sort();
                foreach (var configString in configStrings)
                {
                    if (configString.Contains(SPLITER))
                    {
                        List<string> configKeyValue = configString.Split(SPLITER).ToList();
                        configs.Add(configKeyValue[0], configKeyValue[1]);
                    }
                }
            }
            return configs;
        }

        public static bool? IsTrue(string key)
        {
            StringDictionary configs = ReadConfigs();
            if (configs.ContainsKey(key))
            {
                if (configs[key].ToLower().Equals(TRUE))
                    return true;
                else if (configs[key].ToLower().Equals(FALSE))
                    return false;
                else return null;
            }
            else return null;
        }

        public static string GetConfig(string key)
        {
            StringDictionary configs = ReadConfigs();
            if (configs.ContainsKey(key)) return configs[key];
            else return "";
        }

        public static void AddConfig(string key, string value)
        {
            List<string> config = new List<string>();
            config.Add(key + "," + value);
            File.AppendAllLines("config.txt", config);
        }

        public static void SetConfig(string key, string newValue)
        {
            StringDictionary configs = ReadConfigs();
            if (configs.ContainsKey(key)) configs.Remove(key);
            configs.Add(key, newValue);

            List<string> configList = new List<string>();
            foreach (var configKey in configs.Keys)
            {
                configList.Add(configKey.ToString() + "," + configs[configKey.ToString()]);
            }
            configList.Sort();
            File.WriteAllLines("config.txt", configList);
        }

        public static void RemoveConfig(string key)
        {
            StringDictionary configs = ReadConfigs();
            if (configs.ContainsKey(key)) configs.Remove(key);
            
            List<string> configList = new List<string>();
            foreach (var configKey in configs.Keys)
            {
                configList.Add(configKey.ToString() + "," + configs[configKey.ToString()]);
            }
            configList.Sort();
            File.WriteAllLines("config.txt", configList);
        }

        public static bool ConfigVerification()
        {
            foreach (var configString in ConfigStrings)
            {
                if (string.IsNullOrEmpty(GetConfig(configString))) return false;
            }
            return true;
        }
    }
}
