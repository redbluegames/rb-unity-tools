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
	[RequireComponent (typeof(Renderer))]
	public class TweenFadeInOut : MonoBehaviour
	{
		float DEFAULT_FADE_TIME = 0.0f;
		float fadeTime = 0.5f;
		float targetAlpha = 1.0f;

		public bool isFading { get; private set; }

		/// <summary>
		/// Fade the object in at the default speed.
		/// </summary>
		public void FadeIn ()
		{
			FadeIn (DEFAULT_FADE_TIME);
		}

		/// <summary>
		/// Fades the object in, taking a provided amount of time to fade.
		/// </summary>
		/// <param name="timeToFade">Time to fade.</param>
		public void FadeIn (float timeToFade)
		{
			SetNewFadeTarget (1.0f, timeToFade);
		}
	
		/// <summary>
		/// Fade the object out at the default speed.
		/// </summary>
		public void FadeOut ()
		{
			FadeOut (DEFAULT_FADE_TIME);
		}

		/// <summary>
		/// Fades the object out, taking a provided amount of time to fade.
		/// </summary>
		/// <param name="timeToFade">Time to fade.</param>
		public void FadeOut (float timeToFade)
		{
			SetNewFadeTarget (0.0f, timeToFade);
		}

		/// <summary>
		/// Fade to a provided alpha and over a provided amount of time.
		/// </summary>
		/// <param name="targetAlpha">Target alpha.</param>
		/// <param name="fadeTime">Fade time.</param>
		void SetNewFadeTarget (float targetAlpha, float fadeTime)
		{
			this.fadeTime = fadeTime;
			this.targetAlpha = targetAlpha;

			// Start the corountine if we need to fade to reach the new target and it's not already running.
			if (!IsAlphaAtTarget () && !isFading) {
				StartCoroutine (Fade ());
			}
		}

		/// <summary>
		/// Fade to the current target. This method can only be running once but the
		/// target and time are allowed to change out from under it.
		/// </summary>
		IEnumerator Fade ()
		{
			isFading = true;
			Color currentColor = gameObject.renderer.material.color;
		
			while (!IsAlphaAtTarget()) {
				// Recalculate the rate of change in case targets change mid fade
				float alphaRemaining = targetAlpha - currentColor.a;
				float MAX_ALPHA = 1.0f;
				float rate = (MAX_ALPHA / fadeTime) * Mathf.Sign (alphaRemaining);

				currentColor.a = Mathf.Clamp01 (currentColor.a + rate * Time.deltaTime);
				gameObject.renderer.material.color = currentColor;
				yield return null;
			}
			isFading = false;
		}

		/// <summary>
		/// Determines whether the gameobject's alpha is at target value.
		/// </summary>
		/// <returns><c>true</c> if this instance is alpha at target; otherwise, <c>false</c>.</returns>
		private bool IsAlphaAtTarget ()
		{
			return Mathf.Approximately (gameObject.renderer.material.color.a, targetAlpha);
		}

	}
}