namespace RedBlueGames.Tools
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
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

        /// <summary>
        /// Loads and returns Sprites from a specified Texture asset, sorted in UnityEditor order
        /// </summary>
        /// <returns>The sprites in a texture, sorted in Unity order.</returns>
        /// <param name="texture">Texture with sprite metadata.</param>
        public static Sprite[] LoadSpritesInTextureSorted(Texture2D texture)
        {
            string path = AssetDatabase.GetAssetPath(texture);
            if (string.IsNullOrEmpty(path))
            {
                Debug.LogWarning("Can't find sprites from Texture at path: " + path);
                return null;
            }

            Sprite[] spriteArray = AssetDatabase.LoadAllAssetsAtPath(path).OfType<Sprite>().ToArray();

            List<Sprite> sortedSprites = new List<Sprite>(spriteArray);
            sortedSprites.Sort(delegate(Sprite x, Sprite y)
                {
                    return EditorUtility.NaturalCompare(x.name, y.name);
                });

            return sortedSprites.ToArray();
        }
    }
}
