using UnityEngine;
using System.Collections;

public class TweenFlashColor : MonoBehaviour
{
	public Color color;
	public float duration;

	int flashCount = 0;
	Renderer[] renderers;
	Color[] originalColors;
	
	/// <summary>
	/// Determine original color for restoring after coroutines.
	/// </summary>
	void Awake ()
	{
		renderers = (Renderer[])GetComponentsInChildren<Renderer> ();
		originalColors = new Color[renderers.Length];
		for (int i = 0; i < renderers.Length; ++i) {
			if (renderers [i].material.HasProperty ("_Color")) {
				originalColors [i] = renderers [i].material.color;
			} else {
				originalColors [i] = Color.black;
			}
		}
	}

	/// <summary>
	/// Flash to color and duration specified in editor.
	/// </summary>
	public void Flash ()
	{
		StartCoroutine (FlashCoroutine (color, duration));
	}

	/// <summary>
	/// Flash to color and duration specified by code.
	/// </summary>
	/// <param name="flashColor">Flash color.</param>
	/// <param name="flashSeconds">Flash seconds.</param>
	public void Flash (Color flashColor, float flashSeconds)
	{
		StartCoroutine (FlashCoroutine (flashColor, flashSeconds));
	}

	/// <summary>
	/// Iterates over all child objects the script is attached to and changes the 
	/// color for a specified amount of time.
	/// </summary>
	/// <returns>The coroutine.</returns>
	/// <param name="flashColor">Flash color.</param>
	/// <param name="flashSeconds">Flash seconds.</param>
	IEnumerator FlashCoroutine (Color flashColor, float flashSeconds)
	{
		flashCount++;
		for (int i = 0; i < renderers.Length; ++i) {
			if (renderers [i] != null && renderers [i].material != null) {
				if (renderers [i].material.HasProperty ("_Color")) {
					renderers [i].material.color = flashColor;
				}
			}
		}
		yield return new WaitForSeconds (flashSeconds);
		if (flashCount == 1) {
			for (int i = 0; i < renderers.Length; ++i) {
				if (renderers [i] != null && renderers [i].material != null) {
					renderers [i].material.color = originalColors [i];
				}
			}
		}
		flashCount--;
	}

	/// <summary>
	/// Clean up any coroutines in progress.
	/// </summary>
	void OnDestroy ()
	{
		StopAllCoroutines ();
	}
}
