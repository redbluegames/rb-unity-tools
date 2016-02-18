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
 * Code pattern for this class borrowed from:
 * http://wiki.unity3d.com/index.php/Singleton
 *
 **/
	public class TimeManager : Singleton<TimeManager>
	{
		// Time and Pause handling members
		int pauseRequests;
		int lowLevelPauseRequests;
	
		public bool IsPaused { get; private set; }
	
		public bool IsLowLevelPaused { get; private set; }
	
		// guarantee this will be always a singleton only - can't use the constructor!
		protected TimeManager ()
		{
		}
	
		void Update ()
		{
		}
	
		/// <summary>
		/// Pauses the game, or increments the pause counter if it's already paused.
		/// </summary>
		public void RequestPause ()
		{
			pauseRequests++;
			IsPaused = true;
			ResolveTimeScale ();
		}
	
		/// <summary>
		/// Attempts to unpause the game. Once all requests to pause have been unwound, the game
		/// unpauses.
		/// </summary>
		public void RequestUnpause ()
		{
			pauseRequests--;
			if (pauseRequests == 0) {
				IsPaused = false;
			}
			ResolveTimeScale ();
		}
	
		public void RequestLowLevelPause ()
		{
			lowLevelPauseRequests++;
			IsLowLevelPaused = true;
			ResolveTimeScale ();
		}
	
		public void RequestLowLevelUnpause ()
		{
			lowLevelPauseRequests--;
			if (lowLevelPauseRequests == 0) {
				IsLowLevelPaused = false;
			}
			ResolveTimeScale ();
		}
	
		void ResolveTimeScale ()
		{
			if (IsPaused || IsLowLevelPaused) {
				Time.timeScale = 0.0f;
			} else {
				Time.timeScale = 1.0f;
			}
		}
	}
}