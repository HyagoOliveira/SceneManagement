using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

namespace ActionCode.SceneManagement.Editor
{
    /// <summary>
    /// Checks if the Loading Scene from all <see cref="ScriptableSceneTransitionData"/> has been add to the build./>
    /// </summary>
    public sealed class ScriptableSceneTransitionBuilder : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport _)
        {
            var data = FindAllSceneTransitionData();
            var hasAnyData = data.Length > 0;
            if (hasAnyData) CheckForLoadingScenes(data);
        }

        private static void CheckForLoadingScenes(ScriptableSceneTransitionData[] transitionData)
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
                        $"To add this scene to the Build Settings use the menu File > Build Settings.";
                    throw new BuildFailedException(error);
                }
            }
        }

        private static ScriptableSceneTransitionData[] FindAllSceneTransitionData()
        {
            var filter = $"t:{typeof(ScriptableSceneTransitionData).Name}";
            var guids = AssetDatabase.FindAssets(filter);
            var transitionData = new ScriptableSceneTransitionData[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                transitionData[i] = AssetDatabase.LoadAssetAtPath<ScriptableSceneTransitionData>(path);
            }

            return transitionData;
        }
    }
}