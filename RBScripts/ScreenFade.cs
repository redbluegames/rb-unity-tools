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
	[RequireComponent(typeof(dfTextureSprite))]
	public class ScreenFade : MonoBehaviour
	{

		dfTextureSprite texture;

		// Events to send to any subscribers
		public delegate void FadeCompleteHandler ();

		public event FadeCompleteHandler FadeComplete;

		bool isFading;

		void Start ()
		{
			texture = GetComponent<dfTextureSprite> ();
			texture.Opacity = 0.0f;

			dfGUIManager manager = transform.parent.gameObject.GetComponent<dfGUIManager> ();
			texture.Size = manager.GetScreenSize ();
		}

		/// <summary>
		/// Forces the fader to be the frontmost widget.
		/// </summary>
		void SendToFront ()
		{
			texture.ZOrder = 100;
		}

		public void FadeToWhite (float fadeTime)
		{
			FadeIntoColor (Color.white, fadeTime);
		}
	
		public void FadeToBlack (float fadeTime)
		{
			FadeIntoColor (Color.black, fadeTime);
		}

		void FadeIntoColor (Color fadeColor, float duration)
		{	
			texture.Color = fadeColor;
			if (isFading) {
				StopCurrentFade ();
			}
			StartCoroutine (FadeToOpacity (1.0f, duration, 0.1f));
		}

		public void FadeOut (float duration)
		{	
			if (isFading) {
				StopCurrentFade ();
			}
			StartCoroutine (FadeToOpacity (0.0f, duration, 0.0f));
		}

		/// <summary>
		/// Linearly fades to the desired opacity in the specified amount of time
		/// </summary>
		/// <returns>The to opacity.</returns>
		/// <param name="desiredOpacity">Desired opacity.</param>
		/// <param name="fadeTime">Fade Time.</param>
		/// <param name="timeToRemainFullyFaded">Time to remain at full fade value before completing.</param>
		IEnumerator FadeToOpacity (float desiredOpacity, float fadeTime, float timeToRemainFullyFaded)
		{	
			isFading = true;

			// Wait one frame before fading to fix sorting issues that occur when switching screens
			// prior to a fade out.
			SendToFront ();
			yield return null;

			float startingOpacity = texture.Opacity;
			float opacityChangePerSecond = (desiredOpacity - startingOpacity) / fadeTime;
			float expectedOpacity = startingOpacity;
			float elapsed = 0.0f;
			while (elapsed < fadeTime) {
				elapsed += Time.deltaTime;

				// write and read from a variable since texture's Opacity won't be written as often
				// as this coroutine runs, causing lagging results.
				expectedOpacity = Mathf.Clamp01 (expectedOpacity + (opacityChangePerSecond * Time.deltaTime));
				texture.Opacity = expectedOpacity;

				// Make sure the fade texture remains in front of everything else for the duration of
				// the fade
				SendToFront ();
				yield return null;
			}

			// Pause when fully faded just to give time to perceive the screen is fully obscured. 
			if (timeToRemainFullyFaded > 0.0f) {
				yield return new WaitForSeconds (timeToRemainFullyFaded);
			}
			CompleteFade ();
		}

		void CompleteFade ()
		{
			isFading = false;

			if (FadeComplete != null) {
				FadeComplete ();
			}
		}
	
		void StopCurrentFade ()
		{
			StopAllCoroutines ();
			CompleteFade ();
		}
	}
}