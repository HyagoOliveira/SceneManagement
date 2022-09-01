using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using ActionCode.AwaitableCoroutines;
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

        public async Task LoadScene(string scene) => await LoadScene(scene, defaultTransition);

        public async Task LoadScene(string scene, SceneTransition data)
        {
            try
            {
                await AwaitableCoroutine.Run(LoadSceneCoroutine(scene, data));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IEnumerator LoadSceneCoroutine(string scene, SceneTransition data)
        {
            if (IsLoading)
                throw new Exception($"Cannot load {scene} since other scene is being loaded.");

            IsLoading = true;
            var hasLoadingScene = !string.IsNullOrEmpty(data.LoadingScene);

            yield return data.ScreenFader?.FadeOut();
            IProgress<float> progress = new Progress<float>(ReportProgress);

            if (hasLoadingScene)
            {
                // will automatically unload the previous Scene.
                var loadingSceneOperation = UnitySceneManager.LoadSceneAsync(data.LoadingScene);
                yield return loadingSceneOperation;

                progress.Report(0F);
                yield return data.ScreenFader?.FadeIn();
            }

            yield return new WaitForSeconds(data.TimeBeforeLoading);

            var loadingOperation = UnitySceneManager.LoadSceneAsync(scene);
            if (loadingOperation == null) yield break;

            // will prevent to automatically unload the data.LoadingScene.
            loadingOperation.allowSceneActivation = false;

            yield return loadingOperation.WaitUntilActivationProgress(progress);

            progress.Report(1F);
            yield return new WaitForSeconds(data.TimeAfterLoading);

            if (hasLoadingScene) yield return data.ScreenFader?.FadeOut();

            // will automatically unload the data.LoadingScene.
            loadingOperation.allowSceneActivation = true;
            yield return data.ScreenFader?.FadeIn();

            IsLoading = false;
        }

        private void ReportProgress(float progress) => OnProgressChanged?.Invoke(progress * 100F);

    }
}