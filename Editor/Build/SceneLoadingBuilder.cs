using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

namespace ActionCode.SceneManagement.Editor
{
    /// <summary>
    /// Checks if the Loading Scene from all <see cref="SceneLoadingSettings"/> has been add to the build./>
    /// </summary>
    public sealed class SceneLoadingBuilder : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport _)
        {
            var projectSettings = FindAllSceneLoaderSettings();
            var hasSettings = projectSettings.Length > 0;
            if (hasSettings)
            {
                CheckLoadingScenes(projectSettings);
            }
        }

        private static void CheckLoadingScenes(SceneLoadingSettings[] projectSettings)
        {
            foreach (var settings in projectSettings)
            {
                if (!settings.HasLoadingScene()) continue;

                var sceneIndex = SceneUtility.GetBuildIndexByScenePath(settings.loadingScene);
                var isInvalidScene = sceneIndex == -1;
                if (isInvalidScene)
                {
                    var msg = $"Asset '{settings.name}' has the Loading Scene '{settings.loadingScene}' that " +
                        $"was not add to the build settings.\n" +
                        $"To add a scene to the build settings use the menu File->Build Settings...";
                    // this will stop the build.
                    throw new BuildFailedException(msg);
                }
            }
        }

        private static SceneLoadingSettings[] FindAllSceneLoaderSettings()
        {
            var filter = $"t:{typeof(SceneLoadingSettings).Name}";
            var guids = AssetDatabase.FindAssets(filter);
            var settings = new SceneLoadingSettings[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                settings[i] = AssetDatabase.LoadAssetAtPath<SceneLoadingSettings>(path);
            }

            return settings;
        }
    }
}