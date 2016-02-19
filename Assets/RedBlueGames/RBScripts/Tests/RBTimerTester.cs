/*****************************************************************************
 *  Copyright (C) 2014-2015 Red Blue Games, LLC
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
using RedBlueGames;

namespace RedBlueGames.Tools.Tests
{
	public class RBTimerTester : MonoBehaviour
	{
		public RBTimer myTimer1;
		public RBTimer myTimer2;

		void onTimerExpires1 ()
		{
			Debug.Log ("Done1");
		}

		void onTimerExpires2 ()
		{
			Debug.Log ("Done2");
		}
	
		// Update is called once per frame
		void Update ()
		{
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				if (myTimer1.IsRunning) {
					myTimer1.Stop ();
				} else {
					myTimer1.Start (this, onTimerExpires1);
				}
			}
			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				if (myTimer2.IsRunning) {
					myTimer2.Stop ();
				} else {
					myTimer2.Start (this, onTimerExpires2);
				}
			}
		
			if (Input.GetKeyDown (KeyCode.Space)) {
				StopAllCoroutines ();
			}
		}
	}
}
