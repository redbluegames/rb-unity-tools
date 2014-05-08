/*****************************************************************************
 *  Red Blue Tools are Unity Editor utilities. Some utilities require 3rd party tools.
 *  Copyright (C) 2014 Red Blue Games, LLC
 *  
 *  This program is free software: you can redistribute it and/or modify
 *  it under the terms of the GNU General Public License as published by
 *  the Free Software Foundation, either version 2 of the License, or
 *  (at your option) any later version.
 *
 *  This program is distributed in the hope that it will be useful,
 *  but WITHOUT ANY WARRANTY; without even the implied warranty of
 *  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 *  GNU General Public License for more details.
 *
 *  You should have received a copy of the GNU General Public License
 *  along with this program.  If not, see <http://www.gnu.org/licenses/>.
 ****************************************************************************/
using UnityEngine;
using System.Collections.Generic;

namespace RedBlueTools
{
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
				int k = Random.Range (0, n + 1);
				T value = list [k];
				list [k] = list [n];
				list [n] = value;
			}
		}
	}
}