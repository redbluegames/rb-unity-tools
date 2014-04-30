using UnityEngine;
using System.Collections;

/*
 * Simple timer that does NOT update itself. The classes using
 * this need to check back periodically to see how much time
 * has gone by.
 */
public class RBCountUpTimer
{
	float timeStarted;
	bool isRunning;
	
	const float UNSET = float.MaxValue;
	
	/*
	 * Null constructor. Set our duration to a known invalid value.
	 */
	public RBCountUpTimer ()
	{
		StopTimer ();
	}
	
	/*
	 * Start the timer.
	 */
	public void StartTimer ()
	{
		timeStarted = Time.time;
		isRunning = true;
	}
	
	/*
	 * Unsets the duration and timeStarted fields. This is important
	 * to call if you plan to reuse the timer.
	 */
	public void StopTimer ()
	{
		timeStarted = UNSET;
		isRunning = false;
	}

	/*
	 * Return if the timer has been set.
	 */
	public bool IsRunning ()
	{
		return isRunning;
	}
	
	/*
	 * Get the amount of time in seconds the timer has been running.
	 */
	public float GetTimeSinceStarted ()
	{
		WarnIfUnSet ();
		return Time.time - timeStarted;
	}
	
	/*
	 * Helper warning to tell coder they called a method that needs duration set.
	 */
	void WarnIfUnSet ()
	{
		if (!isRunning || timeStarted == UNSET) {
			Debug.LogWarning ("Tried to check time left on stopped or unset timer.");
		}
	}
}
