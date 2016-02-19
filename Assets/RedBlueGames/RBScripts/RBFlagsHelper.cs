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
using System.Collections;
using System;

namespace RedBlue
{
	// Class suggestion taken from user Dan Tao on StackOverflow: http://stackoverflow.com/questions/3261451/using-a-bitmask-in-c-sharp
	public static class RBFlagsHelper
	{
		/// <summary>
		/// Determines if the specified flag is set.
		/// </summary>
		/// <returns><c>true</c> True if set (1); otherwise, <c>false</c>.</returns>
		/// <param name="flags">Flags.</param>
		/// <param name="flag">Specific flag</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static bool IsSet<T>(T flags, T flag) where T : struct
		{
			int flagsValue = (int)(object)flags;
			int flagValue = (int)(object)flag;
			
			return (flagsValue & flagValue) != 0;
		}

		/// <summary>
		/// Set the specified flag in flags
		/// </summary>
		/// <param name="flags">Flags.</param>
		/// <param name="flag">Flag to set.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Set<T>(ref T flags, T flag) where T : struct
		{
			int flagsValue = (int)(object)flags;
			int flagValue = (int)(object)flag;
			
			flags = (T)(object)(flagsValue | flagValue);
		}

		/// <summary>
		/// Clears the specified flag in flags
		/// </summary>
		/// <param name="flags">Flags.</param>
		/// <param name="flag">Flag to clear.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void Unset<T>(ref T flags, T flag) where T : struct
		{
			int flagsValue = (int)(object)flags;
			int flagValue = (int)(object)flag;
			
			flags = (T)(object)(flagsValue & (~flagValue));
		}

		/// <summary>
		/// Shifts the flags left by n bits, looping around to the start if spilling over the Enum size
		/// </summary>
		/// <param name="flags">Flags.</param>
		/// <param name="numShifts">Number of shifts.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void ShiftLeftCircular<T>(ref T flags, int numShifts) where T : struct
		{
			ShiftCircular(ref flags, numShifts, true);
			StripLeadingBits(ref flags);
		}

		/// <summary>
		/// Shifts the flags right by n bits, looping around to the highest bit if spilling over the front
		/// </summary>
		/// <param name="flags">Flags.</param>
		/// <param name="numShifts">Number of shifts.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void ShiftRightCircular<T>(ref T flags, int numShifts) where T : struct
		{
			ShiftCircular(ref flags, numShifts, false);
			StripLeadingBits(ref flags);
		}

		/// <summary>
		/// Shifts the flags left by n bits, looping around to the start if spilling over the Enum size
		/// </summary>
		/// <param name="flags">Flags.</param>
		/// <param name="numShifts">Number of shifts.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		static void ShiftCircular<T>(ref T flags, int numShifts, bool isLeft) where T : struct
		{
			int flagsValue = (int)(object)flags;
			int numBits = Enum.GetValues (typeof(T)).Length;
			int leftShift, rightShift;
			if(isLeft) {
				leftShift = numShifts;
				rightShift = numBits - numShifts;
			} else {
				leftShift = numBits - numShifts;
				rightShift = numShifts;
			}
			flags = (T) (object) (flagsValue << leftShift | flagsValue >> rightShift);
		}

		/// <summary>
		/// Strips extra bits from the flags, leaving you with just the actual flagged bits
		/// </summary>
		/// <param name="flags">Flags.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		static void StripLeadingBits<T> (ref T flags) where T : struct {
			// Strip extra "1's"
			int flagsValue = (int)(object)flags;
			int numBits = Enum.GetValues (typeof(T)).Length;
			int takeBits = FlagNBits(numBits);
			flags = (T) (object) (flagsValue & takeBits);
		}

		/// <summary>
		/// Flag all the bits in the flags
		/// </summary>
		/// <param name="flags">Flags.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static void FlagAll<T>(ref T flags) where T : struct
		{
			int numBits = 0;
			try {
				numBits = Enum.GetValues (typeof(T)).Length;
			} catch (Exception ex) {
				throw new ArgumentException (
					string.Format (
					"FlagsHelper Error: Can't set all bits in non-enum type '{0}'.",
					typeof(T).Name
					), ex);
			}
			int flaggedBits = FlagNBits(numBits);
			flags = (T) (object) (flaggedBits);
		}

		/// <summary>
		/// Returns an int that has all bits flagged up to N (N 1's in binary)
		/// </summary>
		/// <returns>Number of bits to flag</returns>
		/// <param name="numBits">Number bits.</param>
		static int FlagNBits(int numBits)
		{
			int flags = 1;
			for(int i = 0; i < numBits - 1; i++ ) {
				flags = (flags << 1) | flags;
			}
			return flags;
		}

		/// <summary>
		/// Gets the flags as a string in binary.
		/// </summary>
		/// <returns>The flags in binary as a String.</returns>
		/// <param name="flags">Flags.</param>
		/// <typeparam name="T">The 1st type parameter.</typeparam>
		public static string ToStringInBinary<T> (T flags) where T : struct 
		{
			int flagValues = (int)(object)flags;
			string output = "";
			// Build the string in reverse order (highest order bit first)
			for (int i = Enum.GetValues(typeof(T)).Length - 1; i >= 0; i--) {
				int valueAtFlag = (int) (object) Enum.GetValues (typeof(T)).GetValue (i);
				if ((flagValues & valueAtFlag) != 0) {
					output += "1";
				} else {
					output += "0";
					}
			}

			return output;
		}

		public static void SwapBits<T> (ref T flags, T flagA, T flagB) where T : struct
		{
			int flagsValue = (int) (object) flags;
			int flagAValue = (int) (object) flagA;
			int flagBValue = (int) (object) flagB;
			bool flagAIsSet = IsSet (flags, flagA);
			bool flagBIsSet = IsSet (flags, flagB);
			// If they are the same, no need to swap
			// 1010
			if(flagAIsSet == flagBIsSet) {
				flags = (T) (object) (flagsValue);
				return;
			}
			
			if(flagAIsSet && !flagBIsSet) {
				// 1000 -> 0010
				flagsValue = flagsValue & ~flagAValue;
				flagsValue = flagsValue | flagBValue;
			} else {
				// 0010 -> 1000
				flagsValue = flagsValue | flagAValue;
				flagsValue = flagsValue & ~flagBValue;
			}
			
			flags = (T) (object) (flagsValue);
			return;
		}
	}

}