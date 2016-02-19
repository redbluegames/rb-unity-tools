using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class ArrayExtensions
{
	/// <summary>
	/// Returns the last element in the array
	/// </summary>
	/// <param name="array">Array.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T Last<T> (this T[] array)
	{
		return array [array.Length - 1];
	}

	/// <summary>
	/// Returns the last element in the list
	/// </summary>
	/// <param name="list">List.</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T Last<T> (this List<T> list)
	{
		return list [list.Count - 1];
	}
}
