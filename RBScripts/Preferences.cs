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
	public class Preferences
	{
	
		public enum AvailablePreferences
		{
			none,
			sound,
			music
		}

		// Strings used for Preference Keys
		const string SOUND_ON_PREF = "soundOn";
		const string MUSIC_ON_PREF = "musicOn";
	
		public static void SetMusicPreference (bool shouldPlayMusic)
		{
			SetPreferenceFromBool (MUSIC_ON_PREF, shouldPlayMusic);
		}
	
		public static void SetSoundPreference (bool shouldPlaySound)
		{
			SetPreferenceFromBool (SOUND_ON_PREF, shouldPlaySound);
		}
	
		public static bool GetMusicPreference ()
		{
			return GetBoolPreference (MUSIC_ON_PREF, true);
		}
	
		public static bool GetSoundPreference ()
		{
			return GetBoolPreference (SOUND_ON_PREF, true);
		}
	
		static void SetPreferenceFromBool (string key, bool boolToStore)
		{
			int prefValue = boolToStore ? 1 : 0;
			PlayerPrefs.SetInt (key, prefValue);
		}

		static bool GetBoolPreference (string key, bool defaultValue)
		{
			int defaultAsInt = defaultValue ? 1 : 0;
			bool prefAsBool = PlayerPrefs.GetInt (key, defaultAsInt) == 1 ? true : false;
			return prefAsBool;
		}
	}
}