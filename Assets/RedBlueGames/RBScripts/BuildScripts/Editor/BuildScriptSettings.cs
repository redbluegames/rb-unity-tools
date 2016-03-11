namespace RedBlueGames.Tools
{
    using System.Collections;
    using UnityEditor;
    using UnityEngine;

    public class BuildScriptSettings : ScriptableObject
    {
        public const string SavePath = "Assets/RedBlueGames/RBScripts/BuildScripts/Editor/BuildScriptProjectSettings.asset";
        public string CompanyDisplayName = "Company Name";
        public string DefaultFilenamePattern = appNameKey + "-v" + versionKey;
        public string DefaultBundleIdentifierPattern = "com." + companyKey + "." + appNameKey;

        private const string appNameKey = "${appName}";
        private const string versionKey = "${version}";
        private const string companyKey = "${companyName}";

        public string CompanyName
        {
            get
            {
                if (string.IsNullOrEmpty(CompanyDisplayName))
                {
                    return string.Empty;
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
                    return string.Empty;
                }

                return AppDisplayName.ToLower().Replace(" ", null);
            }
        }

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

        private string ReplaceKeys(string inputString)
        {
            inputString = inputString.Replace(appNameKey, AppName);
            inputString = inputString.Replace(versionKey, PlayerSettings.bundleVersion);
            inputString = inputString.Replace(companyKey, CompanyName);
            return inputString;
        }
    }
}