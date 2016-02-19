using UnityEngine;
using UnityEngine.Serialization;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using RedBlueGames.Tools;

public class AnimBuilder : ScriptableObject
{
	[Header ("Source Texture")]
	public Texture2D TextureWithAnims;

	[Header ("Animations")]
	public int SamplesPerSecond;
	public string PathToSpriteRenderer;
	public List<SpriteAnimClip> Clips;

	public string SavePath {
		get {
			return AssetDatabaseUtility.GetAssetDirectory (this);
		}
	}

	#region Initialization
	public void Initialize ()
	{
		Clips = new List<SpriteAnimClip> ();
		SamplesPerSecond = 12;
		PathToSpriteRenderer = ""; // For now just assume sprite render is on root.
	}
	
	public void InitializeForCharacter ()
	{
		AddEightBlendToClips ("Idle");
		AddEightBlendToClips ("Move");
		AddEightBlendToClips ("Attack");
		AddEightBlendToClips ("AttackH");
		AddEightBlendToClips ("AttackH_Warmup");
		AddEightBlendToClips ("Dodge");
	}

	public void AddEightBlendToClips (string blendStateName)
	{
		string[] directionSuffixes = 
		{
			"U",
			"UR",
			"R",
			"DR",
			"D",
			"DL",
			"L",
			"UL",
		};

		for(int i = 0; i < directionSuffixes.Length; i++) {
			Clips.Add ( new SpriteAnimClip ()
			{
				ClipName = blendStateName + "_" + directionSuffixes[i],
				PathToSpriteRenderer = this.PathToSpriteRenderer,
				AnimationKeyframes = new KeyframeRange[1],
			});
		}
	}

	#endregion
	public void GenerateClips ()
	{
		if (TextureWithAnims == null) {
			Debug.LogError ("No texture provided.");
			return;
		}
		foreach (SpriteAnimClip clip in Clips) {
			clip.SourceTexture = TextureWithAnims;
			clip.PathToSpriteRenderer = PathToSpriteRenderer;
			clip.Samples = SamplesPerSecond;
			clip.GenerateClip (SavePath, TextureWithAnims.name);
		}
		AssetDatabase.SaveAssets ();
		AssetDatabase.Refresh ();
	}
	
	[System.Serializable]
	public class SpriteAnimClip
	{
		public string ClipName;
		public bool IsLooping = true;
		public bool IsMirroredX = false;
		public bool IsMirroredY = false;
		public KeyframeRange[] AnimationKeyframes;
		public AnimationClip SavedClip;
		[HideInInspector]
		public string
			PathToSpriteRenderer;
		[HideInInspector]
		public Texture2D 
			SourceTexture;
		int _samples;
		
		public int Samples {
			get {
				return _samples;
			}
			set {
				int defaultSamples = 12;
				if (value <= 0) {
					_samples = defaultSamples;
				} else {
					_samples = value;
				}
			}
		}

