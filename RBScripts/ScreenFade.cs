using UnityEngine;
using System.Collections;
using RedBlueTools;

[RequireComponent(typeof(dfTextureSprite))]
public class ScreenFade : MonoBehaviour
{

	dfTextureSprite texture;

	// Events to send to any subscribers
	public delegate void FadeCompleteHandler ();
	public event FadeCompleteHandler FadeComplete;

	bool isFading;

	void Start ()
	{
		texture = GetComponent<dfTextureSprite> ();
		texture.Opacity = 0.0f;

		dfGUIManager manager = transform.parent.gameObject.GetComponent<dfGUIManager> ();
		texture.Size = manager.GetScreenSize ();
	}

	/// <summary>
	/// Forces the fader to be the frontmost widget.
	/// </summary>
	void SendToFront ()
	{
		texture.ZOrder = 100;
	}

	public void FadeToWhite (float fadeTime)
	{
		FadeIntoColor (Color.white, fadeTime);
	}
	
	public void FadeToBlack (float fadeTime)
	{
		FadeIntoColor (Color.black, fadeTime);
	}

	void FadeIntoColor (Color fadeColor, float duration)
	{	
		texture.Color = fadeColor;
		if(isFading) {
			StopCurrentFade ();
		}
		StartCoroutine (FadeToOpacity (1.0f, duration));
	}

	public void FadeOut (float duration)
	{	
		if(isFading) {
			StopCurrentFade ();
		}
		StartCoroutine (FadeToOpacity (0.0f, duration));
	}

	/// <summary>
	/// Linearly fades to the desired opacity in the specified amount of time
	/// </summary>
	/// <returns>The to opacity.</returns>
	/// <param name="desiredOpacity">Desired opacity.</param>
	/// <param name="duration">Fade Time.</param>
	IEnumerator FadeToOpacity (float desiredOpacity, float fadeTime)
	{	
		isFading = true;
		float startingOpacity = texture.Opacity;
		float opacityChangePerSecond = (desiredOpacity - startingOpacity) / fadeTime;
		float expectedOpacity = startingOpacity;
		float elapsed = 0.0f;
		while (elapsed < fadeTime) {
			elapsed += Time.deltaTime;

			// write and read from a variable since texture's Opacity won't be written as often
			// as this coroutine runs, causing lagging results.
			expectedOpacity = Mathf.Clamp01 (expectedOpacity + (opacityChangePerSecond * Time.deltaTime));
			texture.Opacity = expectedOpacity;

			// Make sure the fade texture remains in front of everything else for the duration of
			// the fade
			SendToFront ();
			yield return null;
		}

		CompleteFade ();
	}

	void CompleteFade ()
	{
		isFading = false;

		if (FadeComplete != null) {
			FadeComplete ();
		}
	}
	
	
	void StopCurrentFade ()
	{
		StopAllCoroutines ();
		CompleteFade ();
	}
}
