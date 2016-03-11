namespace RedBlueGames.Tools
{
    using UnityEditor;
    using UnityEngine;

    public class BuildScripts : EditorWindow
    {
        private const int NumDigitsPerVersionIteration = 2;
        private string companyDisplayName;
        private string filename;
        private string appName;
        private string bundleIdentifier;
        private bool isDevelopmentBuild;
        private string version = PlayerSettings.bundleVersion;
        private string savePath;
        private bool buildAndroid = true;
        private bool buildIOS;
        private bool iOSSimulationBuild = false;
        private string androidKeystorePath = PlayerSettings.Android.keystoreName;
        private string androidKeystorePassword;
        private string androidKeyAlias = PlayerSettings.Android.keyaliasName;
        private string androidKeyAliasPassword;
        private BuildType buildType;

        private BuildScriptSettings currentSettings;
        private bool isInitialized;

        public enum BuildType
        {
            Development,
            Release
        }

        private void OnEnable()
        {
            ImportBuildScriptSettings();
        }

        private void ImportBuildScriptSettings()
        {
            var assetPath = BuildScriptSettings.SavePath;
            currentSettings = AssetDatabase.LoadAssetAtPath<BuildScriptSettings>(assetPath);

            if (currentSettings == null)
            {
                isInitialized = false;
            }
            else
            {
                isInitialized = true;

                companyDisplayName = currentSettings.CompanyDisplayName;
                appName = currentSettings.AppDisplayName;
                filename = currentSettings.DefaultFilename;
                bundleIdentifier = currentSettings.DefaultBundleIdentifier;
            }
        }

        private void ConfigureBuildOptionsForBuildType(BuildType type)
        {
            switch (type)
            {
                case BuildType.Development:
                    isDevelopmentBuild = true;
                    break;
                case BuildType.Release:
                    isDevelopmentBuild = false;
                    break;
                default:
                    Debug.LogError("Unrecognized build type selected for build.");
                    return;
            }
        }

        [MenuItem("Window/BuildScripts/ExportBuild")]
        public static void ExportBuild()
        {
            EditorWindow.GetWindow<BuildScripts>("Export Builds");
        }

        private void OnGUI()
        {
            GUILayout.Label("BuildSettings", EditorStyles.boldLabel);
            if (!isInitialized)
            {
                EditorGUILayout.HelpBox("Build Scripts have not been setup. Create a settings file" +
                    " and enter the project settings to begin.", MessageType.Info);
                if (GUILayout.Button("Create Settings"))
                {
                    CreateSettings();
                }
                return;
            }

            filename = EditorGUILayout.TextField("Filename: ", filename);
            version = EditorGUILayout.TextField("Version: ", version);

            buildType = (BuildType)EditorGUILayout.EnumPopup("Build Type: ", buildType);

            // Handle App Name and Bundle ID
            appName = EditorGUILayout.TextField("App Name: ", appName);
            bundleIdentifier = EditorGUILayout.TextField("Bundle Identifier: ", bundleIdentifier);

            EditorGUILayout.LabelField("Platform Settings - iOS", EditorStyles.boldLabel);
            iOSSimulationBuild = EditorGUILayout.Toggle("iOS Simulation", iOSSimulationBuild);

            GUILayout.Label("Build Targets", EditorStyles.boldLabel);
            buildAndroid = EditorGUILayout.Toggle("Android", buildAndroid);
            buildIOS = EditorGUILayout.Toggle("iOS", buildIOS);

            if (buildAndroid && buildType == BuildType.Release)
            {
                EditorGUILayout.LabelField("Android Publishing Settings", EditorStyles.boldLabel);

                bool PressedKeystore = GUILayout.Button("Locate Keystore");

                androidKeystorePath = EditorGUILayout.TextField("Path To Keystore: ", androidKeystorePath);
                androidKeystorePassword = EditorGUILayout.PasswordField("Keystore Password: ", androidKeystorePassword);
                androidKeyAlias = EditorGUILayout.TextField("Key Alias: ", androidKeyAlias);
                androidKeyAliasPassword = EditorGUILayout.PasswordField("Key Alias Password: ", androidKeyAliasPassword);

                if (PressedKeystore)
                {
                    androidKeystorePath = EditorUtility.OpenFilePanel("Open Keystore", ".", "keystore");
                }
            }
            else
            {
                // Clear out our keystore info when BuildType is development
                androidKeystorePath = string.Empty;
                androidKeystorePassword = string.Empty;
                androidKeyAlias = string.Empty;
                androidKeyAliasPassword = string.Empty;
            }

            bool UpdatePressed = GUILayout.Button("Update");
            bool BuildPressed = GUILayout.Button("Build");

            if (BuildPressed || UpdatePressed)
            {
                try
                {
                    PrepareUniversalBuild();
                    if (BuildPressed)
                    {
                        savePath = PromptUserForSaveLocation();
                        if (string.IsNullOrEmpty(savePath))
                        {
                            throw new System.ArgumentException("No save path provided");
                        }
                    }
                }
                catch (System.FormatException formatException)
                {
                    Debug.LogError("Error during update: " + formatException.Message);
                    return;
                }

                ConfigureUniversalBuildSettings();

                if (BuildPressed && buildIOS == false && buildAndroid == false)
                {
                    Debug.LogError("Tried to do build without specifying a build target.");
                    return;
                }

                if (buildIOS)
                {
                    ConfigureiOSBuildSettings();
                    if (BuildPressed)
                    {
                        ExportiOSBuild();
                    }
                }

                if (buildAndroid)
                {
                    ConfigureAndroidBuildSettings();
                    if (BuildPressed)
                    {
                        ExportAndroidBuild();
                    }
                }
            }
        }

        private void CreateSettings()
        {
            var assetPath = BuildScriptSettings.SavePath;
            var asset = ScriptableObject.CreateInstance<BuildScriptSettings>();
            AssetDatabase.CreateAsset(asset, assetPath);

            AssetDatabase.SaveAssets();
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = asset;

            ImportBuildScriptSettings();
        }

        #region Field Validation

        private void ValidateAllEditorFields()
        {
            ValidateStringAsFilename(filename);
            ValidateStringAsBundleIdentifier(bundleIdentifier);
            ValidateStringAsVersionNumber(version);
        }

        private void ValidateStringAsFilename(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new System.FormatException("Filename string formatted incorrectly. Must be non-empty.");
            }
        }

        private void ValidateStringAsBundleIdentifier(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new System.FormatException("BundleIdentifier string formatted incorrectly. Must be non-empty.");
            }

            System.Text.RegularExpressions.Regex stringFormat =
                new System.Text.RegularExpressions.Regex("^com\\." + currentSettings.CompanyName + "\\.[\\w\\d]+$");
            if (!stringFormat.IsMatch(inputString))
            {
                throw new System.FormatException("BundleIdentifier string formatted incorrectly. Must be of the regex format: "
                    + stringFormat.ToString());
            }
        }

        private void ValidateStringAsVersionNumber(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new System.FormatException("Version number is not a valid format. Must be non-empty");
            }

            // Valid format of version number is ##.##.## or 1.12.1
            string numDigitsPerIterationAsChar = NumDigitsPerVersionIteration.ToString();
            string regexNoMoreThanXDigits = "[0-9]{1," + numDigitsPerIterationAsChar + "}";
            System.Text.RegularExpressions.Regex stringFormat =
                new System.Text.RegularExpressions.Regex("^" + regexNoMoreThanXDigits +
                    "\\." + regexNoMoreThanXDigits + "\\."
                    + regexNoMoreThanXDigits + "$");

            if (!stringFormat.IsMatch(inputString))
            {
                throw new System.FormatException("Version number is not a valid format." +
                    " Please match the format ##.##.##, or as a regular expression: " + stringFormat.ToString());
            }
        }

        #endregion

        #region Build Configuration

        private void PrepareUniversalBuild()
        {
            ValidateAllEditorFields();
        }

        private string PromptUserForSaveLocation()
        {
            string path = EditorUtility.SaveFolderPanel("Choose location to save build(s)", "", "");
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            return path;
        }

        private void ConfigureUniversalBuildSettings()
        {
            // Configure PlayerSettings that never change //
            PlayerSettings.companyName = companyDisplayName;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.allowedAutorotateToPortrait = true;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = true;
            PlayerSettings.allowedAutorotateToLandscapeLeft = false;
            PlayerSettings.allowedAutorotateToLandscapeRight = false;

            PlayerSettings.bundleIdentifier = bundleIdentifier;
            PlayerSettings.bundleVersion = version;

            PlayerSettings.productName = appName;
        }

        private void ConfigureAndroidBuildSettings()
        {
            PlayerSettings.Android.androidTVCompatibility = false;

            PlayerSettings.Android.bundleVersionCode = ConvertVersionStringToVersionCode(version);
            ConfigurePlatformDefinesForBuiltTarget(BuildTargetGroup.Android);

            PlayerSettings.Android.keystoreName = androidKeystorePath;
            PlayerSettings.Android.keystorePass = androidKeystorePassword;
            PlayerSettings.Android.keyaliasName = androidKeyAlias;
            PlayerSettings.Android.keyaliasPass = androidKeyAliasPassword;
        }

        private int ConvertVersionStringToVersionCode(string inputString)
        {
            // Version string must be the correct format
            // MAJOR_VERSION . MINOR_VERSION . BUILD
            // 4.1.1 = 4 * 10000, 1 * 100 + build = 40101
            string[] parsedVersion = inputString.Split('.');
            int maxNumIterationsInVersion = (int)Mathf.Pow(10, NumDigitsPerVersionIteration);
            int versionOrder = 0;
            int iteration = 0;
            for (int i = parsedVersion.Length - 1; i >= 0; i--)
            {
                int iterationValue = int.Parse(parsedVersion[i]) * (int)Mathf.Pow(maxNumIterationsInVersion, iteration);
                versionOrder += iterationValue;
                iteration++;
            }

            return versionOrder;
        }

        private void ConfigurePlatformDefinesForBuiltTarget(BuildTargetGroup targetGroup)
        {
            string defineSymbols = string.Empty;

            // Turn off Google Play Game Services for iOS
            if (targetGroup == BuildTargetGroup.iOS)
            {
                defineSymbols = "NO_GPGS";
            }

            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defineSymbols);
        }

        private void ConfigureiOSBuildSettings()
        {
            if (iOSSimulationBuild)
            {
                PlayerSettings.iOS.sdkVersion = iOSSdkVersion.SimulatorSDK;
            }
            else
            {
                PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
            }

            ConfigurePlatformDefinesForBuiltTarget(BuildTargetGroup.iOS);
        }

        #endregion

        #region Export Functions

        private void ExportAndroidBuild()
        {
            BuildPipeline.BuildPlayer(GetLevelsForBuild(), savePath + "/" + filename + ".apk", BuildTarget.Android, GetBuildOptionsForBuild());
        }

        private void ExportiOSBuild()
        {
            BuildPipeline.BuildPlayer(GetLevelsForBuild(), savePath + "/" + filename, BuildTarget.iOS, GetBuildOptionsForBuild());
        }

        private string[] GetLevelsForBuild()
        {
            return SceneManager.GetAllPaths();
        }

        private BuildOptions GetBuildOptionsForBuild()
        {
            BuildOptions buildOptions;
            if (isDevelopmentBuild)
            {
                buildOptions = BuildOptions.Development;
            }
            else
            {
                buildOptions = BuildOptions.None;
            }

            return buildOptions;
        }

        #endregion
    }
}