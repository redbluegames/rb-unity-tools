namespace RedBlueGames.Tools
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public static class SceneManager
    {
        public enum SceneKey
        {
            Game,
        }

        private static Dictionary<SceneKey, SceneData> buildScenes = new Dictionary<SceneKey, SceneData>
        {
            { SceneKey.Game, new SceneData("Game", "Assets/_Scenes/Game.unity") },
        };

        public static void LoadScene(SceneKey sceneKey)
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene((int)sceneKey);
        }

        public static string GetSceneName(SceneKey sceneKey)
        {
            return buildScenes[sceneKey].DisplayName;
        }

        public static string GetPathForScene(SceneKey sceneKey)
        {
            return buildScenes[sceneKey].Path;
        }

        public static string[] GetAllPaths()
        {
            var paths = new string[buildScenes.Keys.Count];
            int i = 0;
            foreach (var key in buildScenes)
            {
                paths[i++] = key.Value.Path;
            }

            return paths;
        }

        private struct SceneData
        {
            public string DisplayName;
            public string Path;

            public SceneData(string name, string path)
            {
                this.DisplayName = name;
                this.Path = path;
            }
        }
    }
}