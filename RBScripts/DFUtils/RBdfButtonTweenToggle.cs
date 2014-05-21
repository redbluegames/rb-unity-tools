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
/// <summary>
/// Adds tweens that play when a button is toggled on and off to an existing RBdfButton
/// </summary>
///
	[RequireComponent (typeof(RBdfButton))]
	public class RBdfButtonTweenToggle : MonoBehaviour
	{
		// DF Controls to operate on
		RBdfButton button;
		bool isCollectionShown;
		public dfTweenPlayableBase showCollectionTween;
		public dfTweenPlayableBase hideCollectionTween;
		public dfTweenPlayableBase showCollectionInstantTween;
		public dfTweenPlayableBase hideCollectionInstantTween;

		void Awake ()
		{
			button = GetComponent<RBdfButton> ();
			isCollectionShown = false;

			if (showCollectionTween == null) {
				Debug.LogError ("No show tween linked to ButtonCollection.");
			}
			if (hideCollectionTween == null) {
				Debug.LogError ("No hide tween linked to ButtonCollection.");
			}
		}

		void OnEnable ()
		{
			button.Click += HandleClick;
			button.ControlShown += OnControlShown;
		}

		void OnDisable ()
		{
			button.Click -= HandleClick;
			button.ControlShown -= OnControlShown;
		}

		void HandleClick (dfControl control, dfMouseEventArgs mouseEvent)
		{
			isCollectionShown = !isCollectionShown;
			if(isCollectionShown) {
				ShowCollection ();
			} else {
				HideCollection ();
			}
		}

		void OnControlShown (dfControl control, bool isShown)
		{
			if (Application.isPlaying) {
				if(isCollectionShown) {
					ShowCollectionInstantly ();
				} else {
					HideCollectionInstantly ();
				}
			}
		}

		void ShowCollection()
		{
			StopAllTweens ();
			PlayShowTweens (false);
		}

		void ShowCollectionInstantly ()
		{
			StopAllTweens ();
			PlayShowTweens (true);
		}

		void HideCollection()
		{
			StopAllTweens ();
			PlayHideTweens (false);
		}

		void HideCollectionInstantly()
		{
			StopAllTweens ();
			PlayHideTweens (true);
		}

		void StopAllTweens ()
		{
			showCollectionTween.Stop ();
			hideCollectionTween.Stop ();

			if (showCollectionInstantTween != null) {
				showCollectionInstantTween.Stop ();
			}
			if (hideCollectionInstantTween != null) {
				hideCollectionInstantTween.Stop ();
			}
		}

		void PlayShowTweens (bool fastForward)
		{
			if (fastForward && showCollectionInstantTween != null) {
				showCollectionInstantTween.Play ();
			} else { 
				showCollectionTween.Play ();
			}
		}

		void PlayHideTweens (bool fastForward)
		{
			if (fastForward && hideCollectionInstantTween != null) {
				hideCollectionInstantTween.Play ();
			} else { 
				hideCollectionTween.Play ();
			}
		}
	}
}
