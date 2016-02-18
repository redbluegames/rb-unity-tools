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
	[RequireComponent(typeof(Light))]
	public class TweenLightFlicker : MonoBehaviour
	{
		// Turn flicker on or off
		public bool flickerOn;

		// Flicker Charactersitics
		public float lightsOutMultiplier = 0.2f;
		public float frequency = 3.0f;

		// Base intensity, automatically taken from light parameters.
		float baseIntensity;
	
		void Start ()
		{
			baseIntensity = GetComponent<Light>().intensity;
			StartCoroutine (Flicker ());
		}
	
		void Update ()
		{
		}

		IEnumerator Flicker ()
		{
			while (flickerOn) {
				GetComponent<Light>().intensity = baseIntensity * lightsOutMultiplier;
				yield return new WaitForSeconds (0.005f);
				GetComponent<Light>().intensity = baseIntensity;
				yield return new WaitForSeconds (Random.Range (0, frequency));
			}
		}
	}
}