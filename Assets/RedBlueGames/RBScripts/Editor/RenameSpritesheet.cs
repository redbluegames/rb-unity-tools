namespace RedBlueGames.Tools
{
    using UnityEditor;
    using UnityEngine;

    /// <summary>
    /// Tool that renames all the sprites in a selected spritesheet
    /// </summary>
    public class RenameSpritesheet : EditorWindow
    {
        private string oldSpritePrefix;
        private string newSpritePrefix;

        private static UnityEngine.Object SelectedObject
        {
            get
            {
                var selectedObject = Selection.activeObject;
                if (!AssetDatabase.Contains(selectedObject))
                {
                    Debug.LogError("Selected object not found in Asset Database.");
                    return null;
                }

                return selectedObject;
            }
        }

        [MenuItem(RBToolsMenuPaths.RenameSpritesheet)]
        private static void ShowRenameSpritesheetWindow()
        {
            EditorWindow.GetWindow<RenameSpritesheet>(true, "Rename Texture", true);
        }

        [MenuItem(RBToolsMenuPaths.RenameSpritesheet, true)]
        private static bool IsSelectionTexture()
        {
            if (Selection.activeObject == null)
            {
                return false;
            }

            if (Selection.objects.Length > 1)
            {
                return false;
            }

            return Selection.activeObject.GetType() == typeof(Texture2D);
        }

        private static void RenameSelectedTexture(string oldName, string newName)
        {
            string path = AssetDatabase.GetAssetPath(SelectedObject);
            string metaFile = System.IO.File.ReadAllText(path + ".meta");
            string ammendedMetaFile = ReplaceSpritePrefixInMetafile(metaFile, oldName, newName);
            System.IO.File.WriteAllText(path + ".meta", ammendedMetaFile);

            AssetDatabase.RenameAsset(path, newName);
            AssetDatabase.Refresh();
        }

        private static string ReplaceSpritePrefixInMetafile(string metafileText, string prefixToReplace, string newPrefix)
        {
            string modifiedMetafile = ReplaceFileIDRecycleNames(metafileText, prefixToReplace, newPrefix);
            modifiedMetafile = ReplaceSpriteMetaData(modifiedMetafile, prefixToReplace, newPrefix);
            return modifiedMetafile;
        }

        private static string ReplaceFileIDRecycleNames(string metafileText, string oldPrefix, string newPrefix)
        {
            string fileIDPattern = "([\\d]{8}: )" + oldPrefix;
            var fileIDRegex = new System.Text.RegularExpressions.Regex(fileIDPattern);
            string replacementText = "$1" + newPrefix;
            return fileIDRegex.Replace(metafileText, replacementText);
        }

        private static string ReplaceSpriteMetaData(string metafileText, string oldPrefix, string newPrefix)
        {
            string spritenamePattern = "(- name: )" + oldPrefix;
            var spritenameRegex = new System.Text.RegularExpressions.Regex(spritenamePattern);
            string replacementText = "$1" + newPrefix;
            return spritenameRegex.Replace(metafileText, replacementText);
        }

        private void OnEnable()
        {
            this.oldSpritePrefix = System.IO.Path.GetFileNameWithoutExtension(SelectedObject.name);
            this.newSpritePrefix = this.oldSpritePrefix;
        }

        private void OnGUI()
        {
            EditorGUILayout.HelpBox(
                "This SpritesheetRename tool is used to rename a texture that's been autosliced. It replaces " +
                "all instances of the old prefix with a new one, and renames the Texture to match.",
                MessageType.None);
            this.oldSpritePrefix = EditorGUILayout.TextField("Prefix to Replace", this.oldSpritePrefix);
            this.newSpritePrefix = EditorGUILayout.TextField("New Sprite Prefix", this.newSpritePrefix);

            if (GUILayout.Button("Rename"))
            {
                RenameSelectedTexture(this.oldSpritePrefix, this.newSpritePrefix);
                this.Close();
            }
        }
    }
}