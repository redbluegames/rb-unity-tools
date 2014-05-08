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

namespace RedBlueTools
{
/*
 * Simple timer that does NOT update itself. The classes using
 * this need to check back periodically to see if time is up.
 * Then, if the timer is needed again, reset it.
 */
	[System.Serializable]
	public class CountDownTimer
	{
		float timeStarted;
		float duration;
		bool isRunning;
		const float UNSET = float.MaxValue;
	
		/*
	 * Null constructor. Set our duration to a known invalid value.
	 */
		public CountDownTimer ()
		{
			isRunning = false;
			duration = UNSET;
			timeStarted = UNSET;
		}
	
		/*
	 * Start the timer.
	 */
		public void StartTimer (float desiredDuration)
		{
			duration = desiredDuration;
			timeStarted = Time.time;
			isRunning = true;
		}
	
		/*
	 * Unsets the duration and timeStarted fields. This is important
	 * to call if you plan to reuse the timer.
	 */
		public void StopTimer ()
		{
			duration = UNSET;
			timeStarted = UNSET;
			isRunning = false;
		}
	
		/*
	 * Check if time is up for the timer. If no duration has been specified return true.
	 * If time isn't up, return false.
	 */
		public bool IsTimeUp ()
		{
			if (duration == UNSET) {
				return true;
			}
			return GetTimeLeft () <= 0;
		}
	
		/*
	 * Return if the timer has been set.
	 */
		public bool IsRunning ()
		{
			return isRunning;
		}
	
		/*
	 * Get the time remaining on the timer.
	 */
		public float GetTimeLeft ()
		{
			WarnIfUnSet ();
			return duration - (Time.time - timeStarted);
		}
	
		/*
	 * Helper warning to tell coder they called a method that needs duration set.
	 */
		void WarnIfUnSet ()
		{
			if (!isRunning || duration == UNSET || timeStarted == UNSET) {
				Debug.LogWarning ("Tried to check time left on stopped or unset timer.");
			}
		}
	}
}