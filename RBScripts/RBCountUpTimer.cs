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

namespace RedBlueTools
{
/*
 * Simple timer that does NOT update itself. The classes using
 * this need to check back periodically to see how much time
 * has gone by.
 */
	public class RBCountUpTimer
	{
		float timeStarted;
		bool isRunning;
		const float UNSET = float.MaxValue;
	
		/*
	 * Null constructor. Set our duration to a known invalid value.
	 */
		public RBCountUpTimer ()
		{
			StopTimer ();
		}
	
		/*
	 * Start the timer.
	 */
		public void StartTimer ()
		{
			timeStarted = Time.time;
			isRunning = true;
		}
	
		/*
	 * Unsets the duration and timeStarted fields. This is important
	 * to call if you plan to reuse the timer.
	 */
		public void StopTimer ()
		{
			timeStarted = UNSET;
			isRunning = false;
		}

		/*
	 * Return if the timer has been set.
	 */
		public bool IsRunning ()
		{
			return isRunning;
		}
	
		/*
	 * Get the amount of time in seconds the timer has been running.
	 */
		public float GetTimeSinceStarted ()
		{
			WarnIfUnSet ();
			return Time.time - timeStarted;
		}
	
		/*
	 * Helper warning to tell coder they called a method that needs duration set.
	 */
		void WarnIfUnSet ()
		{
			if (!isRunning || timeStarted == UNSET) {
				Debug.LogWarning ("Tried to check time left on stopped or unset timer.");
			}
		}
	}
}