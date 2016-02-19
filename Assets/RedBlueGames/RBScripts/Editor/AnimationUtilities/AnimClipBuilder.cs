using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public static class AnimClipBuilder {

	public static AnimationClip CreateClip (Sprite[] sprites, string clipName)
	{
		// Output nothing if there is no clip name
		if (string.IsNullOrEmpty (clipName)) {
			return null;
		}

		// Could be inputs
		int SampleRate = 12;
		bool IsLooping = false;

		// Create a new Clip
		AnimationClip clip = new AnimationClip ();

		// Apply the name and framerate
		clip.name = clipName;
		clip.frameRate = SampleRate;

		// Apply Looping Settings
		AnimationClipSettings clipSettings = new AnimationClipSettings ();
		clipSettings.loopTime = IsLooping;
		AnimationUtility.SetAnimationClipSettings (clip, clipSettings);

		// Initialize the curve property for the animation clip
		EditorCurveBinding curveBinding = new EditorCurveBinding ();
		curveBinding.propertyName = "m_Sprite";
		// Assumes user wants to apply the sprite property to the root element
		curveBinding.path = "";
		curveBinding.type = typeof(SpriteRenderer);
		
		// Build keyframes for the property using the supplied Sprites
		ObjectReferenceKeyframe[] keys = CreateKeysForSprites (sprites, SampleRate);
		
		// Build the clip if valid
		if (keys.Length > 0) {
			// Set the keyframes to the animation
			AnimationUtility.SetObjectReferenceCurve (clip, curveBinding, keys);
		}

		return clip;
	}
	
	static ObjectReferenceKeyframe[] CreateKeysForSprites (Sprite[] sprites, int samplesPerSecond)
	{
		List<ObjectReferenceKeyframe> keys = new List<ObjectReferenceKeyframe> ();
		float timePerFrame = 1.0f / samplesPerSecond;
		float currentTime = 0.0f;
		foreach (Sprite sprite in sprites) {
			ObjectReferenceKeyframe keyframe = new ObjectReferenceKeyframe ();
			keyframe.time = currentTime;
			keyframe.value = sprite;
			keys.Add (keyframe);
			
			currentTime += timePerFrame;
		}
		
		return keys.ToArray ();
	}
}
