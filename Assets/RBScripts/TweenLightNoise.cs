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
	[RequireComponent(typeof(Light))]
	public class TweenLightNoise : MonoBehaviour
	{
		// Turn noise on or off
		public bool noiseOn;

		// Noise characteristics
		public float speed = 2.0f;
		public float magnitude = 1.0f;

		// Facilitate noise generation
		float baseIntensity;
		float randomStart;

		// Use this for initialization
		void Start ()
		{
			baseIntensity = GetComponent<Light>().intensity;
			randomStart = Random.Range (-1000.0f, 1000.0f);
		}

		void Update ()
		{
			if (noiseOn) {
				float progress = (randomStart + Time.time) * speed;
				float flickerNoise = Mathf.PerlinNoise (progress, 0) * magnitude;
				GetComponent<Light>().intensity = baseIntensity + flickerNoise;
			}
		}
	}
}