using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Specialization of <see cref="UnityEngine.SceneManagement.SceneManager"/>.
    /// <para>Loads Scenes at run-time using the Screen Fader and Loading Scene 
    /// from a given <see cref="SceneLoadingSettings"/>.</para>
    /// </summary>
    public sealed class SceneManager : UnityEngine.SceneManagement.SceneManager
    {
        /// <summary>
        /// If true, will lock the next scene activation until it is set to false.
        /// <para>For example, you can use it on your Loading Scenes after the loading process is done to:</para>
        /// <list type="bullet">
        /// <item>Wait an input to go to the next scene.</item>
        /// <item>Wait until an Animation is completely played.</item>
        /// </list>
        /// </summary>
        public static bool LockNextScene { get; set; }

        /// <summary>
        /// The Previous Scene. It is only available on the loading process.
        /// </summary>
        public static Scene PreviousScene { get; private set; }

        /// <summary>
        /// The Next Scene. It is only available on the loading process.
        /// </summary>
        public static Scene NextScene { get; private set; }

        /// <summary>
        /// Whether the loading process is happening.
        /// </summary>
        public static bool IsLoading { get; private set; }

        /// <summary>
        /// The current loading progress. The value is from 0F to 1F.
        /// </summary>
        public static float LoadingProgress { get; private set; } = 0F;

        /// <summary>
        /// The current loading progress as a percentage. The value is from 0F to 100F.
        /// </summary>
        public static float LoadingProgressPercentage => LoadingProgress * 100F;

        /// <summary>
        /// Loads a new scene and unloads the previous one.
        /// </summary>
        /// <param name="scene">The new scene name or path.</param>
        /// <param name="settings">Data container for loading Scenes.</param>
        public static void LoadScene(string scene, SceneLoadingSettings settings)
            => LoadScene(scene, -1, settings);

        /// <summary>
        /// Loads a new scene and unloads the previous one.
        /// </summary>
        /// <param name="sceneBuildIndex">The new scene build index.</param>
        /// <param name="settings">Data container for loading Scenes.</param>
        public static void LoadScene(int sceneBuildIndex, SceneLoadingSettings settings)
            => LoadScene(null, sceneBuildIndex, settings);

        /// <summary>
        /// Loads the Next Scene based on the Scenes presents on the Build Settings window.
        /// <para>Goes to the first scene if there is no next scene available.</para>
        /// </summary>
        /// <param name="settings">Data container for loading Scenes.</param>
        public static void LoadNextScene(SceneLoadingSettings settings)
        {
            var sceneIndex = GetActiveScene().buildIndex + 1;
            if (sceneIndex >= sceneCountInBuildSettings) sceneIndex = 0;
            LoadScene(sceneIndex, settings);
        }

        /// <summary>
        /// Loads the Previous Scene based on the Scenes presents on the Build Settings window.
        /// <para>Goes to the last scene if there is no previous scene available.</para>
        /// </summary>
        /// <param name="settings">Data container for loading Scenes.</param>
        public static void LoadPreviousScene(SceneLoadingSettings settings)
        {
            var sceneIndex = GetActiveScene().buildIndex - 1;
            if (sceneIndex < 0) sceneIndex = sceneCountInBuildSettings - 1;
            LoadScene(sceneIndex, settings);
        }

        /// <summary>
        /// Fades the screen out, invokes the given action and fades back the screen in.
        /// <para>It'll use the Screen Fader Prefab from the given settings.</para>
        /// </summary>
        /// <param name="settings">Data container for loading Scenes.</param>
        /// <param name="onFadeOut">Action invoked when the screen fade out completely and before the screen fades in.</param>
        /// <param name="fadeOutTime">The time (in seconds) to wait after the screen fades out and before it fades in.</param>
        public static void FadeScreen(SceneLoadingSettings settings, Action onFadeOut = null, float fadeOutTime = 0F)
        {
            settings.GetOrInstantiateScreenFader();
            FadeScreen(settings.ScreenFader, onFadeOut, fadeOutTime);
        }

        /// <summary>
        /// Fades the screen out, invokes the given action and fades back the screen in.
        /// </summary>
        /// <param name="screenFader">Screen Fader instance to be used.</param>
        /// <param name="onFadeOut">Action invoked when the screen fade out completely and before the screen fades in.</param>
        /// <param name="fadeOutTime">The time (in seconds) to wait after the screen fades out and before it fades in.</param>
        public static void FadeScreen(AbstractScreenFader screenFader, Action onFadeOut = null, float fadeOutTime = 0F)
        {
            var coroutine = FadeScreenCoroutine(onFadeOut, screenFader, fadeOutTime);
            SceneLoader.FindOrInstanciate().StartCoroutine(coroutine);
        }

        /// <summary>
        /// Coroutine to fades the screen and invokes the given action.
        /// </summary>
        private static IEnumerator FadeScreenCoroutine(Action onFadeOut, AbstractScreenFader screenFader, float fadeOutTime)
        {
            var hasScreenFader = screenFader != null;
            if (hasScreenFader) yield return screenFader.WaitToFadeOut();
            onFadeOut?.Invoke();
            yield return new WaitForSeconds(fadeOutTime);
            if (hasScreenFader) yield return screenFader.WaitToFadeIn();
        }

        /// <summary>
        /// The loading scene logic is made using this Coroutine.
        /// <para>If available, the screen fades out, the Loading Scene is loaded,
        /// the screen fades in, the next scene loading process begins until it completes
        /// then the screen fades out, the Loading Scene is unload and finally the screen fades in.</para>
        /// </summary>
        private static IEnumerator LoadSceneCoroutine(string scene, int sceneIndex, SceneLoadingSettings settings)
        {
            if (IsLoading) yield break;
            if (settings == null)
            {
                // Creates settings instance with all default values.
                settings = ScriptableObject.CreateInstance<SceneLoadingSettings>();
            }

            // The real loading progress will be between 0F to .9F until the new scene is activated.
            const float maxRealLoadingProgress = 0.9F;

            IsLoading = true;
            LoadingProgress = 0F;
            PreviousScene = GetActiveScene();
            // NextScene cannot be set here since GetSceneBy* functions only work for loaded scenes.

            settings.GetOrInstantiateScreenFader();
            var hasScreenFader = settings.HasScreenFader();
            var hasLoadingScene = settings.HasLoadingScene();

            if (hasScreenFader) yield return settings.ScreenFader.WaitToFadeOut();
            if (hasLoadingScene)
            {
                var waitToLoadLoadingScene = LoadSceneAsync(settings.loadingScene, LoadSceneMode.Single);
                var hasValidLoadingScene = waitToLoadLoadingScene != null;
                if (hasValidLoadingScene)
                {
                    yield return waitToLoadLoadingScene;
                    // Only fades in if the Loading Scene was found and it's valid (enabled inside BuildSettings).
                    if (hasScreenFader) yield return settings.ScreenFader.WaitToFadeIn();
                }
            }

            // Waits the time before loading.
            yield return new WaitForSeconds(settings.timeBeforeLoading);

            var hasScene = !string.IsNullOrEmpty(scene);
            var hasSceneIndex = sceneIndex > -1;
            var loadingNextSceneOperation = new AsyncOperation();

            if (hasScene)
            {
                loadingNextSceneOperation = LoadSceneAsync(scene);
                NextScene = GetSceneByName(scene);
                if (!NextScene.IsValid()) GetSceneByPath(scene);
            }
            else if (hasSceneIndex)
            {
                loadingNextSceneOperation = LoadSceneAsync(sceneIndex);
                NextScene = GetSceneByBuildIndex(sceneIndex);
            }

            // It'll be null if Loading Scene is invalid (disabled inside BuildSettings).
            var hasInvalidNextScene = loadingNextSceneOperation == null;
            if (hasInvalidNextScene) yield break;

            // Disallow activation to prevent the next scene activation after loading.
            loadingNextSceneOperation.allowSceneActivation = false;

            // The next scene real loading.
            while (IsLoading = loadingNextSceneOperation.progress < maxRealLoadingProgress)
            {
                // Clamps the progress from [0F -> .9F] to [0F -> 1F].
                LoadingProgress = Mathf.Clamp01(loadingNextSceneOperation.progress / maxRealLoadingProgress);
                yield return null;
            }
            LoadingProgress = 1F;

            // Waits until LockNextScene is false.
            yield return new WaitUntil(() => LockNextScene == false);

            // Waits the time after loading.
            yield return new WaitForSeconds(settings.timeAfterLoading);

            if (hasScreenFader) yield return settings.ScreenFader.WaitToFadeOut();
            // Unloads the Loading Scene if one was loaded.
            loadingNextSceneOperation.allowSceneActivation = true;
            if (hasScreenFader) yield return settings.ScreenFader.WaitToFadeIn();

            PreviousScene = NextScene = default;
        }

        private static void LoadScene(string scene, int sceneIndex, SceneLoadingSettings settings)
        {
            var coroutine = LoadSceneCoroutine(scene, sceneIndex, settings);
            SceneLoader.FindOrInstanciate().StartCoroutine(coroutine);
        }
    }
}