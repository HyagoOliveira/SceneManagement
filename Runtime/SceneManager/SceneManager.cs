using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using ActionCode.Attributes;
using ActionCode.AwaitableCoroutines;
using SceneMode = UnityEngine.SceneManagement.LoadSceneMode;
using UnitySceneManager = UnityEngine.SceneManagement.SceneManager;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// The Scene Manager. It loads new Scenes by first opening a LoadingScene and showing fade transitions.
    /// </summary>
    [CreateAssetMenu(fileName = "SceneManager", menuName = "ActionCode/SceneManager/Scene Manager", order = 110)]
    public sealed class SceneManager : ScriptableObject
    {
        [CreateButton(typeof(SceneTransition))]
        [Tooltip("De default Scene Transition values used when none is provided.")]
        public SceneTransition defaultTransition;

        /// <summary>
        /// Action fired when loading is started.
        /// </summary>
        public event Action OnLoadingStarted;

        /// <summary>
        /// Action fired when loading is finished.
        /// </summary>
        public event Action OnLoadingFinished;

        /// <summary>
        /// Action fired when the loading progress changes.
        /// <para>
        /// The given float param is the progress in percentage, 
        /// i.e. a number between 0F and 100F.
        /// </para>
        /// </summary>
        public event Action<float> OnProgressChanged;

        /// <summary>
        /// Whether a Scene loading process is happening. 
        /// </summary>
        public bool IsLoading { get; private set; }

        /// <summary>
        /// Whether the Scene loading process is locked. 
        /// </summary>
        public bool IsLoadingLocked { get; private set; }

        private void OnDisable()
        {
            IsLoading = false;
            UnlockLoading();
        }

        /// <summary>
        /// Loads the given Scene while showing a LoadingScene and fade 
        /// transitions using the given transition data.
        /// </summary>
        /// <param name="scene">The name or path of the Scene to load.</param>
        /// <param name="transition">The transition data to use.</param>
        public void LoadScene(string scene, SceneTransition transition = null) =>
            _ = LoadSceneAsync(scene, transition != null ? transition : defaultTransition);

        public async Task LoadSceneAsync(string scene) => await LoadSceneAsync(scene, defaultTransition);

        /// <summary>
        /// Loads the given Scene asynchronously while showing a LoadingScene and fade 
        /// transitions using the given transition data.
        /// </summary>
        /// <param name="scene"><inheritdoc cref="LoadSceneAsync(string)"/></param>
        /// <param name="transition">The transition data to use.</param>
        /// <returns><inheritdoc cref="LoadSceneAsync(string)"/></returns>
        public async Task LoadSceneAsync(string scene, SceneTransition transition) =>
            await AwaitableCoroutine.Run(LoadSceneCoroutine(scene, transition));

        /// <summary>
        /// Locks the Scene loading process.
        /// <para>
        /// Use it to lock into the Loading Scene until the <see cref="UnlockLoading"/> is called.
        /// </para>
        /// </summary>
        public void LockLoading() => IsLoadingLocked = true;

        /// <summary>
        /// Unlocks the Scene loading process.
        /// </summary>
        public void UnlockLoading() => IsLoadingLocked = false;

        private IEnumerator LoadSceneCoroutine(string scene, SceneTransition transition)
        {
            if (IsLoading)
                throw new Exception("Cannot load new Scene since other scene is being loaded.");

            if (transition == null) transition = CreateInstance<SceneTransition>();

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

        private void ReportProgress(float progress) => OnProgressChanged?.Invoke(progress * 100F);
    }
}