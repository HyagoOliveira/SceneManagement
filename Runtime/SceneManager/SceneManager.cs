using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using ActionCode.AwaitableCoroutines;
using SceneMode = UnityEngine.SceneManagement.LoadSceneMode;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// The Scene Manager. 
    /// It loads new Scenes by first opening a LoadingScene and showing fade transitions.
    /// </summary>
    public static class SceneManager
    {
        /// <summary>
        /// Action fired when loading is started.
        /// </summary>
        public static event Action OnLoadingStarted;

        /// <summary>
        /// Action fired when loading is finished.
        /// </summary>
        public static event Action OnLoadingFinished;

        /// <summary>
        /// Action fired when the loading progress changes.
        /// <para>
        /// The given float param is the progress in percentage, 
        /// i.e. a number between 0F and 100F.
        /// </para>
        /// </summary>
        public static event Action<float> OnProgressChanged;

        /// <summary>
        /// Whether a Scene loading process is happening. 
        /// </summary>
        public static bool IsLoading { get; private set; }

        /// <summary>
        /// Whether the Scene loading process is locked. 
        /// </summary>
        public static bool IsLoadingLocked { get; private set; }

        /// <summary>
        /// Locks the Scene loading process.
        /// <para>
        /// Use it to lock into the Loading Scene until the <see cref="UnlockLoading"/> is called.
        /// </para>
        /// </summary>
        public static void LockLoading() => IsLoadingLocked = true;

        /// <summary>
        /// Unlocks the Scene loading process.
        /// </summary>
        public static void UnlockLoading() => IsLoadingLocked = false;

        /// <summary>
        /// Loads the given Scene using a Loading Scene and fade transitions.
        /// </summary>
        /// <param name="scene">The name or path of the Scene to load.</param>
        /// <param name="transition">The transition data to use.</param>
        public static void LoadScene(string scene, SceneTransition transition = null) =>
            _ = LoadSceneAsync(scene, transition);

        /// <summary>
        /// Loads the given Scene asynchronously using a Loading Scene and fade transitions.
        /// </summary>
        /// <param name="scene">The name or path of the Scene to load</param>
        /// <param name="transition">The transition data to use.</param>
        /// <returns>An asynchronously Task.</returns>
        public static async Task LoadSceneAsync(string scene, SceneTransition transition = null) =>
            await AwaitableCoroutine.Run(LoadSceneCoroutine(scene, transition));


        private static IEnumerator LoadSceneCoroutine(string scene, SceneTransition transition)
        {
            if (IsLoading)
                throw new Exception("Cannot load new Scene since other scene is being loaded.");

            if (transition == null) transition = ScriptableObject.CreateInstance<SceneTransition>();

            transition.Initialize();

            IsLoading = true;
            OnLoadingStarted?.Invoke();

            var hasLoadingScene = transition.HasLoadingScene();

            yield return transition.ScreenFader?.FadeOut();
            IProgress<float> progress = new Progress<float>(ReportProgress);

            if (hasLoadingScene)
            {
                // Automatically unload the previous Scene.
                var loadingSceneOperation = UnitySceneManager.LoadSceneAsync(transition.LoadingScene, SceneMode.Single);

                // Check if LoadingScene was valid.
                if (loadingSceneOperation == null)
                {
                    IsLoading = false;
                    yield break;
                }

                yield return loadingSceneOperation;

                progress.Report(0F);
                yield return transition.ScreenFader?.FadeIn();
            }

            yield return new WaitForSeconds(transition.TimeBeforeLoading);

            var loadingOperation = UnitySceneManager.LoadSceneAsync(scene);

            // Check if scene was valid.
            if (loadingOperation == null)
            {
                IsLoading = false;
                yield break;
            }

            // Prevent to automatically unload the LoadingScene if any.
            loadingOperation.allowSceneActivation = false;

            yield return loadingOperation.WaitUntilActivationProgress(progress);

            progress.Report(1F);
            yield return new WaitForSeconds(transition.TimeAfterLoading);
            yield return new WaitUntil(() => !IsLoadingLocked);

            if (hasLoadingScene) yield return transition.ScreenFader?.FadeOut();

            // Automatically unload the LoadingScene if any.
            loadingOperation.allowSceneActivation = true;

            yield return new WaitUntil(() => loadingOperation.isDone);

            yield return transition.ScreenFader?.FadeIn();

            IsLoading = false;
            OnLoadingFinished?.Invoke();
        }

        private static void ReportProgress(float progress) => OnProgressChanged?.Invoke(progress * 100F);
    }
}