using UnityEngine;
using UnityEditor;
using System.Collections;

public class PackageExporter : UnityEditor.EditorWindow {

	static string assetPathName = "Assets/RBScripts";

	[MenuItem ("PackageExporter/Export as Package")]
	public static void ExportPRBScripts ()
	{
		AssetDatabase.ExportPackage (assetPathName, "RBScripts.unitypackage", ExportPackageOptions.Recurse |
		                             ExportPackageOptions.IncludeDependencies);
		Debug.Log ("Exported!");
	}
}
