namespace RedBlueGames.Tools
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// This tool helps us streamline the build process by combining shared settings, and building multiple platforms
    /// at once
    /// </summary>
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
        private bool simulationBuildIOS = false;
        private string androidKeystorePath = PlayerSettings.Android.keystoreName;
        private string androidKeystorePassword;
        private string androidKeyAlias = PlayerSettings.Android.keyaliasName;
        private string androidKeyAliasPassword;
        private BuildType buildType;

        private BuildScriptSettings currentSettings;
        private bool isInitialized;

        private enum BuildType
        {
            Development,
            Release
        }

        [MenuItem(RBToolsMenuPaths.BuildScripts)]
        private static void ExportBuild()
        {
            EditorWindow.GetWindow<BuildScripts>("Export Builds");
        }

        private void OnEnable()
        {
            this.ImportBuildScriptSettings();
        }

        private void ImportBuildScriptSettings()
        {
            var assetPath = BuildScriptSettings.SavePath;
            var loadedSettings = AssetDatabase.LoadAssetAtPath<BuildScriptSettings>(assetPath);

            if (loadedSettings == null)
            {
                this.isInitialized = false;
            }
            else
            {
                this.isInitialized = true;
                this.currentSettings = loadedSettings;

                this.companyDisplayName = loadedSettings.CompanyDisplayName;
                this.appName = loadedSettings.AppDisplayName;
                this.filename = loadedSettings.DefaultFilename;
                this.bundleIdentifier = loadedSettings.DefaultBundleIdentifier;
            }
        }

        private void ConfigureBuildOptionsForBuildType(BuildType type)
        {
            switch (type)
            {
                case BuildType.Development:
                    this.isDevelopmentBuild = true;
                    break;
                case BuildType.Release:
                    this.isDevelopmentBuild = false;
                    break;
                default:
                    Debug.LogError("Unrecognized build type selected for build.");
                    return;
            }
        }

        private void OnGUI()
        {
            GUILayout.Label("BuildSettings", EditorStyles.boldLabel);
            if (!this.isInitialized)
            {
                EditorGUILayout.HelpBox(
                    "Build Scripts have not been setup. Create a settings file and enter the project settings to begin.",
                    MessageType.Info);
                if (GUILayout.Button("Create Settings"))
                {
                    this.CreateSettings();
                }

                return;
            }

            this.filename = EditorGUILayout.TextField("Filename: ", this.filename);
            this.version = EditorGUILayout.TextField("Version: ", this.version);

            this.buildType = (BuildType)EditorGUILayout.EnumPopup("Build Type: ", this.buildType);

            // Handle App Name and Bundle ID
            this.appName = EditorGUILayout.TextField("App Name: ", this.appName);
            this.bundleIdentifier = EditorGUILayout.TextField("Bundle Identifier: ", this.bundleIdentifier);

            EditorGUILayout.LabelField("Platform Settings - iOS", EditorStyles.boldLabel);
            this.simulationBuildIOS = EditorGUILayout.Toggle("iOS Simulation", this.simulationBuildIOS);

            GUILayout.Label("Build Targets", EditorStyles.boldLabel);
            this.buildAndroid = EditorGUILayout.Toggle("Android", this.buildAndroid);
            this.buildIOS = EditorGUILayout.Toggle("iOS", this.buildIOS);

            if (this.buildAndroid && this.buildType == BuildType.Release)
            {
                EditorGUILayout.LabelField("Android Publishing Settings", EditorStyles.boldLabel);

                bool pressedKeystore = GUILayout.Button("Locate Keystore");

                this.androidKeystorePath = EditorGUILayout.TextField("Path To Keystore: ", this.androidKeystorePath);
                this.androidKeystorePassword = EditorGUILayout.PasswordField("Keystore Password: ", this.androidKeystorePassword);
                this.androidKeyAlias = EditorGUILayout.TextField("Key Alias: ", this.androidKeyAlias);
                this.androidKeyAliasPassword = EditorGUILayout.PasswordField("Key Alias Password: ", this.androidKeyAliasPassword);

                if (pressedKeystore)
                {
                    this.androidKeystorePath = EditorUtility.OpenFilePanel("Open Keystore", ".", "keystore");
                }
            }
            else
            {
                // Clear out our keystore info when BuildType is development
                this.androidKeystorePath = string.Empty;
                this.androidKeystorePassword = string.Empty;
                this.androidKeyAlias = string.Empty;
                this.androidKeyAliasPassword = string.Empty;
            }

            bool updatePressed = GUILayout.Button("Update");
            bool buildPressed = GUILayout.Button("Build");

            if (buildPressed || updatePressed)
            {
                try
                {
                    this.PrepareUniversalBuild();
                    if (buildPressed)
                    {
                        this.savePath = this.PromptUserForSaveLocation();
                        if (string.IsNullOrEmpty(this.savePath))
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

                this.ConfigureUniversalBuildSettings();

                if (buildPressed && this.buildIOS == false && this.buildAndroid == false)
                {
                    Debug.LogError("Tried to do build without specifying a build target.");
                    return;
                }

                if (this.buildIOS)
                {
                    this.ConfigureiOSBuildSettings();
                    if (buildPressed)
                    {
                        this.ExportiOSBuild();
                    }
                }

                if (this.buildAndroid)
                {
                    this.ConfigureAndroidBuildSettings();
                    if (buildPressed)
                    {
                        this.ExportAndroidBuild();
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

            this.ImportBuildScriptSettings();
        }

        #region Field Validation

        private void ValidateAllEditorFields()
        {
            this.ValidateStringAsFilename(this.filename);
            this.ValidateStringAsBundleIdentifier(this.bundleIdentifier);
            this.ValidateStringAsVersionNumber(this.version);
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
                new System.Text.RegularExpressions.Regex("^com\\." + this.currentSettings.CompanyName + "\\.[\\w\\d]+$");
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
            this.ValidateAllEditorFields();
        }

        private string PromptUserForSaveLocation()
        {
            string path = EditorUtility.SaveFolderPanel("Choose location to save build(s)", string.Empty, string.Empty);
            if (string.IsNullOrEmpty(path))
            {
                return string.Empty;
            }

            return path;
        }

        private void ConfigureUniversalBuildSettings()
        {
            // Configure PlayerSettings that never change //
            PlayerSettings.companyName = this.companyDisplayName;
            PlayerSettings.defaultInterfaceOrientation = UIOrientation.Portrait;
            PlayerSettings.allowedAutorotateToPortrait = true;
            PlayerSettings.allowedAutorotateToPortraitUpsideDown = true;
            PlayerSettings.allowedAutorotateToLandscapeLeft = false;
            PlayerSettings.allowedAutorotateToLandscapeRight = false;

            PlayerSettings.bundleIdentifier = this.bundleIdentifier;
            PlayerSettings.bundleVersion = this.version;

            PlayerSettings.productName = this.appName;
        }

        private void ConfigureAndroidBuildSettings()
        {
            PlayerSettings.Android.androidTVCompatibility = false;

            PlayerSettings.Android.bundleVersionCode = this.ConvertVersionStringToVersionCode(this.version);
            this.ConfigurePlatformDefinesForBuiltTarget(BuildTargetGroup.Android);

            PlayerSettings.Android.keystoreName = this.androidKeystorePath;
            PlayerSettings.Android.keystorePass = this.androidKeystorePassword;
            PlayerSettings.Android.keyaliasName = this.androidKeyAlias;
            PlayerSettings.Android.keyaliasPass = this.androidKeyAliasPassword;
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
            if (this.simulationBuildIOS)
            {
                PlayerSettings.iOS.sdkVersion = iOSSdkVersion.SimulatorSDK;
            }
            else
            {
                PlayerSettings.iOS.sdkVersion = iOSSdkVersion.DeviceSDK;
            }

            this.ConfigurePlatformDefinesForBuiltTarget(BuildTargetGroup.iOS);
        }

        #endregion

        #region Export Functions

        private void ExportAndroidBuild()
        {
            BuildPipeline.BuildPlayer(
                this.GetLevelsForBuild(),
                this.savePath + "/" + this.filename + ".apk",
                BuildTarget.Android,
                this.GetBuildOptionsForBuild());
        }

        private void ExportiOSBuild()
        {
            BuildPipeline.BuildPlayer(
                this.GetLevelsForBuild(),
                this.savePath + "/" + this.filename,
                BuildTarget.iOS, 
                this.GetBuildOptionsForBuild());
        }

        private string[] GetLevelsForBuild()
        {
            int sceneCount = UnityEngine.SceneManagement.SceneManager.sceneCount;
            var scenePaths = new string[sceneCount];
            for (int i = 0; i < scenePaths.Length; i++)
            {
                scenePaths[i] = UnityEngine.SceneManagement.SceneManager.GetSceneAt(i).path;
            }

            return scenePaths;
        }

        private BuildOptions GetBuildOptionsForBuild()
        {
            BuildOptions buildOptions;
            if (this.isDevelopmentBuild)
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