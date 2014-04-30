using UnityEngine;
using System.Collections;

/*
 * Simple timer that does NOT update itself. The classes using
 * this need to check back periodically to see if time is up.
 * Then, if the timer is needed again, reset it.
 */
public class RBCountDownTimer : MonoBehaviour
{
	public float TimeStarted {get; private set;}
	public float Duration {get; private set;}
	public float TimeRemaining {get; private set;}
	public bool IsRunning  {get; private set;}

	const float UNSET = float.MaxValue;

	public delegate void CompleteEvent ();
	public CompleteEvent OnComplete;

	public RBCountDownTimer ()
	{
		InitializeToDefaults ();
	}
	
	void InitializeToDefaults ()
	{
		IsRunning = false;
		Duration = UNSET;
		TimeStarted = UNSET;
		TimeRemaining = 0.0f;
	}

	/// <summary>
	/// Starts the timer. Fires off OnComplete when done.
	/// </summary>
	/// <param name="desiredDuration">The desired duration for the timer in game seconds.</param>
	public void StartTimer (float desiredDuration)
	{
		// Stop the timer if it's running
		StopAllCoroutines ();

		StartCoroutine(CountdownForDuration(desiredDuration));
	}

	IEnumerator CountdownForDuration (float desiredDuration)
	{
		IsRunning = true;
		Duration = desiredDuration;
		TimeStarted = Time.time;

		TimeRemaining = Duration;
		while (TimeRemaining >= 0.0f ) {
			TimeRemaining -= Time.deltaTime;
			yield return null;
		}

		CompleteCountdown();
	}

	/// <summary>
	/// End a completed timer.
	/// </summary>
	void CompleteCountdown ()
	{
		if (OnComplete != null) {
			OnComplete ();
		}

		InitializeToDefaults ();
	}

	/// <summary>
	/// Stop the timer without firing off the complete event.
	/// </summary>
	public void StopTimer ()
	{
		InitializeToDefaults ();
		StopAllCoroutines ();
	}
}
