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
	public class RBCountDownTimer : MonoBehaviour
	{
		public float TimeStarted { get; private set; }

		public float Duration { get; private set; }

		public float TimeRemaining { get; private set; }

		public bool IsRunning  { get; private set; }

		const float UNSET = float.MaxValue;

		public delegate void CompleteEvent ();

		public CompleteEvent OnComplete;

		public RBCountDownTimer ()
		{
			InitializeToDefaults ();
		}
	
		void InitializeToDefaults ()
		{
			IsRunning = false;
			Duration = UNSET;
			TimeStarted = UNSET;
			TimeRemaining = 0.0f;
		}

		/// <summary>
		/// Starts the timer. Fires off OnComplete when done.
		/// </summary>
		/// <param name="desiredDuration">The desired duration for the timer in game seconds.</param>
		public void StartTimer (float desiredDuration)
		{
			// Stop the timer if it's running
			StopAllCoroutines ();

			StartCoroutine (CountdownForDuration (desiredDuration));
		}

		IEnumerator CountdownForDuration (float desiredDuration)
		{
			IsRunning = true;
			Duration = desiredDuration;
			TimeStarted = Time.time;

			TimeRemaining = Duration;
			while (TimeRemaining >= 0.0f) {
				TimeRemaining -= Time.deltaTime;
				yield return null;
			}

			CompleteCountdown ();
		}

		/// <summary>
		/// End a completed timer.
		/// </summary>
		void CompleteCountdown ()
		{
			if (OnComplete != null) {
				OnComplete ();
			}

			InitializeToDefaults ();
		}

		/// <summary>
		/// Stop the timer without firing off the complete event.
		/// </summary>
		public void StopTimer ()
		{
			InitializeToDefaults ();
			StopAllCoroutines ();
		}
	}

}