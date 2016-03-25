using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Tool that helps us export RedBlueTools for use in other projects, as well as for the public
/// </summary>
public class PackageExporter : UnityEditor.EditorWindow
{
    private static string assetPathName = "Assets/RedBlueGames";

    private static string Filename
    {
        get
        {
            return "RBScripts.unitypackage";
        }
    }

    private static string FilenameWithTests
    {
        get
        {
            return "RBScriptsWithTests.unitypackage";
        }
    }

    [MenuItem("RedBlueTools/PackageExporter/Export with Tests", false, 211)]
    private static void ExportRBScriptsWithTests()
    {
        ExportRBScripts(false);
    }

    [MenuItem("RedBlueTools/PackageExporter/Export", false, 200)]
    private static void ExportRBScripts()
    {
        ExportRBScripts(true);
    }

    private static void ExportRBScripts(bool excludeTests)
    {
        var subDirectories = System.IO.Directory.GetDirectories(assetPathName, "*", System.IO.SearchOption.AllDirectories);
        var directoriesToExport = new List<string>(subDirectories);

        var testDirectories = GetTestDirectories(subDirectories);
        if (excludeTests)
        {
            foreach (var testDirectory in testDirectories)
            {
                directoriesToExport.Remove(testDirectory);
            }
        }

        var allAssetPaths = new List<string>();
        foreach (var directory in directoriesToExport)
        {
            var filesInDirectory = System.IO.Directory.GetFiles(directory);
            allAssetPaths.AddRange(filesInDirectory); 
        }

        string filename = excludeTests ? Filename : FilenameWithTests;
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
            if (IsPathSubdirectoryOfThesePaths(directory, testDirectories))
            {
                testDirectories.Add(directory);
                continue;
            }

            if (IsPathToTestDirectory(directory))
            {
                testDirectories.Add(directory);
            }
        }

        return testDirectories;
    }

    private static bool IsPathSubdirectoryOfThesePaths(string path, List<string> possibleParentDirectories)
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

    private static bool IsPathToTestDirectory(string path)
    {
        return System.IO.Path.GetFileName(path) == "Tests";
    }
}