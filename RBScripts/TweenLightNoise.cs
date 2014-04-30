using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Light))]
public class TweenLightNoise : MonoBehaviour
{
	// Turn noise on or off
	public bool noiseOn;

	// Noise characteristics
	public float speed = 2.0f;
	public float magnitude = 1.0f;

	// Facilitate noise generation
	float baseIntensity;
	float randomStart;

	// Use this for initialization
	void Start ()
	{
		baseIntensity = light.intensity;
		randomStart = Random.Range (-1000.0f, 1000.0f);
	}

	void Update ()
	{
		if (noiseOn) {
			float progress = (randomStart + Time.time) * speed;
			float flickerNoise = Mathf.PerlinNoise (progress, 0) * magnitude;
			light.intensity = baseIntensity + flickerNoise;
		}
	}
}