		public void GenerateClip (string savePath, string filenamePrefix)
		{
			// Output nothing if there is no clip name
			if (string.IsNullOrEmpty (ClipName)) {
				return;
			}

			// Output nothing if no frames are defined
			if (AnimationKeyframes == null || AnimationKeyframes.Length == 0) {
				return;
			}

			// Get the clip to add our Sprite Animation into, or create a new one.
			AnimationClip builtClip;
			bool clipIsNew;
			if (SavedClip == null) {
				builtClip = new AnimationClip ();
				clipIsNew = true;
			} else {
				builtClip = SavedClip;
				clipIsNew = false;
			}

			builtClip.name = ClipName;
			builtClip.frameRate = Samples;

			// Set the Looping status of the clip
			AnimationClipSettings clipSettings = new AnimationClipSettings ();
			clipSettings.loopTime = IsLooping;
			AnimationUtility.SetAnimationClipSettings (builtClip, clipSettings);

			// Clear ALL existing sprite bindings in the clip
			EditorCurveBinding[] existingObjectBinding = AnimationUtility.GetObjectReferenceCurveBindings (builtClip);
			for (int i = 0; i < existingObjectBinding.Length; i++) {
				EditorCurveBinding currentBinding = existingObjectBinding [i];
				if (currentBinding.type == typeof(SpriteRenderer)) {
					AnimationUtility.SetObjectReferenceCurve (builtClip, currentBinding, null);
				} 
			}

			// Clear existing Scale since it will be replaced
			EditorCurveBinding[] existingValueBindings = AnimationUtility.GetCurveBindings (builtClip);
			for (int i = 0; i < existingValueBindings.Length; i++)
			{
				EditorCurveBinding currentBinding = existingValueBindings [i];
				if (currentBinding.type == typeof(Transform) && currentBinding.propertyName == "m_LocalScale.x") {
					builtClip.SetCurve (currentBinding.path, typeof(Transform), "m_LocalScale", null);
					break;
				}
			}
			
			// Initialize the curve property
			EditorCurveBinding curveBinding = new EditorCurveBinding ();
			curveBinding.propertyName = "m_Sprite";
			curveBinding.path = PathToSpriteRenderer;
			curveBinding.type = typeof(SpriteRenderer);
			
			// Build keyframes for the property
			Sprite[] sprites = SpriteSlicer.GetSortedSpritesInTexture (SourceTexture);
			ObjectReferenceKeyframe[] keys = CreateKeysForKeyframeRanges 
				(sprites, AnimationKeyframes, Samples);

			// Build the clip if valid
			if (keys != null && keys.Length > 0) {
				// Set the keyframes to the animation
				AnimationUtility.SetObjectReferenceCurve (builtClip, curveBinding, keys);

				// Add scaling to mirror sprites
				// Need to also restore scale in case a clip was previously mirrored and then unflagged
				AnimationCurve normalCurve = AnimationCurve.Linear (0.0f, 1.0f, builtClip.length, 1.0f);
				AnimationCurve mirrorCurve = AnimationCurve.Linear (0.0f, -1.0f, builtClip.length, -1.0f);
				AnimationCurve xCurve = IsMirroredX ? mirrorCurve : normalCurve;
				AnimationCurve yCurve = IsMirroredY ? mirrorCurve : normalCurve;
				builtClip.SetCurve (PathToSpriteRenderer, typeof(Transform), "localScale.x", xCurve);
				builtClip.SetCurve (PathToSpriteRenderer, typeof(Transform), "localScale.y", yCurve);
				builtClip.SetCurve (PathToSpriteRenderer, typeof(Transform), "localScale.z", normalCurve);
			
				// Create or replace the file
				string filenameSansExtension = filenamePrefix + "_" + ClipName;
				if (clipIsNew) {
					string filename = filenameSansExtension + ".anim";
					string fullpath = savePath + filename;
					AssetDatabase.CreateAsset (builtClip, fullpath);
				} else {
					string pathToAsset = AssetDatabase.GetAssetPath (SavedClip);
					// renaming file doesn't expect extension for some reason
					AssetDatabase.RenameAsset (pathToAsset, filenameSansExtension);
				}

				// Store reference to created clip to allow overwriting / renaming
				SavedClip = builtClip;
			} else {
				if (keys == null) {
					Debug.LogWarning ("Skipping clip due to no keys found: " + ClipName);
				} else {
					Debug.LogWarning ("Encountered invalid clip. Not enough keys. Skipping clip: " + ClipName);
				}
			}
		}

		ObjectReferenceKeyframe[] CreateKeysForKeyframeRanges (Sprite[] sprites, KeyframeRange[] keyframeRanges, int samplesPerSecond)
		{
			List<ObjectReferenceKeyframe> keys = new List<ObjectReferenceKeyframe> ();
			float timePerFrame = 1.0f / samplesPerSecond;
			int currentKeyIndex = 0;
			float currentTime = 0.0f;
			for (int rangeIndex = 0; rangeIndex < AnimationKeyframes.Length; rangeIndex++) {
				KeyframeRange range = AnimationKeyframes[rangeIndex];

				// Skip invalid ranges
				if (!range.IsValid () || sprites == null) {
					Debug.LogWarning ("Found invalid KeyframeRange. Skipping Range on Clip: " + ClipName);
					continue;
				}
				float timePerSubkey = range.SamplesPerFrame * timePerFrame;
				for (int subkey = 0; subkey < range.NumKeyframes; subkey++) {
					int spriteIndex = range.FirstFrame + subkey;
					if (spriteIndex >= sprites.Length) {
						Debug.LogError ("Sprite not found at index: " + spriteIndex +
						                " for clip: " + ClipName + ". RangeIndex: " + rangeIndex);
						return null;
					}
					ObjectReferenceKeyframe keyframe = new ObjectReferenceKeyframe ();
					keyframe.time = currentTime;
					keyframe.value = sprites [spriteIndex];
					keys.Add (keyframe);

					currentTime += timePerSubkey;
					currentKeyIndex ++;
				}
			}
			// If the last KeyframeRange is longer than one frame we need to add a keyframe at the end of the interval
			// to keep anim from ending early
			if (keyframeRanges.Last ().IsValid () && keyframeRanges.Last ().SamplesPerFrame > 1) {
				ObjectReferenceKeyframe keyframe = new ObjectReferenceKeyframe ();
				keyframe.time = (currentTime - timePerFrame);
				keyframe.value = sprites [keyframeRanges.Last ().LastFrame];
				keys.Add (keyframe);
			}

			return keys.ToArray ();
		}
	}

