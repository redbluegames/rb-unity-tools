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
	public class TweenFlashColor : MonoBehaviour
	{
		public Color color;
		public float duration;
		int flashCount = 0;
		Renderer[] renderers;
		Color[] originalColors;
	
		/// <summary>
		/// Determine original color for restoring after coroutines.
		/// </summary>
		void Awake ()
		{
			renderers = (Renderer[])GetComponentsInChildren<Renderer> ();
			originalColors = new Color[renderers.Length];
			for (int i = 0; i < renderers.Length; ++i) {
				if (renderers [i].material.HasProperty ("_Color")) {
					originalColors [i] = renderers [i].material.color;
				} else {
					originalColors [i] = Color.black;
				}
			}
		}

		/// <summary>
		/// Flash to color and duration specified in editor.
		/// </summary>
		public void Flash ()
		{
			StartCoroutine (FlashCoroutine (color, duration));
		}

		/// <summary>
		/// Flash to color and duration specified by code.
		/// </summary>
		/// <param name="flashColor">Flash color.</param>
		/// <param name="flashSeconds">Flash seconds.</param>
		public void Flash (Color flashColor, float flashSeconds)
		{
			StartCoroutine (FlashCoroutine (flashColor, flashSeconds));
		}

		/// <summary>
		/// Iterates over all child objects the script is attached to and changes the 
		/// color for a specified amount of time.
		/// </summary>
		/// <returns>The coroutine.</returns>
		/// <param name="flashColor">Flash color.</param>
		/// <param name="flashSeconds">Flash seconds.</param>
		IEnumerator FlashCoroutine (Color flashColor, float flashSeconds)
		{
			flashCount++;
			for (int i = 0; i < renderers.Length; ++i) {
				if (renderers [i] != null && renderers [i].material != null) {
					if (renderers [i].material.HasProperty ("_Color")) {
						renderers [i].material.color = flashColor;
					}
				}
			}
			yield return new WaitForSeconds (flashSeconds);
			if (flashCount == 1) {
				for (int i = 0; i < renderers.Length; ++i) {
					if (renderers [i] != null && renderers [i].material != null) {
						renderers [i].material.color = originalColors [i];
					}
				}
			}
			flashCount--;
		}

		/// <summary>
		/// Clean up any coroutines in progress.
		/// </summary>
		void OnDestroy ()
		{
			StopAllCoroutines ();
		}
	}
}