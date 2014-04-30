using UnityEngine;
using System.Collections;
using RedBlueTools;

/*
 * Code pattern for this class borrowed from:
 * http://wiki.unity3d.com/index.php/Singleton
 *
 **/
public class TimeManager : Singleton<TimeManager>
{
	// Time and Pause handling members
	int pauseRequests;
	int lowLevelPauseRequests;
	
	public bool IsPaused { get; private set; }
	
	public bool IsLowLevelPaused { get; private set; }
	
	// guarantee this will be always a singleton only - can't use the constructor!
	protected TimeManager ()
	{
	}
	
	void Update ()
	{
	}
	
	/*
	 * Pauses the game, or increments the pause counter if it's already paused.
	 */
	public void RequestPause ()
	{
		pauseRequests++;
		IsPaused = true;
		ResolveTimeScale ();
	}
	
	/*
	 * Attempts to unpause the game. Once all requests to pause have been unwound, the game
	 * unpauses.
	 */
	public void RequestUnpause ()
	{
		pauseRequests--;
		if (pauseRequests == 0) {
			IsPaused = false;
		}
		ResolveTimeScale ();
	}
	
	public void RequestLowLevelPause ()
	{
		lowLevelPauseRequests++;
		IsLowLevelPaused = true;
		ResolveTimeScale ();
	}
	
	public void RequestLowLevelUnpause ()
	{
		lowLevelPauseRequests--;
		if (lowLevelPauseRequests == 0) {
			IsLowLevelPaused = false;
		}
		ResolveTimeScale ();
	}
	
	void ResolveTimeScale ()
	{
		if (IsPaused || IsLowLevelPaused) {
			Time.timeScale = 0.0f;
		} else {
			Time.timeScale = 1.0f;
		}
	}
}