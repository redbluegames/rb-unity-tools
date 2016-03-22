namespace RedBlueGames.Tools
{
    using System.Collections;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Settings file for Build Exporter tool
    /// </summary>
    public class BuildScriptSettings : ScriptableObject
    {
        public const string SavePath = "Assets/RedBlueGames/RBScripts/BuildScripts/Editor/BuildScriptProjectSettings.asset";
        private const string AppNameKey = "${appName}";
        private const string VersionKey = "${version}";
        private const string CompanyKey = "${companyName}";

        [SerializeField]
        private string companyDisplayName = "Company Name";
        [SerializeField]
        private string defaultFilenamePattern = AppNameKey + "-v" + VersionKey;
        [SerializeField]
        private string defaultBundleIdentifierPattern = "com." + CompanyKey + "." + AppNameKey;

        [SerializeField]
        private string appDisplayName = "App Name";

        /// <summary>
        /// Gets the Display name of the company, as opposed to an internal key. Ex: Red Blue Games
        /// </summary>
        /// <value>The name of the company as displayed to public.</value>
        public string CompanyDisplayName
        {
            get
            {
                return this.companyDisplayName;
            }
        }

        /// <summary>
        /// Gets the name of the company used internally. Ex: redbluegames
        /// </summary>
        /// <value>The name of the company.</value>
        public string CompanyName
        {
            get
            {
                if (string.IsNullOrEmpty(this.companyDisplayName))
                {
                    return string.Empty;
                }

                return this.companyDisplayName.ToLower().Replace(" ", null);
            }
        }

        /// <summary>
        /// Gets the name of the app to display in the operating system. Ex: Mini Fish
        /// </summary>
        /// <value>The name of the app display.</value>
        public string AppDisplayName
        {
            get
            {
                return this.appDisplayName;
            }
        }

        /// <summary>
        /// Gets the internal name of the app. Ex: minifish
        /// </summary>
        /// <value>The name of the app.</value>
        public string AppName
        {
            get
            {
                if (string.IsNullOrEmpty(this.appDisplayName))
                {
                    return string.Empty;
                }

                return this.appDisplayName.ToLower().Replace(" ", null);
            }
        }

        /// <summary>
        /// Gets the default filename.
        /// </summary>
        /// <value>The default filename.</value>
        public string DefaultFilename
        {
            get
            {
                var pattern = this.defaultFilenamePattern;
                if (string.IsNullOrEmpty(pattern))
                {
                    return string.Empty;
                }

                return this.ReplaceKeys(pattern);
            }
        }

        /// <summary>
        /// Gets the default bundle identifier.
        /// </summary>
        /// <value>The default bundle identifier.</value>
        public string DefaultBundleIdentifier
        {
            get
            {
                var pattern = this.defaultBundleIdentifierPattern;
                if (string.IsNullOrEmpty(pattern))
                {
                    return string.Empty;
                }

                return this.ReplaceKeys(pattern);
            }
        }

        private string ReplaceKeys(string inputString)
        {
            inputString = inputString.Replace(BuildScriptSettings.AppNameKey, this.AppName);
            inputString = inputString.Replace(BuildScriptSettings.VersionKey, PlayerSettings.bundleVersion);
            inputString = inputString.Replace(BuildScriptSettings.CompanyKey, this.CompanyName);
            return inputString;
        }
    }
}