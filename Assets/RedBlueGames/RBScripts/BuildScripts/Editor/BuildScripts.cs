using UnityEngine;
using UnityEditor;

namespace RedBlueGames.Tools
{
    public class BuildScripts : EditorWindow
    {
        const int NumDigitsPerVersionIteration = 2;
        string companyDisplayName;
        string filename;
        string appName;
        string bundleIdentifier;
        bool isDevelopmentBuild;
        string version = PlayerSettings.bundleVersion;
        string savePath;
        bool buildAndroid = true;
        bool buildIOS;
        bool iOSSimulationBuild = false;
        string androidKeystorePath = PlayerSettings.Android.keystoreName;
        string androidKeystorePassword;
        string androidKeyAlias = PlayerSettings.Android.keyaliasName;
        string androidKeyAliasPassword;
        BuildType buildType;

        BuildScriptSettings currentSettings;
        bool isInitialized;

        public enum BuildType
        {
            Development,
            Release
        }

        void OnEnable()
        {
            ImportBuildScriptSettings();
        }

        void ImportBuildScriptSettings()
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

        void ConfigureBuildOptionsForBuildType(BuildType type)
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

        void OnGUI()
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

        void CreateSettings()
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

        void ValidateAllEditorFields()
        {
            ValidateStringAsFilename(filename);
            ValidateStringAsBundleIdentifier(bundleIdentifier);
            ValidateStringAsVersionNumber(version);
        }

        void ValidateStringAsFilename(string inputString)
        {
            if (string.IsNullOrEmpty(inputString))
            {
                throw new System.FormatException("Filename string formatted incorrectly. Must be non-empty.");
            }
        }

        void ValidateStringAsBundleIdentifier(string inputString)
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

        void ValidateStringAsVersionNumber(string inputString)
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

        void PrepareUniversalBuild()
        {
            ValidateAllEditorFields();
        }

        string PromptUserForSaveLocation()
        {
            string path = EditorUtility.SaveFolderPanel("Choose location to save build(s)", "", "");
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }
            return path;
        }

        void ConfigureUniversalBuildSettings()
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

        void ConfigureAndroidBuildSettings()
        {
            PlayerSettings.Android.androidTVCompatibility = false;

            PlayerSettings.Android.bundleVersionCode = ConvertVersionStringToVersionCode(version);
            ConfigurePlatformDefinesForBuiltTarget(BuildTargetGroup.Android);

            PlayerSettings.Android.keystoreName = androidKeystorePath;
            PlayerSettings.Android.keystorePass = androidKeystorePassword;
            PlayerSettings.Android.keyaliasName = androidKeyAlias;
            PlayerSettings.Android.keyaliasPass = androidKeyAliasPassword;
        }

        int ConvertVersionStringToVersionCode(string inputString)
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

        void ConfigurePlatformDefinesForBuiltTarget(BuildTargetGroup targetGroup)
        {
            string defineSymbols = string.Empty;
            // Turn off Google Play Game Services for iOS
            if (targetGroup == BuildTargetGroup.iOS)
            {
                defineSymbols = "NO_GPGS";
            }
            PlayerSettings.SetScriptingDefineSymbolsForGroup(targetGroup, defineSymbols);
        }

        void ConfigureiOSBuildSettings()
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

        void ExportAndroidBuild()
        {
            BuildPipeline.BuildPlayer(GetLevelsForBuild(), savePath + "/" + filename + ".apk", BuildTarget.Android, GetBuildOptionsForBuild());
        }

        void ExportiOSBuild()
        {
            BuildPipeline.BuildPlayer(GetLevelsForBuild(), savePath + "/" + filename, BuildTarget.iOS, GetBuildOptionsForBuild());
        }

        string[] GetLevelsForBuild()
        {
            return SceneManager.GetAllPaths();
        }

        BuildOptions GetBuildOptionsForBuild()
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