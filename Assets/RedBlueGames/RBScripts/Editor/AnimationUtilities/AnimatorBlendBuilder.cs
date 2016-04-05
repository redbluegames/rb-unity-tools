namespace RedBlueGames.Tools
{
    using System.Collections;
    using UnityEditor;
    using UnityEditor.Animations;
    using UnityEngine;

    /// <summary>
    /// Facilitates quick building of common blend trees for Pixel art games
    /// </summary>
    public class AnimatorBlendBuilder
    {
        private const string CreateFourDirMenuPath = RBToolsMenuPaths.AnimationUtilitiesBase +
                                                     RBToolsMenuPaths.AnimatorSubmenu +
                                                     "Create FourDir Blend";
        
        private const string CreateEightDirMenuPath = RBToolsMenuPaths.AnimationUtilitiesBase +
                                                      RBToolsMenuPaths.AnimatorSubmenu +
                                                      "Create EightDir Blend";

        /// <summary>
        /// Create a four way (NSEW) directional blend in a selected Animator
        /// </summary>
        [MenuItem(CreateFourDirMenuPath)]
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

        /// <summary>
        /// Create an eight way directional (N, NE, E, SE, S, SW, W, NW) blend in a selected Animator
        /// </summary>
        [MenuItem(CreateEightDirMenuPath)]
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

        private static void SaveAnimatorController(AnimatorController controller)
        {
            string path = AssetDatabase.GetAssetPath(controller);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void InitializeBlendTree(ref BlendTree tree, string name)
        {
            tree.name = name;
            tree.blendType = BlendTreeType.FreeformCartesian2D;
            tree.blendParameter = "FaceX";
            tree.blendParameterY = "FaceY";

            tree.hideFlags = HideFlags.HideInHierarchy;
        }

        // Note that we pass the same path, and also pass "true" to the second argument.
        [MenuItem(CreateFourDirMenuPath, true)]
        private static bool IsCreateFourDirBlendValid()
        {
            return SelectionUtility.IsActiveObjectOfType<AnimatorController>();
        }

        // Note that we pass the same path, and also pass "true" to the second argument.
        [MenuItem(CreateEightDirMenuPath, true)]
        private static bool IsCreateEightDirBlendValid()
        {
            return SelectionUtility.IsActiveObjectOfType<AnimatorController>();
        }
    }
}