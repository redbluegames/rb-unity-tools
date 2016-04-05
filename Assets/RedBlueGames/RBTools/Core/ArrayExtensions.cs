namespace RedBlueGames.Tools
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Extensions to Array type
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Returns the last element in the array
        /// </summary>
        /// <param name="array">Array instance</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>The last element of type T</returns>
        public static T Last<T>(this T[] array)
        {
            return array[array.Length - 1];
        }
    }
}