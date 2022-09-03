using UnityEditor;
using UnityEditor.Build;
using UnityEditor.Build.Reporting;
using UnityEngine.SceneManagement;

namespace ActionCode.SceneManagement.Editor
{
    /// <summary>
    /// Checks if the Loading Scene from all <see cref="SceneTransition"/> has been add to the build./>
    /// </summary>
    public sealed class SceneTransitionDataBuilder : IPreprocessBuildWithReport
    {
        public int callbackOrder => 0;

        public void OnPreprocessBuild(BuildReport _)
        {
            var transitions = FindAllSceneTransitionAssets();
            var hasAnyTransition = transitions.Length > 0;
            if (hasAnyTransition) CheckForLoadingScenes(transitions);
        }

        private static void CheckForLoadingScenes(SceneTransition[] transitions)
        {
            foreach (var transition in transitions)
            {
                var noLoadingScene = !transition.HasLoadingScene();
                if (noLoadingScene) continue;

                var sceneIndex = SceneUtility.GetBuildIndexByScenePath(transition.LoadingScene);
                var isValidScene = sceneIndex != -1;
                if (isValidScene) continue;

                var assetPath = AssetDatabase.GetAssetPath(transition);
                var error = $"Asset '{assetPath}' has the Loading Scene '{transition.LoadingScene}' which " +
                    $"was not add to the Build Settings. This Loading Scene cannot be loaded at runtime.\n" +
                    $"To fix this, use the menu File > Build Settings to add this scene to the Build Settings.";
                throw new BuildFailedException(error);
            }
        }

        private static SceneTransition[] FindAllSceneTransitionAssets()
        {
            var filter = $"t:{typeof(SceneTransition).Name}";
            var guids = AssetDatabase.FindAssets(filter);
            var transitions = new SceneTransition[guids.Length];

            for (int i = 0; i < guids.Length; i++)
            {
                var path = AssetDatabase.GUIDToAssetPath(guids[i]);
                transitions[i] = AssetDatabase.LoadAssetAtPath<SceneTransition>(path);
            }
            return transitions;
        }
    }
}