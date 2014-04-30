using UnityEngine;
using System.Collections;

/// <summary>
/// A custom Tween written on top of Diakon Forge GUI that increments a label's
/// numeric text across a provided amount of time. Can be used like other
/// dfGUI tweens.
/// </summary>
public class dfTweenPlayParticles : dfTweenPlayableBase
{
	public string tweenName;
	public ParticleSystem particleSystemToTween;
	bool isTweening;

	void PlayParticles ()
	{
		if (particleSystemToTween != null) {
			particleSystemToTween.Play ();
		} else {
			Debug.LogError ("StarburstSystem not wired up in editor. " +
				"Could not show particle effect.");
		}
	}

	#region dfTweenPlayableBase abstract methods

	/// <summary>
	/// Starts the tween animation
	/// </summary>
	public override void Play ()
	{
		PlayParticles ();
	}

	/// <summary>
	/// Stops the tween animation
	/// </summary>
	public override void Stop ()
	{
		particleSystemToTween.Clear ();
		particleSystemToTween.Stop ();
		isTweening = false;
	}

	/// <summary>
	/// Resets the tween animation to the beginning
	/// </summary>
	public override void Reset ()
	{
		particleSystemToTween.Clear ();
		particleSystemToTween.Stop ();
		isTweening = false;
	}

	/// <summary>
	/// Returns TRUE if the tween animation is currently playing
	/// </summary>
	/// <value><c>true</c> if this instance is playing; otherwise, <c>false</c>.</value>
	public override bool IsPlaying {
		get {
			return isTweening;
		}
	}

	/// <summary>
	/// Gets or sets the user-defined name of the Tween, which is 
	/// useful to the developer at design time when there are 
	/// multiple tweens on a single GameObject
	/// </summary>
	/// <value>The name of the tween.</value>
	public override string TweenName {
		get {
			return tweenName;
		}
		set {
			tweenName = value;
		}
	}

	#endregion

	#region Tweening Code

	#endregion
}
