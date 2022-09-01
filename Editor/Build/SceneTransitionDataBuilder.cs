using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

namespace ActionCode.SceneManagement.Editor
{
    /// <summary>
    /// Checks if the Loading Scene from all <see cref="SceneTransitionData"/> has been add to the build./>
    /// </summary>
    public sealed class SceneTransitionDataBuilder : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport _)
        {
            var data = FindAllSceneTransitionData();
            var hasAnyData = data.Length > 0;
            if (hasAnyData) CheckForLoadingScenes(data);
        }

        private static void CheckForLoadingScenes(SceneTransitionData[] transitionData)
        {
            foreach (var data in transitionData)
            {
                var hasLoadingScene = !string.IsNullOrEmpty(data.LoadingScene);
                if (hasLoadingScene) continue;

                var sceneIndex = SceneUtility.GetBuildIndexByScenePath(data.LoadingScene);
                var isInvalidScene = sceneIndex == -1;
                if (isInvalidScene)
                {
                    var assetPath = AssetDatabase.GetAssetPath(data);
                    var error = $"Asset '{assetPath}' has the Loading Scene '{data.LoadingScene}' which " +
                        $"was not add to the Build Settings. This Loading Scene cannot be loaded at runtime.\n" +
                        $"Use the menu File > Build Settings to add this scene to the Build Settings.";
                    throw new BuildFailedException(error);
                }
            }
        }

        private static SceneTransitionData[] FindAllSceneTransitionData()
        {
            var filter = $"t:{typeof(SceneTransitionData).Name}";
            var guids = AssetDatabase.FindAssets(filter);
            var transitionData = new SceneTransitionData[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                transitionData[i] = AssetDatabase.LoadAssetAtPath<SceneTransitionData>(path);
            }

            return transitionData;
        }
    }
}