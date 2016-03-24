namespace RedBlueGames.Tools
{
    using System.Collections;
    using System.IO;
    using RedBlueGames.Tools;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Utility functions for ScriptableObjects.
    /// </summary>
    public static class ScriptableObjectUtility
    {
        /// <summary>
        /// Creates and saves the Scriptable Object as an asset in the Editor. Adapted from Unity3D wiki:
        /// http://wiki.unity3d.com/index.php?title=CreateScriptableObjectAsset
        /// </summary>
        /// <typeparam name="T">The type of the Scriptable Object to create.</typeparam>
        public static T CreateAsset<T>() where T : ScriptableObject
        {
            T scriptableObjectInstance = ScriptableObject.CreateInstance<T>();

            string currentPath = SelectionUtilities.GetDirectoryOfSelection();
            string path = currentPath;
            string typeName = typeof(T).ToString();
            string typeWithoutNamespace = typeName.Split('.').Last();
            string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath(path + "New " + typeWithoutNamespace + ".asset");

            AssetDatabaseUtility.SaveAndSelectObject(scriptableObjectInstance, currentPath, Path.GetFileName(uniqueAssetPath));

            return scriptableObjectInstance;
        }
    }
}