namespace RedBlueGames.Tools
{
    using System.Collections;
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Animation builder editor draws the AnimBuilder scriptable object
    /// </summary>
    [CustomEditor(typeof(AnimBuilder))]
    public class AnimBuilderEditor : UnityEditor.Editor
    {
        private string clipName;
        private bool allowErasing = false;

        /// <summary>
        /// Raises the inspector GU event.
        /// </summary>
        public override void OnInspectorGUI()
        {
            this.DrawDefaultInspector();

            if (GUILayout.Button("Generate Clips"))
            {
                AnimBuilder animBuilder = (AnimBuilder)target;
                animBuilder.GenerateClips();
            }

            this.clipName = EditorGUILayout.TextField("Clip Name", this.clipName);
            if (GUILayout.Button("Add Eight Dir Clips"))
            {
                AnimBuilder animBuilder = (AnimBuilder)target;
                animBuilder.AddEightBlendToClips(this.clipName);
            }

            // Enable this only for when you duplicate a builder and need to clear it... should rarely come up.
            this.allowErasing = EditorGUILayout.BeginToggleGroup("EnableEraseClips", this.allowErasing);
            if (GUILayout.Button("Erase clips"))
            {
                AnimBuilder animBuilder = (AnimBuilder)target;
                foreach (AnimBuilder.SpriteAnimClip clip in animBuilder.Clips)
                {
                    clip.SavedClip = null;
                }
            }

            EditorGUILayout.EndToggleGroup();
        }

        /// <summary>
        /// Class that contains methods for creating AnimBuilders 
        /// </summary>
        public static class AnimBuilderCreator
        {
            [MenuItem("Assets/Create/AnimBuilder/Empty")]
            private static void CreateAnimBuilder()
            {
                AnimBuilder animBuilder = ScriptableObjectUtility.CreateAsset<AnimBuilder>();
                animBuilder.Initialize();

                AssetDatabase.SaveAssets();
            }

            [MenuItem("Assets/Create/AnimBuilder/Character")]
            private static void CreateAnimBuilderForCharacter()
            {
                AnimBuilder animBuilder = ScriptableObjectUtility.CreateAsset<AnimBuilder>();
                animBuilder.Initialize();
                animBuilder.InitializeForCharacter();

                AssetDatabase.SaveAssets();
            }
        }
    }
}