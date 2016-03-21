namespace RedBlueGames.Tools
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Utility class to help create AnimClips in code
    /// </summary>
    public static class AnimClipBuilder
    {
        /// <summary>
        /// Create an animation clip from passed in sprites
        /// </summary>
        /// <returns>The created clip.</returns>
        /// <param name="sprites">Sprites, in order.</param>
        /// <param name="clipName">Clip name.</param>
        public static AnimationClip CreateClip(Sprite[] sprites, string clipName)
        {
            // Output nothing if there is no clip name
            if (string.IsNullOrEmpty(clipName))
            {
                return null;
            }

            // Could be inputs
            int sampleRate = 12;
            bool isLooping = false;

            // Create a new Clip
            AnimationClip clip = new AnimationClip();

            // Apply the name and framerate
            clip.name = clipName;
            clip.frameRate = sampleRate;

            // Apply Looping Settings
            AnimationClipSettings clipSettings = new AnimationClipSettings();
            clipSettings.loopTime = isLooping;
            AnimationUtility.SetAnimationClipSettings(clip, clipSettings);

            // Initialize the curve property for the animation clip
            EditorCurveBinding curveBinding = new EditorCurveBinding();
            curveBinding.propertyName = "m_Sprite";

            // Assumes user wants to apply the sprite property to the root element
            curveBinding.path = string.Empty;
            curveBinding.type = typeof(SpriteRenderer);

            // Build keyframes for the property using the supplied Sprites
            ObjectReferenceKeyframe[] keys = CreateKeysForSprites(sprites, sampleRate);

            // Build the clip if valid
            if (keys.Length > 0)
            {
                // Set the keyframes to the animation
                AnimationUtility.SetObjectReferenceCurve(clip, curveBinding, keys);
            }

            return clip;
        }

        private static ObjectReferenceKeyframe[] CreateKeysForSprites(Sprite[] sprites, int samplesPerSecond)
        {
            List<ObjectReferenceKeyframe> keys = new List<ObjectReferenceKeyframe>();
            float timePerFrame = 1.0f / samplesPerSecond;
            float currentTime = 0.0f;
            foreach (Sprite sprite in sprites)
            {
                ObjectReferenceKeyframe keyframe = new ObjectReferenceKeyframe();
                keyframe.time = currentTime;
                keyframe.value = sprite;
                keys.Add(keyframe);

                currentTime += timePerFrame;
            }

            return keys.ToArray();
        }
    }
}