namespace RedBlueGames.Tools
{
    using System.Collections;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Utilities for Unity's built in AssetDatabase class
    /// </summary>
    public static class AssetDatabaseUtility
    {
        /// <summary>
        /// Gets just the directory name of an asset including DirectorySeparator. Does not get the path.
        /// </summary>
        /// <returns>The asset's directory name including DiectorySeparatorChar</returns>
        /// <param name="asset">Asset in a directory.</param>
        public static string GetAssetDirectory(UnityEngine.Object asset)
        {
            string path = AssetDatabase.GetAssetPath(asset);
            return Path.GetDirectoryName(path) + Path.DirectorySeparatorChar;
        }

        /// <summary>
        /// Create and Save an object in the AssetDatabase and then select it.
        /// </summary>
        /// <param name="objectToSave">Object to save.</param>
        /// <param name="path">Path for new file, not including filename</param>
        /// <param name="filename">Filename including file extension.</param>
        public static void SaveAndSelectObject(UnityEngine.Object objectToSave, string path, string filename)
        {
            SaveObject(objectToSave, path, filename);
            SelectionUtilities.SelectObject(objectToSave);
        }

        /// <summary>
        /// Create and Save the object in the AssetDatabase
        /// </summary>
        /// <param name="objectToSave">Object to save.</param>
        /// <param name="path">Path for the new file, not including filename</param>
        /// <param name="filename">Filename including file extension</param>
        public static void SaveObject(UnityEngine.Object objectToSave, string path, string filename)
        {
            string fullpath = path + filename;

            AssetDatabase.CreateAsset(objectToSave, fullpath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }
    }
}
