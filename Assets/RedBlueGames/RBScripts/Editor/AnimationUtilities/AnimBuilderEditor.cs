namespace RedBlueGames.Tools
{
    using UnityEditor;
    using UnityEngine;
    using System.Collections;

    [CustomEditor(typeof(AnimBuilder))]
    public class AnimBuilderEditor : UnityEditor.Editor
    {
        string clipName;
        bool allowErasing = false;

        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();

            if (GUILayout.Button("Generate Clips"))
            {
                AnimBuilder animBuilder = (AnimBuilder)target;
                animBuilder.GenerateClips();
            }

            clipName = EditorGUILayout.TextField("Clip Name", clipName);
            if (GUILayout.Button("Add Eight Dir Clips"))
            {
                AnimBuilder animBuilder = (AnimBuilder)target;
                animBuilder.AddEightBlendToClips(clipName);
            }

            // Enable this only for when you duplicate a builder and need to clear it... should rarely come up.
            allowErasing = EditorGUILayout.BeginToggleGroup("EnableEraseClips", allowErasing);
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
    }

    public static class AnimBuilderCreator
    {
        [MenuItem("Assets/Create/AnimBuilder/Empty")]
        static void CreateAnimBuilder()
        {
            AnimBuilder animBuilder = ScriptableObjectUtility.CreateAsset <AnimBuilder>();
            animBuilder.Initialize();

            AssetDatabase.SaveAssets();
        }

        [MenuItem("Assets/Create/AnimBuilder/Character")]
        static void CreateAnimBuilderForCharacter()
        {
            AnimBuilder animBuilder = ScriptableObjectUtility.CreateAsset <AnimBuilder>();
            animBuilder.Initialize();
            animBuilder.InitializeForCharacter();

            AssetDatabase.SaveAssets();
        }
    }
}