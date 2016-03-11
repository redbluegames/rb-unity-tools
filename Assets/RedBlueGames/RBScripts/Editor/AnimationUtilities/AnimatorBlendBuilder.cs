using UnityEngine;
using UnityEditor;
using UnityEditor.Animations;
using System.Collections;

namespace RedBlueGames.Tools
{
    public class AnimatorBlendBuilder
    {

        #region Menu Items

        [MenuItem("Assets/Animator Utility/Create FourDir Blend")]
        public static void CreateFourDirBlend()
        {
            AnimatorController controller = (AnimatorController)Selection.activeObject;
		
            BlendTree tree;
            controller.CreateBlendTreeInController("New FourDir Blend Tree", out tree);
            InitializeBlendTree(ref tree, "New FourDir Blend");
		
            tree.AddChild(null, new Vector2(0.0f, 1.0f));
            tree.AddChild(null, new Vector2(1.0f, 0.0f));
            tree.AddChild(null, new Vector2(0.0f, -1.0f));
            tree.AddChild(null, new Vector2(-1.0f, 0.0f));

            SaveAnimatorController(controller);
        }

        static void SaveAnimatorController(AnimatorController controller)
        {
            string path = AssetDatabase.GetAssetPath(controller);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        static void InitializeBlendTree(ref BlendTree tree, string name)
        {
            tree.name = name;
            tree.blendType = BlendTreeType.FreeformCartesian2D;
            tree.blendParameter = "FaceX";
            tree.blendParameterY = "FaceY";

            tree.hideFlags = HideFlags.HideInHierarchy;
        }

        [MenuItem("Assets/Animator Utility/Create EightDir Blend")]
        public static void CreateEightDirBlend()
        {
            AnimatorController controller = (AnimatorController)Selection.activeObject;

            BlendTree tree;
            controller.CreateBlendTreeInController("New EightDir Blend Tree", out tree);
            InitializeBlendTree(ref tree, "New EightDir Blend");

            tree.AddChild(null, new Vector2(0.0f, 1.0f));
            tree.AddChild(null, new Vector2(1.0f, 1.0f));
            tree.AddChild(null, new Vector2(1.0f, 0.0f));
            tree.AddChild(null, new Vector2(1.0f, -1.0f));
            tree.AddChild(null, new Vector2(0.0f, -1.0f));
            tree.AddChild(null, new Vector2(-1.0f, -1.0f));
            tree.AddChild(null, new Vector2(-1.0f, 0.0f));
            tree.AddChild(null, new Vector2(-1.0f, 1.0f));

            SaveAnimatorController(controller);
        }

        #endregion

        #region Menu Item Validation

        // Note that we pass the same path, and also pass "true" to the second argument.
        [MenuItem("Assets/Animator Utility/Create FourDir Blend", true)]
        private static bool IsCreateFourDirBlendValid()
        {
            return SelectionUtilities.IsActiveObjectOfType<AnimatorController>();
        }
	
        // Note that we pass the same path, and also pass "true" to the second argument.
        [MenuItem("Assets/Animator Utility/Create EightDir Blend", true)]
        private static bool IsCreateEightDirBlendValid()
        {
            return SelectionUtilities.IsActiveObjectOfType<AnimatorController>();
        }

        #endregion
    }
}