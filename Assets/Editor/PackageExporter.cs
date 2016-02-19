using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class PackageExporter : UnityEditor.EditorWindow
{

	static string assetPathName = "Assets/RedBlueGames";

	static string Filename
	{
		get
		{
			return "RBScripts.unitypackage";
		}
	}

	static string FilenameWithTests
	{
		get
		{
			return "RBScriptsWithTests.unitypackage";
		}
	}

	[MenuItem ("Assets/PackageExporter/Export with Tests", false, 111)]
	public static void ExportRBScriptsWithTests ()
	{
		ExportRBScripts (false);
	}

	[MenuItem ("Assets/PackageExporter/Export", false, 100)]
	public static void ExportRBScripts ()
	{
		ExportRBScripts (true);
	}

	static void ExportRBScripts (bool excludeTests)
	{
		var subDirectories = System.IO.Directory.GetDirectories (assetPathName, "*", 
			                     System.IO.SearchOption.AllDirectories);
		var directoriesToExport = new List<string> (subDirectories);

		var testDirectories = GetTestDirectories (subDirectories);
		if (excludeTests) {
			foreach (var testDirectory in testDirectories) {
				directoriesToExport.Remove (testDirectory);
			}
		}

		var allAssetPaths = new List<string> ();
		foreach (var directory in directoriesToExport) {
			var filesInDirectory = System.IO.Directory.GetFiles (directory);
			allAssetPaths.AddRange (filesInDirectory); 
		}

		string filename = excludeTests ? Filename : FilenameWithTests;
		AssetDatabase.ExportPackage (allAssetPaths.ToArray (), filename,
			ExportPackageOptions.IncludeDependencies |
			ExportPackageOptions.Interactive);
	}

	static List<string> GetTestDirectories (string[] directories)
	{
		var testDirectories = new List<string> ();
		foreach (var directory in directories) {
			if (IsPathSubdirectoryOfThesePaths (directory, testDirectories)) {
				testDirectories.Add (directory);
				continue;
			}
			if (IsPathToTestDirectory (directory)) {
				testDirectories.Add (directory);
			}
		}
		return testDirectories;
	}

	static bool IsPathSubdirectoryOfThesePaths (string path, List<string> possibleParentDirectories)
	{
		foreach (var possibleParentDirectory in possibleParentDirectories) {
			if (path.Contains (possibleParentDirectory)) {
				return true;
			}
		}
		return false;
	}

	static bool IsPathToTestDirectory (string path)
	{
		return System.IO.Path.GetFileName (path) == "Tests";
	}
}