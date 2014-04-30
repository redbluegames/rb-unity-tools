using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class TweenLightFlicker : MonoBehaviour
{
	// Turn flicker on or off
	public bool flickerOn;

	// Flicker Charactersitics
	public float lightsOutMultiplier = 0.2f;
	public float frequency = 3.0f;

	// Base intensity, automatically taken from light parameters.
	float baseIntensity;
	
	void Start ()
	{
		baseIntensity = light.intensity;
		StartCoroutine (Flicker ());
	}
	
	void Update ()
	{
	}

	IEnumerator Flicker ()
	{
		while (flickerOn) {
			light.intensity = baseIntensity * lightsOutMultiplier;
			yield return new WaitForSeconds (0.005f);
			light.intensity = baseIntensity;
			yield return new WaitForSeconds (Random.Range (0, frequency));
		}
	}
}
