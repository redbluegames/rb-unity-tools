using UnityEngine;
using System.Collections;

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
	public AvailablePreferences preferenceToBind;

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
		if(Application.isPlaying) {
			RenderButtonForPreference ();
		}
	}

	void RenderButtonForPreference ()
	{
		if (IsPreferenceSet ()) {
			button.BackgroundSprite = activeButtonSprite;
			icon.SpriteInfo.name = activeIconSprite;
		} else {
			button.BackgroundSprite = inactiveButtonSprite;
			icon.SpriteInfo.name = inactiveIconSprite;
		}
	}

	bool IsPreferenceSet ()
	{
		if (preferenceToBind == AvailablePreferences.music) {
			return GameDataManager.Instance.GetMusicPreference ();
		} else if (preferenceToBind == AvailablePreferences.sound) {
			return GameDataManager.Instance.GetSoundPreference ();
		} else {
			return false;
		}
	}
}
