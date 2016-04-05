namespace RedBlueGames.Tools
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    /// <summary>
    /// Extensions to the List collection
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Returns the last element in the list
        /// </summary>
        /// <param name="list">List instance.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        /// <returns>The last element of type T in the List</returns>
        public static T Last<T>(this List<T> list)
        {
            return list[list.Count - 1];
        }
    }
}