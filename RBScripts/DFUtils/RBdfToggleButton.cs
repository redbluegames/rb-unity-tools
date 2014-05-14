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
/// Extends dfButton to add default Down and Up tweens.
/// </summary>
///
	[RequireComponent (typeof(RBdfButton))]
	public class RBdfToggleButton : MonoBehaviour
	{
		// DF Controls to operate on
		RBdfButton button;
		dfSprite icon;

		// Preference Type
		public string preferenceKey;
//		public Preferences.AvailablePreferences preferenceToBind;

		// Sprites
		public string activeButtonSprite;
		public string inactiveButtonSprite;
		public string activeIconSprite;
		public string inactiveIconSprite;

		void Awake ()
		{
			button = GetComponent<RBdfButton> ();
			icon = GetComponentInChildren<dfSprite> ();
		}

		void OnEnable ()
		{
			RenderButtonForPreference ();
			button.MouseUp += OnMouseUp;
			button.ControlShown += OnControlShown;
		}

		void OnDisable ()
		{
			button.MouseUp -= OnMouseUp;
			button.ControlShown -= OnControlShown;
		}

		void OnMouseUp (dfControl control, dfMouseEventArgs mouseEvent)
		{
			RenderButtonForPreference ();
		}

		void OnControlShown (dfControl control, bool isShown)
		{
			if (Application.isPlaying) {
				RenderButtonForPreference ();
			}
		}

		void RenderButtonForPreference ()
		{
			if (IsPreferenceSet (preferenceKey)) {
				button.BackgroundSprite = activeButtonSprite;
				icon.SpriteName = activeIconSprite;
			} else {
				button.BackgroundSprite = inactiveButtonSprite;
				icon.SpriteName = inactiveIconSprite;
			}
		}

		bool IsPreferenceSet (string key)
		{
			if (PlayerPrefs.HasKey (key)) {
				return PlayerPrefs.GetInt (key, 1) >= 1 ? true : false;
			} else {
				return false;
			}
		}
	}
}
