using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        public AbstractScreenFader Fader { get; private set; }

        private bool isLoading;
        private SceneManagerBehaviour behaviour;

        public bool HasLoadingScene() => !string.IsNullOrEmpty(loadingScene);

        public async Task LoadScene(string scene)
        {
            CheckInstances();
            if (isLoading) throw new Exception($"Cannot load {scene} since other scene is being loaded.");

            behaviour.StartCoroutine(LoadSceneCoroutine(scene));
            while (isLoading) await Task.Yield();
        }

        private IEnumerator LoadSceneCoroutine(string scene)
        {
            isLoading = true;

            yield return FadeScreenOut();
            IProgress<float> progress = new Progress<float>(ReportProgress);

            if (HasLoadingScene())
            {
                // will automatically unload the previous Scene.
                var loadingSceneOperation = SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Single);
                yield return loadingSceneOperation;

                progress.Report(0F);
                yield return FadeScreenIn();
            }

            yield return new WaitForSeconds(timeBeforeLoading);

            var loadingOperation = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            // will prevent to automatically unload the Loading Scene.
            loadingOperation.allowSceneActivation = false;

            yield return loadingOperation.WaitUntilActivationProgress(progress);

            progress.Report(1F);
            yield return new WaitForSeconds(timeAfterLoading);

            if (HasLoadingScene()) yield return FadeScreenOut();

            // will automatically unload the Loading Scene.
            loadingOperation.allowSceneActivation = true;
            yield return FadeScreenIn();

            isLoading = false;
        }

        private IEnumerator FadeScreenIn() => Fader ? Fader.FadeIn() : null;
        private IEnumerator FadeScreenOut() => Fader ? Fader.FadeOut() : null;

        private void ReportProgress(float progress) => OnProgressChanged?.Invoke(progress * 100F);

        private void CheckInstances()
        {
            CheckBehaviourInstance();
            CheckFaderInstance();
        }

        private void CheckBehaviourInstance()
        {
            if (behaviour) return;
            behaviour = GetOrStaticCreate<SceneManagerBehaviour>("SceneManager");
            behaviour.Settings = this;
        }

        private void CheckFaderInstance()
        {
            if (Fader != null) return;
            if (screenFaderPrefab == null)
            {
                Debug.LogWarning("No Screen Fader Prefab set. " +
                    "Set one in Project Settings > ActionCode > Scene Manager.");
                return;
            }

            Fader = GetOrStaticCreate<AbstractScreenFader>(screenFaderPrefab.gameObject);
        }

        private static T GetOrStaticCreate<T>(string name) where T : Component
        {
            var component = FindObjectOfType<T>(includeInactive: true);
            if (component != null)
            {
                DontDestroyOnLoad(component.gameObject);
                return component;
            }

            var gameObject = new GameObject(name);
            DontDestroyOnLoad(gameObject);

            return gameObject.AddComponent<T>();
        }

        private static T GetOrStaticCreate<T>(GameObject prefab) where T : Component
        {
            var component = FindObjectOfType<T>(includeInactive: true);
            if (component != null)
            {
                DontDestroyOnLoad(component.gameObject);
                return component;
            }

            var gameObject = Instantiate(prefab);
            gameObject.name = prefab.name;
            DontDestroyOnLoad(gameObject);

            return gameObject.GetComponent<T>();
        }

        internal void Dispose()
        {
            Fader = null;
            behaviour = null;
            isLoading = false;
        }
    }
}