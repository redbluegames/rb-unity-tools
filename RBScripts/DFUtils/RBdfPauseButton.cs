using UnityEngine;
using System.Collections;

/// <summary>
/// Extends dfButton to add default Down and Up tweens.
/// </summary>
public class RBdfPauseButton : RBdfButton
{
	CustomPlatformer2DUserControl controller;
	bool pausePressed;

	public override void Update ()
	{
		base.Update ();
		if (pausePressed) {
			// Verify if player is still pressing on the screen after button down
			if (Input.touchCount == 0 && !Input.GetMouseButton (0)) {
				pausePressed = false;
			}
			controller.SkipPlayerInputThisFrame = true;
		}
	}

	/// <summary>
	/// Raises the mouse down event. Plays the down tween and resets the up tween. Also, for
	/// Pause Buttons, we need to prevent character movement on button press.
	/// </summary>
	/// <param name="args">Arguments.</param>
	internal protected override void OnMouseDown (dfMouseEventArgs args)
	{
		base.OnMouseDown (args);
		controller = FishMinigameManager.Instance.Player.GetComponent<CustomPlatformer2DUserControl> ();
		controller.SkipPlayerInputThisFrame = true;
		pausePressed = true;
	}
}
