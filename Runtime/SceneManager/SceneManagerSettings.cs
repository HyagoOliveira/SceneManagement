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
        public IScreenFader Fader { get; private set; }
        private SceneManagerBehaviour Behaviour => behaviour.Value;

        private bool isLoading;
        private readonly Lazy<SceneManagerBehaviour> behaviour = new Lazy<SceneManagerBehaviour>(GetOrCreateBehaviour);

        private void OnDisable() => isLoading = false;

        public bool HasLoadingScene() => !string.IsNullOrEmpty(loadingScene);

        public async Task LoadScene(string scene)
        {
            CheckFader();
            if (isLoading) throw new Exception($"Cannot load {scene} since other scene is being loaded.");

            Behaviour.StartCoroutine(LoadSceneCoroutine(scene));
            while (isLoading) await Task.Yield();
        }

        private IEnumerator LoadSceneCoroutine(string scene)
        {
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

        private void CheckFader()
        {
            if (Fader != null) return;

            var fader = FindObjectOfType<AbstractScreenFader>(includeInactive: true);
            GameObject instance;

            if (fader) instance = fader.gameObject;
            else
            {
                if (screenFaderPrefab == null) return;

                var prefab = screenFaderPrefab.gameObject;
                instance = Instantiate(prefab);
                instance.name = prefab.name;
                fader = instance.GetComponent<AbstractScreenFader>();
            }

            Fader = fader;
            DontDestroyOnLoad(instance);
        }

        private static SceneManagerBehaviour GetOrCreateBehaviour()
        {
            var name = typeof(SceneManagerBehaviour).Name;
            return GetOrStaticCreate<SceneManagerBehaviour>(name, HideFlags.NotEditable);
        }

        private static T GetOrStaticCreate<T>(string name, HideFlags hideFlags = HideFlags.None) where T : Component
        {
            var component = FindObjectOfType<T>(includeInactive: true);
            var hasComponent = component != null;
            var gameObject = hasComponent ?
                component.gameObject :
                new GameObject(name);

            gameObject.hideFlags = hideFlags;
            DontDestroyOnLoad(gameObject);

            return hasComponent ? component : gameObject.AddComponent<T>();
        }
    }
}