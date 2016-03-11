using UnityEngine;
using UnityEditor;

namespace RedBlueGames.Tools
{
    public class RenameSpritesheet : EditorWindow
    {
        string oldSpritePrefix;
        string newSpritePrefix;

        static Object selectedObject
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

        [MenuItem("Assets/Rename Spritesheet")]
        public static void ShowRenameSpritesheetWindow()
        {
            EditorWindow.GetWindow<RenameSpritesheet>(true, "Rename Texture", true);
        }

        [MenuItem("Assets/Rename Spritesheet", true)]
        public static bool IsSelectionTexture()
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

        void OnEnable()
        {
            oldSpritePrefix = System.IO.Path.GetFileNameWithoutExtension(selectedObject.name);
            newSpritePrefix = oldSpritePrefix;
        }

        void OnGUI()
        {
            EditorGUILayout.HelpBox("This SpritesheetRename tool is used to rename a texture that's been autosliced. It replaces " +
                "all instances of the old prefix with a new one, and renames the Texture to match.", MessageType.None);
            oldSpritePrefix = EditorGUILayout.TextField("Prefix to Replace", oldSpritePrefix);
            newSpritePrefix = EditorGUILayout.TextField("New Sprite Prefix", newSpritePrefix);

            if (GUILayout.Button("Rename"))
            {
                RenameSelectedTexture(oldSpritePrefix, newSpritePrefix);
                Close();
            }
        }

        static void RenameSelectedTexture(string oldName, string newName)
        {
            string path = AssetDatabase.GetAssetPath(selectedObject);
            string metaFile = System.IO.File.ReadAllText(path + ".meta");
            string ammendedMetaFile = ReplaceSpritePrefixInMetafile(metaFile, oldName, newName);
            System.IO.File.WriteAllText(path + ".meta", ammendedMetaFile);

            AssetDatabase.RenameAsset(path, newName);
            AssetDatabase.Refresh();
        }

        static string ReplaceSpritePrefixInMetafile(string metafileText, string prefixToReplace, string newPrefix)
        {
            string modifiedMetafile = ReplaceFileIDRecycleNames(metafileText, prefixToReplace, newPrefix);
            modifiedMetafile = ReplaceSpriteMetaData(metafileText, prefixToReplace, newPrefix);
            return modifiedMetafile;
        }

        static string ReplaceFileIDRecycleNames(string metafileText, string oldPrefix, string newPrefix)
        {
            string fileIDPattern = "([\\d]{8}: )" + oldPrefix;
            var fileIDRegex = new System.Text.RegularExpressions.Regex(fileIDPattern);
            string replacementText = "$1" + newPrefix;
            return fileIDRegex.Replace(metafileText, replacementText);
        }

        static string ReplaceSpriteMetaData(string metafileText, string oldPrefix, string newPrefix)
        {
            string spritenamePattern = "(- name: )" + oldPrefix;
            var spritenameRegex = new System.Text.RegularExpressions.Regex(spritenamePattern);
            string replacementText = "$1" + newPrefix;
            return spritenameRegex.Replace(metafileText, replacementText);
        }
    }
}