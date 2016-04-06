using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using RedBlueGames.Tools;

/// <summary>
/// Tool that helps us export RedBlueTools for use in other projects, as well as for the public
/// </summary>
public class RBPackageExporter : UnityEditor.EditorWindow
{
    private static string companyPath = "Assets/RedBlueGames";
    private static List<RBAsset> rbAssets;
    private static string packageExtension = ".unitypackage";
    private static string testPackageSuffix = "WithTests";

    private bool includeTests;

    [MenuItem("Assets/RBPackage Exporter")]
    private static void ExportRBScriptsWithTests()
    {
        EditorWindow.GetWindow<RBPackageExporter>(false, "RBPackage Exporter", true);
    }

    private void OnEnable()
    {
        rbAssets = new List<RBAsset>();
        FindAssetsInCompanyFolder();
    }

    private void FindAssetsInCompanyFolder()
    {
        foreach (var subdirectory in System.IO.Directory.GetDirectories(companyPath))
        {
            var splitSubdirectory = subdirectory.Split(System.IO.Path.DirectorySeparatorChar);
            string folderName = splitSubdirectory.Last();
            rbAssets.Add(new RBAsset()
                {
                    AssetName = folderName,
                    IsSelected = false
                });
        }
    }

    private void OnGUI()
    {
        EditorGUILayout.HelpBox(
            "This tool Allows quick export of specific RedBlueGames custom assets. It also allows optional export of Tests folders.",
            MessageType.None);
        EditorGUILayout.LabelField("Asset Packages to export:", EditorStyles.boldLabel);
        EditorGUILayout.BeginVertical();
        for (int i = 0; i < rbAssets.Count; i++)
        {
            rbAssets[i].IsSelected = EditorGUILayout.Toggle(
                rbAssets[i].AssetName,
                rbAssets[i].IsSelected);
        }
        EditorGUILayout.EndVertical();

        // Check if any assets are selected.
        bool atLeastOnePackageSelected = false;
        foreach (var assetPackage in rbAssets)
        {
            if (assetPackage.IsSelected)
            {
                atLeastOnePackageSelected = true;
                break;
            }
        }

        EditorGUILayout.Separator();
        includeTests = EditorGUILayout.Toggle("Include Tests", includeTests);

        EditorGUI.BeginDisabledGroup(!atLeastOnePackageSelected);
        if (GUILayout.Button("Export"))
        {
            ExportAllPackages(includeTests);
        }
        EditorGUI.EndDisabledGroup();

        if (!atLeastOnePackageSelected)
        {
            EditorGUILayout.HelpBox(
                "No packages selected to export. Select at least one asset Package.",
                MessageType.Warning);
        }
    }

    private static void ExportAllPackages(bool includeTests)
    {
        foreach (var asset in rbAssets)
        {
            if (asset.IsSelected)
            {
                ExportRBScripts(asset, includeTests);
            }
        }
    }

    private static void ExportRBScripts(RBAsset assetToExport, bool includeTests)
    {
        var subDirectories = System.IO.Directory.GetDirectories(companyPath, "*", System.IO.SearchOption.AllDirectories);
        var directoriesToExport = new List<string>(subDirectories);

        var testDirectories = GetTestDirectories(subDirectories);
        if (includeTests)
        {
            foreach (var testDirectory in testDirectories)
            {
                directoriesToExport.Remove(testDirectory);
            }
        }

        // Do not export the other packages
        foreach (var asset in rbAssets)
        {
            if (assetToExport.AssetName != asset.AssetName)
            {
                string assetPath = companyPath + System.IO.Path.DirectorySeparatorChar + asset.AssetName;
                directoriesToExport.Remove(assetPath);

                var subdirectoriesOfAsset = System.IO.Directory.GetDirectories(assetPath, "*", System.IO.SearchOption.AllDirectories);
                foreach (var subdirectory in subdirectoriesOfAsset)
                {
                    directoriesToExport.Remove(subdirectory);
                }
            }
        }

        var allAssetPaths = new List<string>();
        foreach (var directory in directoriesToExport)
        {
            var filesInDirectory = System.IO.Directory.GetFiles(directory);
            allAssetPaths.AddRange(filesInDirectory); 
        }

        if (allAssetPaths.Count == 0)
        {
            Debug.Log("No assets to export. Will not export asset package: " + assetToExport.AssetName);
            return;
        }

        string filename = string.Concat(assetToExport.AssetName, includeTests ? testPackageSuffix : string.Empty, packageExtension);
        AssetDatabase.ExportPackage(
            allAssetPaths.ToArray(),
            filename,
            ExportPackageOptions.IncludeDependencies | ExportPackageOptions.Interactive);
    }

    private static List<string> GetTestDirectories(string[] directories)
    {
        var testDirectories = new List<string>();
        foreach (var directory in directories)
        {
            if (IsDirectoryAChildOfAnyOfThese(directory, testDirectories))
            {
                testDirectories.Add(directory);
                continue;
            }

            if (IsTestDirectory(directory))
            {
                testDirectories.Add(directory);
            }
        }

        return testDirectories;
    }

    private static bool IsDirectoryAChildOfAnyOfThese(string path, List<string> possibleParentDirectories)
    {
        foreach (var possibleParentDirectory in possibleParentDirectories)
        {
            if (path.Contains(possibleParentDirectory))
            {
                return true;
            }
        }

        return false;
    }

    private static bool IsTestDirectory(string path)
    {
        return System.IO.Path.GetFileName(path) == "Tests";
    }

    private class RBAsset
    {
        public string AssetName { get; set; }

        public bool IsSelected { get; set; }
    }
}