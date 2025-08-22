using System;
using UnityEngine;
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
        /// Action fired when loading is started. <see cref="LoadingScene"/> will be set.
        /// </summary>
        public static event Action OnLoadingStarted;

        /// <summary>
        /// Action fired when loading is finished. 
        /// <see cref="LoadingScene"/> will be null.
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
        /// Whether the Scene loading process is locked. 
        /// </summary>
        public static bool IsLoadingLocked { get; private set; }

        /// <summary>
        /// The current Scene been loaded.
        /// </summary>
        public static Scene LoadingScene { get; private set; }

        /// <summary>
        /// Whether a Scene loading process is happening. 
        /// </summary>
        public static bool IsLoading() => LoadingScene != null;

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
            LoadScene(new Scene(scene), transition);

        /// <summary>
        /// <inheritdoc cref="LoadScene(string, SceneTransition)"/>
        /// </summary>
        /// <param name="scene">The Scene to load.</param>
        /// <param name="transition"><inheritdoc cref="LoadScene(string, SceneTransition)" path="/param[@name='transition']"/></param>
        public static void LoadScene(Scene scene, SceneTransition transition = null) =>
            _ = LoadSceneAsync(scene, transition);

        /// <summary>
        /// Loads the given Scene asynchronously using a Loading Scene and fade transitions.
        /// </summary>
        /// <param name="scene"><inheritdoc cref="LoadScene(string, SceneTransition)" path="/param[@name='scene']"/></param>
        /// <param name="transition"><inheritdoc cref="LoadScene(string, SceneTransition)" path="/param[@name='transition']"/></param>
        /// <returns>An Awaitable Task.</returns>
        public static async Awaitable LoadSceneAsync(string scene, SceneTransition transition = null) =>
            await LoadSceneAsync(new Scene(scene), transition);

        /// <summary>
        /// <inheritdoc cref="LoadSceneAsync(string, SceneTransition)"/>
        /// </summary>
        /// <param name="scene"><inheritdoc cref="LoadScene(Scene, SceneTransition)" path="/param[@name='scene']"/></param>
        /// <param name="transition"><inheritdoc cref="LoadScene(string, SceneTransition)" path="/param[@name='transition']"/></param>
        /// <returns><inheritdoc cref="LoadSceneAsync(string, SceneTransition)"/></returns>
        public static async Awaitable LoadSceneAsync(Scene scene, SceneTransition transition = null)
        {
            try
            {
                await LoadSceneAsync_Internal(scene, transition);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                LoadingScene = null;
                OnLoadingFinished?.Invoke();
            }
        }

        public static async Awaitable LoadSceneAsync_Internal(Scene scene, SceneTransition transition)
        {
            Time.timeScale = 1f;

            if (IsLoading())
                throw new Exception($"Cannot load {scene} since {LoadingScene} is being loaded.");

            if (transition == null) transition = ScriptableObject.CreateInstance<SceneTransition>();

            transition.Initialize();

            LoadingScene = scene;
            OnLoadingStarted?.Invoke();

            var hasLoadingScene = transition.HasLoadingScene();

            if (transition.ScreenFader) await transition.ScreenFader.FadeOutAsync();
            IProgress<float> progress = new Progress<float>(ReportProgress);

            if (hasLoadingScene)
            {
                // Automatically unload the previous Scene.
                var loadingSceneOperation = UnitySceneManager.LoadSceneAsync(transition.LoadingScene, SceneMode.Single);
                var hasInvalidLoadingScene = loadingSceneOperation == null;

                if (hasInvalidLoadingScene)
                    throw new Exception($"Loading Scene {LoadingScene} is invalid.");

                await loadingSceneOperation;

                progress.Report(0F);
                if (transition.ScreenFader) await transition.ScreenFader.FadeInAsync();
            }

            await Awaitable.WaitForSecondsAsync(transition.TimeBeforeLoading);

            var loadingOperation = UnitySceneManager.LoadSceneAsync(scene.path);
            var hasInvalidScene = loadingOperation == null;

            if (hasInvalidScene)
                throw new Exception($"Scene {scene.path} is invalid.");

            // Prevent to automatically unload the LoadingScene if any.
            loadingOperation.allowSceneActivation = false;

            await loadingOperation.WaitUntilActivationProgress(progress);

            progress.Report(1F);

            await Awaitable.WaitForSecondsAsync(transition.TimeAfterLoading);
            await WaitUntilAsync(() => !IsLoadingLocked);

            if (hasLoadingScene && transition.ScreenFader)
                await transition.ScreenFader.FadeOutAsync();

            // Automatically unload the LoadingScene if any.
            loadingOperation.allowSceneActivation = true;

            await WaitUntilAsync(() => loadingOperation.isDone);
            if (transition.ScreenFader) await transition.ScreenFader.FadeInAsync();
        }

        private static void ReportProgress(float progress) => OnProgressChanged?.Invoke(progress * 100F);

        private static async Awaitable WaitUntilAsync(Func<bool> condition)
        {
            while (!condition()) await Awaitable.NextFrameAsync();
        }
    }
}