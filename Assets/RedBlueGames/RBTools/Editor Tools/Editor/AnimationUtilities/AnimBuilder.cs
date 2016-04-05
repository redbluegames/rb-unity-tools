namespace RedBlueGames.Tools
{
    using System.Collections;
    using System.Collections.Generic;
    using System.IO;
    using RedBlueGames.Tools;
    using UnityEditor;
    using UnityEngine;
    using UnityEngine.Serialization;

    /// <summary>
    /// ScriptableObject that helps us build and update animation clips from textures quickly
    /// </summary>
    public class AnimBuilder : ScriptableObject
    {
        [Header("Source Texture")]
        [SerializeField]
        private Texture2D textureWithAnims;

        [Header("Animations")]
        [SerializeField]
        private int samplesPerSecond;
        [SerializeField]
        private string pathToSpriteRenderer;
        [SerializeField]
        private List<SpriteAnimClip> clips;

        /// <summary>
        /// Gets all the clips that have been built by the AnimBuilder
        /// </summary>
        /// <value>The clips.</value>
        public List<SpriteAnimClip> Clips
        {
            get
            {
                return this.clips;
            }
        }

        private string SavePath
        {
            get
            {
                return AssetDatabaseUtility.GetAssetDirectory(this);
            }
        }

        /// <summary>
        /// Initialize the AnimBuilder. This is used sort of like a constructor for Scriptable Objects.
        /// </summary>
        public void Initialize()
        {
            this.clips = new List<SpriteAnimClip>();
            this.samplesPerSecond = 12;
            this.pathToSpriteRenderer = string.Empty; // For now just assume sprite render is on root.
        }

        /// <summary>
        /// Initialize the AnimBuilder for use with a Character. This is used sort of like a constructor for Scriptable Objects.
        /// </summary>
        public void InitializeForCharacter()
        {
            this.AddEightBlendToClips("Idle");
            this.AddEightBlendToClips("Move");
            this.AddEightBlendToClips("Attack");
            this.AddEightBlendToClips("AttackH");
            this.AddEightBlendToClips("AttackH_Warmup");
            this.AddEightBlendToClips("Dodge");
        }

        /// <summary>
        /// Adds eight clips to the builder, setup for Octant animations.
        /// </summary>
        /// <param name="blendStateName">Blend state name.</param>
        public void AddEightBlendToClips(string blendStateName)
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

            for (int i = 0; i < directionSuffixes.Length; i++)
            {
                string clipName = blendStateName + "_" + directionSuffixes[i];
                this.clips.Add(new SpriteAnimClip(clipName, this.pathToSpriteRenderer));
            }
        }

        /// <summary>
        /// Generates or updates clips based on the settings in the AnimBuilder.
        /// </summary>
        public void GenerateClips()
        {
            if (this.textureWithAnims == null)
            {
                Debug.LogError("No texture provided.");
                return;
            }

            foreach (SpriteAnimClip clip in this.Clips)
            {
                clip.SourceTexture = this.textureWithAnims;
                clip.PathToSpriteRenderer = this.pathToSpriteRenderer;
                clip.Samples = this.samplesPerSecond;
                clip.GenerateClip(this.SavePath, this.textureWithAnims.name);
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        /// <summary>
        /// A container for an AnimClip that is simply used for Sprite Animation.
        /// </summary>
        [System.Serializable]
        public class SpriteAnimClip
        {
            [SerializeField]
            private string clipName;
            [SerializeField]
            private bool isLooping = true;
            [SerializeField]
            private bool isMirroredX = false;
            [SerializeField]
            private bool isMirroredY = false;
            [SerializeField]
            private KeyframeRange[] animationKeyframes;
            [SerializeField]
            private AnimationClip savedClip;

            private int samples;

            /// <summary>
            /// Initializes a new instance of the <see cref="SpriteAnimClip"/> class.
            /// </summary>
            /// <param name="clipName">Clip name.</param>
            /// <param name="pathToRenderer">Path to the Sprite renderer, used in the AnimClip's Property.</param>
            public SpriteAnimClip(string clipName, string pathToRenderer = default(string))
            {
                this.clipName = clipName;
                this.PathToSpriteRenderer = pathToRenderer;
                this.animationKeyframes = new KeyframeRange[1];
            }

            /// <summary>
            /// Gets or sets the number of samples for the clip
            /// </summary>
            /// <value>The samples.</value>
            public int Samples
            {
                get
                {
                    return this.samples;
                }

                set
                {
                    int defaultSamples = 12;
                    if (value <= 0)
                    {
                        this.samples = defaultSamples;
                    }
                    else
                    {
                        this.samples = value;
                    }
                }
            }

            /// <summary>
            /// Gets or sets the path to sprite renderer, used to create the correct path for the AnimClip Property
            /// </summary>
            /// <value>The path to sprite renderer.</value>
            public string PathToSpriteRenderer { get; set; }

            /// <summary>
            /// Gets or sets the Source texture to build the sprites from
            /// </summary>
            /// <value>The source texture.</value>
            public Texture2D SourceTexture { get; set; }

            /// <summary>
            /// Gets or sets the clip that has been previously saved, if one exists.
            /// </summary>
            /// <value>The saved clip.</value>
            public AnimationClip SavedClip
            {
                get
                {
                    return this.savedClip;
                }

                set
                {
                    this.savedClip = value;
                }
            }

            /// <summary>
            /// Generates a clip from this SpriteAnim
            /// </summary>
            /// <param name="savePath">Save path.</param>
            /// <param name="filenamePrefix">Filename prefix.</param>
            public void GenerateClip(string savePath, string filenamePrefix)
            {
                // Output nothing if there is no clip name
                if (string.IsNullOrEmpty(this.clipName))
                {
                    return;
                }

                // Output nothing if no frames are defined
                if (this.animationKeyframes == null || this.animationKeyframes.Length == 0)
                {
                    return;
                }

                // Get the clip to add our Sprite Animation into, or create a new one.
                AnimationClip builtClip;
                bool clipIsNew;
                if (this.savedClip == null)
                {
                    builtClip = new AnimationClip();
                    clipIsNew = true;
                }
                else
                {
                    builtClip = this.savedClip;
                    clipIsNew = false;
                }

                builtClip.name = this.clipName;
                builtClip.frameRate = this.Samples;

                // Set the Looping status of the clip
                AnimationClipSettings clipSettings = new AnimationClipSettings();
                clipSettings.loopTime = this.isLooping;
                AnimationUtility.SetAnimationClipSettings(builtClip, clipSettings);

                // Clear ALL existing sprite bindings in the clip
                EditorCurveBinding[] existingObjectBinding = AnimationUtility.GetObjectReferenceCurveBindings(builtClip);
                for (int i = 0; i < existingObjectBinding.Length; i++)
                {
                    EditorCurveBinding currentBinding = existingObjectBinding[i];
                    if (currentBinding.type == typeof(SpriteRenderer))
                    {
                        AnimationUtility.SetObjectReferenceCurve(builtClip, currentBinding, null);
                    } 
                }

                // Clear existing Scale since it will be replaced
                EditorCurveBinding[] existingValueBindings = AnimationUtility.GetCurveBindings(builtClip);
                for (int i = 0; i < existingValueBindings.Length; i++)
                {
                    EditorCurveBinding currentBinding = existingValueBindings[i];
                    if (currentBinding.type == typeof(Transform) && currentBinding.propertyName == "m_LocalScale.x")
                    {
                        builtClip.SetCurve(currentBinding.path, typeof(Transform), "m_LocalScale", null);
                        break;
                    }
                }

                // Initialize the curve property
                EditorCurveBinding curveBinding = new EditorCurveBinding();
                curveBinding.propertyName = "m_Sprite";
                curveBinding.path = this.PathToSpriteRenderer;
                curveBinding.type = typeof(SpriteRenderer);

                // Build keyframes for the property
                Sprite[] sprites = AssetDatabaseUtility.LoadSpritesInTextureSorted(this.SourceTexture);
                ObjectReferenceKeyframe[] keys = this.CreateKeysForKeyframeRanges(sprites, this.animationKeyframes, this.Samples);

                // Build the clip if valid
                if (keys != null && keys.Length > 0)
                {
                    // Set the keyframes to the animation
                    AnimationUtility.SetObjectReferenceCurve(builtClip, curveBinding, keys);

                    // Add scaling to mirror sprites
                    // Need to also restore scale in case a clip was previously mirrored and then unflagged
                    AnimationCurve normalCurve = AnimationCurve.Linear(0.0f, 1.0f, builtClip.length, 1.0f);
                    AnimationCurve mirrorCurve = AnimationCurve.Linear(0.0f, -1.0f, builtClip.length, -1.0f);
                    AnimationCurve xCurve = this.isMirroredX ? mirrorCurve : normalCurve;
                    AnimationCurve yCurve = this.isMirroredY ? mirrorCurve : normalCurve;
                    builtClip.SetCurve(this.PathToSpriteRenderer, typeof(Transform), "localScale.x", xCurve);
                    builtClip.SetCurve(this.PathToSpriteRenderer, typeof(Transform), "localScale.y", yCurve);
                    builtClip.SetCurve(this.PathToSpriteRenderer, typeof(Transform), "localScale.z", normalCurve);

                    // Create or replace the file
                    string filenameSansExtension = filenamePrefix + "_" + this.clipName;
                    if (clipIsNew)
                    {
                        string filename = filenameSansExtension + ".anim";
                        string fullpath = savePath + filename;
                        AssetDatabase.CreateAsset(builtClip, fullpath);
                    }
                    else
                    {
                        string pathToAsset = AssetDatabase.GetAssetPath(this.savedClip);

                        // renaming file doesn't expect extension for some reason
                        AssetDatabase.RenameAsset(pathToAsset, filenameSansExtension);
                    }

                    // Store reference to created clip to allow overwriting / renaming
                    this.savedClip = builtClip;
                }
                else
                {
                    if (keys == null)
                    {
                        Debug.LogWarning("Skipping clip due to no keys found: " + this.clipName);
                    }
                    else
                    {
                        Debug.LogWarning("Encountered invalid clip. Not enough keys. Skipping clip: " + this.clipName);
                    }
                }
            }

            private ObjectReferenceKeyframe[] CreateKeysForKeyframeRanges(Sprite[] sprites, KeyframeRange[] keyframeRanges, int samplesPerSecond)
            {
                List<ObjectReferenceKeyframe> keys = new List<ObjectReferenceKeyframe>();
                float timePerFrame = 1.0f / samplesPerSecond;
                int currentKeyIndex = 0;
                float currentTime = 0.0f;
                for (int rangeIndex = 0; rangeIndex < this.animationKeyframes.Length; rangeIndex++)
                {
                    KeyframeRange range = this.animationKeyframes[rangeIndex];

                    // Skip invalid ranges
                    if (!range.IsValid() || sprites == null)
                    {
                        Debug.LogWarning("Found invalid KeyframeRange. Skipping Range on Clip: " + this.clipName);
                        continue;
                    }

                    float timePerSubkey = range.SamplesPerFrame * timePerFrame;
                    for (int subkey = 0; subkey < range.NumKeyframes; subkey++)
                    {
                        int spriteIndex = range.FirstFrame + subkey;
                        if (spriteIndex >= sprites.Length)
                        {
                            Debug.LogError("Sprite not found at index: " + spriteIndex +
                                " for clip: " + this.clipName + ". RangeIndex: " + rangeIndex);
                            return null;
                        }

                        ObjectReferenceKeyframe keyframe = new ObjectReferenceKeyframe();
                        keyframe.time = currentTime;
                        keyframe.value = sprites[spriteIndex];
                        keys.Add(keyframe);

                        currentTime += timePerSubkey;
                        currentKeyIndex++;
                    }
                }

                // If the last KeyframeRange is longer than one frame we need to add a keyframe at the end of the interval
                // to keep anim from ending early
                if (keyframeRanges.Last().IsValid() && keyframeRanges.Last().SamplesPerFrame > 1)
                {
                    ObjectReferenceKeyframe keyframe = new ObjectReferenceKeyframe();
                    keyframe.time = currentTime - timePerFrame;
                    keyframe.value = sprites[keyframeRanges.Last().LastFrame];
                    keys.Add(keyframe);
                }

                return keys.ToArray();
            }
        }

        /// <summary>
        /// KeyframeRange defines a range of frames to use in a clip, and a sample rate.
        /// </summary>
        [System.Serializable]
        public class KeyframeRange
        {
            [SerializeField]
            private int firstFrame = -1;
            [SerializeField]
            private int lastFrame = -1;
            [SerializeField]
            private int samplesPerFrame = 1;

            /// <summary>
            /// Gets or sets the first frame index of the range
            /// </summary>
            /// <value>The first frame.</value>
            public int FirstFrame
            {
                get
                {
                    return this.firstFrame;
                }

                set
                {
                    this.firstFrame = value;
                }
            }

            /// <summary>
            /// Gets or sets the last frame index of the range
            /// </summary>
            /// <value>The last frame.</value>
            public int LastFrame
            {
                get
                {
                    return this.lastFrame;
                }

                set
                {
                    this.lastFrame = value;
                }
            }

            /// <summary>
            /// Gets or sets the samples per frame for the range
            /// </summary>
            /// <value>The samples per frame.</value>
            public int SamplesPerFrame
            {
                get
                {
                    return this.samplesPerFrame;
                }

                set
                {
                    this.samplesPerFrame = value;
                }
            }

            /// <summary>
            /// Gets the number keyframes in the range
            /// </summary>
            /// <value>The number keyframes.</value>
            public int NumKeyframes
            {
                get
                {
                    return this.LastFrame - this.FirstFrame + 1;
                }
            }

            /// <summary>
            /// Determines whether this instance defines a valid range.
            /// </summary>
            /// <returns><c>true</c> if this instance is valid; otherwise, <c>false</c>.</returns>
            public bool IsValid()
            {
                if (this.SamplesPerFrame < 0)
                {
                    return false;
                }

                if (this.FirstFrame < 0 || this.LastFrame < 0)
                {
                    return false;
                }

                if (this.FirstFrame > this.LastFrame)
                {
                    return false;
                }

                return true;
            }
        }
    }
}