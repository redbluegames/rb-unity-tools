using UnityEngine;
using System.Collections;
using System.IO;

namespace RedBlueGames.Tools
{
    /// <summary>
    /// Ideal for static methods that could be called anywhere.
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// Opens the URL in a new window if webplayer build, otherwise uses the behavior
        /// built into Application.OpenURL which depends on the platform.
        /// </summary>
        /// <param name="url">URL.</param>
        /// <param name="windowTitle">Window title.</param>
        public static void OpenURL(string url)
        {
            if (Application.isWebPlayer)
            {
                string evalString = string.Format("window.open('{0}')", url);
                Application.ExternalEval(evalString);
            }
            else
            {
                Application.OpenURL(url);
            }
        }

        /// <summary>
        /// Copies the string to the OS buffer.
        /// </summary>
        /// <param name="copyString">String to copy.</param>
        public static void CopyStringToBuffer(string copyString)
        {
            TextEditor te = new TextEditor();
            te.text = copyString;
            te.SelectAll();
            te.Copy();
        }

        public static Texture2D ConvertFileToTexture2D(string path)
        {
            Texture2D texture = new Texture2D(0, 0, TextureFormat.ARGB32, false);
            byte[] readBytes;
            try
            {
                readBytes = File.ReadAllBytes(path);
            }
            catch (FileNotFoundException e)
            {
                Debug.LogError("Caught exception when trying to load texture from file: " + e.ToString());
                return null;
            }
            texture.LoadImage(readBytes);
            return texture;
        }

        public static void WriteTextureToDisk(Texture2D texture, string outputDirectory, string filename)
        {
            byte[] bytes = texture.EncodeToPNG();
            string path = outputDirectory + filename;
            Debug.LogWarning("Writing file to disk: " + path);
            try
            {
                System.IO.File.WriteAllBytes(path, bytes);
            }
            catch (System.Exception e)
            {
                Debug.LogWarning("Failed to write. Reason: " + e.Message);
            }
        }
    }
}