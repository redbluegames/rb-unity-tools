using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class RBRandom
{
	/*
	 * Rolls a random number and returns true at a rate in accordance with the passed in percent.
	 */
	public static bool PercentageChance (int percent)
	{
		int rand = Random.Range (0, 100);
		return rand < percent;
	}

	/*
	 * Rolls a random number and returns true at a rate in accordance with the passed in percent.
	 */
	public static bool PercentageChance (float percent)
	{
		float rand = Random.Range (0, 100.0f);
		return rand < percent;
	}
	
	/*
	 * Shuffle using the Fisher-Yates shuffle.
	 * http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
	 */
	public static void Shuffle<T> (List<T> list)
	{
		int n = list.Count;
		while (n > 1) {
			n--;
			int k = Random.Range(0, n+1);
			T value = list [k];
			list [k] = list [n];
			list [n] = value;
		}
	}
}