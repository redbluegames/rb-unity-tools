namespace RedBlueGames.Tools
{
    using System.Collections;
    using System.IO;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Utility functions to extend Unity's Selection class
    /// </summary>
    public static class SelectionUtility
    {
        /// <summary>
        /// Determines if is active object of the specified type.
        /// </summary>
        /// <returns><c>true</c> if the selected active object is of type T; otherwise, <c>false</c>.</returns>
        /// <typeparam name="T">The expected Type.</typeparam>
        public static bool IsActiveObjectOfType<T>()
        {
            if (Selection.activeObject == null)
            {
                return false;
            }

            return Selection.activeObject.GetType() == typeof(T);
        }

        /// <summary>
        /// Get the directory of the current selection
        /// </summary>
        /// <returns>The directory of selection.</returns>
        public static string GetDirectoryOfSelection()
        {
            // TODO THIS DOESN"T ALWAYS WORK.
            string path = "Assets";
            foreach (UnityEngine.Object obj in Selection.GetFiltered(typeof(UnityEngine.Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }

                break;
            }

            path += Path.DirectorySeparatorChar;
            return path;
        }

        /// <summary>
        /// Select a Unity Object
        /// </summary>
        /// <param name="objectToSelect">Object to select.</param>
        public static void SelectObject(UnityEngine.Object objectToSelect)
        {
            EditorUtility.FocusProjectWindow();
            Selection.activeObject = objectToSelect;
        }
    }
}