	[System.Serializable]
	public class KeyframeRange
	{
		public int FirstFrame = -1;
		public int LastFrame = -1;
		public int SamplesPerFrame = 1;

		public int NumKeyframes {
			get {
				return LastFrame - FirstFrame + 1;
			}
		}

		public bool IsValid ()
		{
			if (SamplesPerFrame < 0) {
				return false;
			}

			if (FirstFrame < 0 || LastFrame < 0) {
				return false;
			}

			if (FirstFrame > LastFrame) {
				return false;
			}

			return true;
		}
	}
}

#if UNITY_EDITOR
public static class AnimBuilderCreator
{
	static string defaultName = "New AnimBuilder.asset";

	[MenuItem ("Assets/Create/AnimBuilder/Empty")]
	static void CreateAnimBuilder ()
	{
		AnimBuilder animBuilder = ScriptableObject.CreateInstance<AnimBuilder> ();
		animBuilder.Initialize ();

		string currentPath = AssetDatabaseUtility.GetDirectoryOfSelection ();
		string path = AssetDatabaseUtility.GetDirectoryOfSelection ();
		string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath (path + defaultName);
		AssetDatabaseUtility.SaveAndSelectObject (animBuilder, currentPath, Path.GetFileName(uniqueAssetPath));
	}

	[MenuItem ("Assets/Create/AnimBuilder/Character")]
	static void CreateAnimBuilderForCharacter ()
	{
		AnimBuilder animBuilder = ScriptableObject.CreateInstance<AnimBuilder> ();
		animBuilder.Initialize ();
		animBuilder.InitializeForCharacter ();
		
		string currentPath = AssetDatabaseUtility.GetDirectoryOfSelection ();
		string path = AssetDatabaseUtility.GetDirectoryOfSelection ();
		string uniqueAssetPath = AssetDatabase.GenerateUniqueAssetPath (path + defaultName);
		AssetDatabaseUtility.SaveAndSelectObject (animBuilder, currentPath, Path.GetFileName(uniqueAssetPath));
	}
}

[CustomEditor (typeof (AnimBuilder))]
public class AnimBuilderInspector : UnityEditor.Editor
{
	string clipName;
	bool allowErasing = false;

	public override void OnInspectorGUI ()
	{
		DrawDefaultInspector ();
		
		if (GUILayout.Button ("Generate Clips")) {
			AnimBuilder animBuilder = (AnimBuilder)target;
			animBuilder.GenerateClips ();
		}

		clipName = EditorGUILayout.TextField ("Clip Name", clipName);
		if (GUILayout.Button ("Add Eight Dir Clips")) {
			AnimBuilder animBuilder = (AnimBuilder)target;
			animBuilder.AddEightBlendToClips (clipName);
		}

		// Enable this only for when you duplicate a builder and need to clear it... should rarely come up.
		allowErasing = EditorGUILayout.BeginToggleGroup ("EnableEraseClips", allowErasing);
		if (GUILayout.Button ("Erase clips")) {
			AnimBuilder animBuilder = (AnimBuilder)target;
			foreach (AnimBuilder.SpriteAnimClip clip in animBuilder.Clips) {
				clip.SavedClip = null;
			}
		}
		EditorGUILayout.EndToggleGroup ();
	}
}
#endif