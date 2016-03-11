using UnityEngine;
using UnityEditor;
using System.Collections;

namespace RedBlueGames.Tools
{
    public class BuildScriptSettings : ScriptableObject
    {
        public const string SavePath = "Assets/RedBlueGames/RBScripts/BuildScripts/Editor/BuildScriptProjectSettings.asset";

        public string CompanyDisplayName = "Company Name";

        public string CompanyName
        {
            get
            {
                if (string.IsNullOrEmpty(CompanyDisplayName))
                {
                    return "";
                }
                return CompanyDisplayName.ToLower().Replace(" ", null);
            }
        }

        public string AppDisplayName = "App Name";

        public string AppName
        {
            get
            {
                if (string.IsNullOrEmpty(AppDisplayName))
                {
                    return "";
                }
                return AppDisplayName.ToLower().Replace(" ", null);
            }
        }

        public string DefaultFilenamePattern = appNameKey + "-v" + versionKey;

        public string DefaultFilename
        {
            get
            {
                var pattern = DefaultFilenamePattern;
                if (string.IsNullOrEmpty(pattern))
                {
                    return string.Empty;
                }
                return ReplaceKeys(pattern);
            }
        }

        public string DefaultBundleIdentifierPattern = "com." + companyKey + "." + appNameKey;

        public string DefaultBundleIdentifier
        {
            get
            {
                var pattern = DefaultBundleIdentifierPattern;
                if (string.IsNullOrEmpty(pattern))
                {
                    return string.Empty;
                }
                return ReplaceKeys(pattern);
            }
        }

        const string appNameKey = "${appName}";
        const string versionKey = "${version}";
        const string companyKey = "${companyName}";

        string ReplaceKeys(string inputString)
        {
            inputString = inputString.Replace(appNameKey, AppName);
            inputString = inputString.Replace(versionKey, PlayerSettings.bundleVersion);
            inputString = inputString.Replace(companyKey, CompanyName);
            return inputString;
        }
    }
}