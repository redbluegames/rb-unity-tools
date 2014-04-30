using UnityEngine;
using System.Collections;

public class Preferences {
	
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
