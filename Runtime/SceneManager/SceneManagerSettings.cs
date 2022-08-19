using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;
using ActionCode.AwaitableCoroutines;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// The Scene Manager Settings.
    /// </summary>
    public sealed class SceneManagerSettings : ScriptableObject, ISceneManager
    {
        [SerializeField, Min(0F), Tooltip("Time (in seconds) to wait before starts the loading process.")]
        private float timeBeforeLoading = 0F;
        [SerializeField, Min(0F), Tooltip("Time (in seconds) to wait after the loading process has finished.")]
        private float timeAfterLoading = 0F;
        [SerializeField, Scene, Tooltip("The Loading Scene.")]
        private string loadingScene;
        [SerializeField, Tooltip("The Prefab containing an instance of AbstractScreenFader. It'll be instantiated at runtime.")]
        private AbstractScreenFader screenFaderPrefab;

        public event Action<float> OnProgressChanged;

        public string LoadingScene => loadingScene;
        public IScreenFader Fader => fader.Value;

        private bool isLoading;
        private Lazy<IScreenFader> fader;

        private void OnDisable() => isLoading = false;

        public bool HasLoadingScene() => !string.IsNullOrEmpty(loadingScene);

        public async Task LoadScene(string scene)
        {
            CheckLazyFader();

            try
            {
                await AwaitableCoroutine.Run(LoadSceneCoroutine(scene));
            }
            catch (Exception)
            {
                throw;
            }
        }

        private IEnumerator LoadSceneCoroutine(string scene)
        {
            if (isLoading)
            {
                Debug.LogError($"Cannot load {scene} since other scene is being loaded.");
                yield break;
            }

            isLoading = true;

            yield return Fader?.FadeOut();
            IProgress<float> progress = new Progress<float>(ReportProgress);

            if (HasLoadingScene())
            {
                // will automatically unload the previous Scene.
                var loadingSceneOperation = SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Single);
                yield return loadingSceneOperation;

                progress.Report(0F);
                yield return Fader?.FadeIn();
            }

            yield return new WaitForSeconds(timeBeforeLoading);

            var loadingOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            if (loadingOperation == null) yield break;

            // will prevent to automatically unload the Loading Scene.
            loadingOperation.allowSceneActivation = false;

            yield return loadingOperation.WaitUntilActivationProgress(progress);

            progress.Report(1F);
            yield return new WaitForSeconds(timeAfterLoading);

            if (HasLoadingScene()) yield return Fader?.FadeOut();

            // will automatically unload the Loading Scene.
            loadingOperation.allowSceneActivation = true;
            yield return Fader?.FadeIn();

            isLoading = false;
        }

        private void ReportProgress(float progress) => OnProgressChanged?.Invoke(progress * 100F);

        private void CheckLazyFader()
        {
            if (fader != null) return;
            fader = new Lazy<IScreenFader>(ScreenFaderFactory.Create(screenFaderPrefab));
        }
    }
}