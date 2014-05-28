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
	public class CameraShake : MonoBehaviour
	{
		// Position offsets from the camera
		public Vector3 DefaultPosition;
		Vector3 shakeOffset;

		void Start ()
		{
			SetupDefaults ();
		}

		/*
	 * Helper function that wires up all default settings for
	 * the camera.
	 */
		void SetupDefaults ()
		{
			// Resolve offset so that we can get the angle to the target
			DefaultPosition = camera.transform.position;
		}

		void LateUpdate ()
		{
			ResolveOffsets ();
		}

		/*
	 * Calculate where the camera will display after having performed other
	 * game updates. Things like shake and camera follow should take effect now.
	 */
		void ResolveOffsets ()
		{
			camera.transform.position = DefaultPosition + shakeOffset;
		}

	#region Shake Functionality
		// -------------------------------------------------------------------------
		/*
	 * Shake the camera with a given speed, duration, and magnitude using a Perlin
	 * noised utility.
	 */
		public void Shake (float speed, float duration, float magnitude)
		{
			StartCoroutine (SetShakeOffset (speed, duration, magnitude));
		}

		/*
	 * Shake camera, setting temporary offset variables which will be applied on
	 * LateUpdate.
	 * 
	 * Resource:
	 * http://unitytipsandtricks.blogspot.com/2013/05/camera-shake.html
	 */
		IEnumerator SetShakeOffset (float speed, float duration, float magnitude)
		{
			float randomStart = Random.Range (-1000.0f, 1000.0f);
			float elapsed = 0.0f;
			while (elapsed < duration) {
				elapsed += Time.deltaTime;			
			
				float percentComplete = elapsed / duration;			

				// We want to reduce the shake from full power to 0 starting half way through
				float damper = 1.0f - Mathf.Clamp (2.0f * percentComplete - 1.0f, 0.0f, 1.0f);
			
				// Calculate the noise parameter starting randomly and going as fast as speed allows
				float alpha = randomStart + speed * percentComplete;
			
				// map noise to [-1, 1] (noise generated from 0-1)
				float x = Mathf.PerlinNoise (alpha, 0) * 2.0f - 1.0f;
				float y = Mathf.PerlinNoise (0, alpha) * 2.0f - 1.0f;
			
				x *= magnitude * damper;
				y *= magnitude * damper;

				shakeOffset = new Vector3 (x, y, 0);
				yield return null;
			}
		}
	#endregion
	}
}
