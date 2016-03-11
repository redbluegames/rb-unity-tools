namespace RedBlueGames.Tools
{
    using System.Collections;
    using UnityEditor;
    using UnityEngine;

    public static class SelectionUtilities
    {
        public static bool IsActiveObjectOfType<T>()
        {
            if (Selection.activeObject == null)
            {
                return false;
            }

            return Selection.activeObject.GetType() == typeof(T);
        }
    }
}