using UnityEngine;
using System.Collections;

/// <summary>
/// A custom Tween written on top of Diakon Forge GUI that increments a label's
/// numeric text across a provided amount of time. Can be used like other
/// dfGUI tweens.
/// </summary>
public class dfTweenCountToNumber : dfTweenPlayableBase
{
	// Tween Configurations
	public string tweenName;
	public dfLabel label;
	public int targetNumber;
	public string numberUnit = "m";
	public bool playSounds;
	AudioSource counterAudioSource;
	public int startNumber;
	public bool SyncOnRun;
	public DurationType tweenDurationType;
	[HideInInspector]
	public float pointsPerSec;
	[HideInInspector]
	public float fixedDurationSecs;
	const float MAX_DURATION = 2;
	
	public string currentNumberWithUnit { get; private set; }

	// Defines whether tween is a fixed duration or flexbile, based on speed
	public enum DurationType
	{
		fixedDuration,
		pointsPerSecond
	}

	// Facilitate tweens waiting on this tween to complete
	public delegate void TweenCompleteHandler ();

	public event TweenCompleteHandler ReadyForNextTween;

	// Tween status and progress variables
	int currentNumber;
	int originalNumber;
	bool isTweening;
	
	/// <summary>
	/// Awake is called by the Unity engine when the script instance is being loaded.
	/// </summary>
	void Awake ()
	{
		if (Application.isPlaying) {
			AddAudioSource ();
		}
	}
	
	/// <summary>
	/// This function is called by the Unity engine when the control will be destroyed.
	/// </summary>
	void OnDestroy ()
	{
		if (Application.isPlaying) {
			DestroyCounterAudioSource ();
		}
	}

	#region dfTweenPlayableBase abstract methods
	
	/// <summary>
	/// Starts the tween animation
	/// </summary>
	public override void Play ()
	{
		if (SyncOnRun) {
			string currentText = GetTextFromObject ();
			// Trim out the unit, if it has one.
			if (numberUnit != null) {
				currentText = currentText.TrimEnd (numberUnit.ToCharArray ());
			}
			if (!int.TryParse (currentText, out currentNumber)) {
				Debug.LogError (string.Format ("Cannot Play CountToNumberTween because label " +
					"text ('{0}') could not be parsed", currentText));
				return;
			}
			startNumber = currentNumber;
		}

		if (!isTweening) {
			originalNumber = startNumber;
			StartCoroutine (CountUpOrDown ());
		}
	}
	
	/// <summary>
	/// Stops the tween animation
	/// </summary>
	public override void Stop ()
	{
		isTweening = false;
		StopCoroutine ("CountUpOrDown");
	}
	
	/// <summary>
	/// Resets the tween animation to the beginning
	/// </summary>
	public override void Reset ()
	{
		SetCurrentNumber (startNumber);
		AssignCurrentNumberToLabel ();
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
	
	/// <summary>
	/// Iterate up or down to the target number over a time set in the editor.
	/// </summary>
	IEnumerator CountUpOrDown ()
	{
		// Advanced Configurations
		const float minTweenTime = 0.05f;

		float tweenDuration = CalculateTweenDuration ();
		if (Mathf.Approximately (tweenDuration, 0)) {
			SetCurrentNumber (targetNumber);
			AssignCurrentNumberToLabel ();
		}

		// Perform our tween for set duration
		isTweening = true;
		float elapsed = 0;
		float timeOfPreviousIteration = Time.time;
		int previousNumber = originalNumber;
		while (elapsed < tweenDuration) {
			elapsed += Time.time - timeOfPreviousIteration;
			timeOfPreviousIteration = Time.time;
			previousNumber = currentNumber;
			SetCurrentNumber ((int)Mathf.Lerp (originalNumber, targetNumber, elapsed / tweenDuration));
			AssignCurrentNumberToLabel ();
			if (playSounds && currentNumber != previousNumber) {
				// ToDo: Play a count sound
				//AudioManager.Instance.PlayCountSound (counterAudioSource);
			}
			yield return new WaitForSeconds (minTweenTime);
		}
		// Fire off our event that says the tween is complete
		if (currentNumber == targetNumber && ReadyForNextTween != null) {
			ReadyForNextTween ();
		}
		isTweening = false;
	}

	/// <summary>
	/// Calculates the duration of the tween depending on the Tween Duration Type.
	/// </summary>
	/// <returns>The tween duration.</returns>
	float CalculateTweenDuration ()
	{
		float duration;
		if (tweenDurationType == DurationType.pointsPerSecond) {
			if (Mathf.Approximately (pointsPerSec, 0)) {
				return 0;
			}
			duration = (targetNumber - startNumber) / pointsPerSec;
		} else {
			duration = fixedDurationSecs;
		}
		return Mathf.Min (duration, MAX_DURATION);
	}

	/// <summary>
	/// Sets the current number to the assigned value and updates the member that represents it
	/// as a string.
	/// </summary>
	/// <param name="number">Number.</param>
	void SetCurrentNumber (int number)
	{
		currentNumber = number;
		currentNumberWithUnit = currentNumber.ToString () + numberUnit;
	}

	/// <summary>
	/// Assigns the current number to the label with the unit
	/// </summary>
	void AssignCurrentNumberToLabel ()
	{
		SetText (currentNumberWithUnit);
	}

	/// <summary>
	/// Gets the text from a dfLabel.
	/// </summary>
	/// <returns>The text from object.</returns>
	public string GetTextFromObject ()
	{
		return label.Text;
	}
	
	/// <summary>
	/// Sets the text on a dfLabel.
	/// </summary>
	/// <param name="textToSet">Text to set.</param>
	public void SetText (string textToSet)
	{
		label.Text = textToSet;
	}
	#endregion

	
	/// <summary>
	/// Adds the audio source.
	/// </summary>
	void AddAudioSource ()
	{
		if (counterAudioSource == null) {
			counterAudioSource = gameObject.AddComponent <AudioSource> ();
			counterAudioSource.playOnAwake = false;
		}
	}
	
	/// <summary>
	/// Destroies the audio source.
	/// </summary>
	void DestroyCounterAudioSource ()
	{
		Destroy (counterAudioSource);
	}
}