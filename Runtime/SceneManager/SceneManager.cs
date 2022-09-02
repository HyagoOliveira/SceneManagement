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
    /// The Scene Manager. It loads new Scenes by first opening a LoadingScene and showing fade transitions.
    /// </summary>
    public sealed class SceneManager : ScriptableObject, ISceneManager
    {
        [Tooltip("De default Scene Transition values used when none is provided.")]
        public SceneTransition defaultTransition;

        public event Action<float> OnProgressChanged;

        public bool IsLoading { get; private set; }

        private void OnDisable() => IsLoading = false;

        public async Task LoadScene(string scene) => await LoadScene(scene, defaultTransition);

        public async Task LoadScene(string scene, SceneTransition transition)
        {
            try
            {
                await AwaitableCoroutine.Run(LoadSceneCoroutine(scene, transition));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IEnumerator LoadSceneCoroutine(string scene, SceneTransition transition)
        {
            if (IsLoading)
                throw new Exception("Cannot load new Scene since other scene is being loaded.");

            if (transition == null) transition = CreateInstance<SceneTransition>();

            transition.Initialize();

            IsLoading = true;
            var hasLoadingScene = transition.HasValidLoadingScene();

            yield return transition.ScreenFader?.FadeOut();
            IProgress<float> progress = new Progress<float>(ReportProgress);

            if (hasLoadingScene)
            {
                // Automatically unload the previous Scene.
                var loadingSceneOperation = UnitySceneManager.LoadSceneAsync(transition.LoadingScene, SceneMode.Single);
                yield return loadingSceneOperation;

                progress.Report(0F);
                yield return transition.ScreenFader?.FadeIn();
            }

            yield return new WaitForSeconds(transition.TimeBeforeLoading);

            var loadingOperation = UnitySceneManager.LoadSceneAsync(scene, SceneMode.Additive);

            // Check if scene was valid.
            if (loadingOperation == null) yield break;

            // Prevent to automatically unload the LoadingScene if any.
            loadingOperation.allowSceneActivation = false;

            yield return loadingOperation.WaitUntilActivationProgress(progress);

            progress.Report(1F);
            yield return new WaitForSeconds(transition.TimeAfterLoading);

            if (hasLoadingScene) yield return transition.ScreenFader?.FadeOut();

            // Automatically unload the LoadingScene if any.
            loadingOperation.allowSceneActivation = true;
            yield return transition.ScreenFader?.FadeIn();

            IsLoading = false;
        }

        private void ReportProgress(float progress) => OnProgressChanged?.Invoke(progress * 100F);

    }
}