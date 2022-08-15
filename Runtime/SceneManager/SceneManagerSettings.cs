using System;
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
        private AbstractScreenFader screenFader;

        public event Action<float> OnProgressChanged;

        public string LoadingScene => loadingScene;
        public IScreenFader Fader { get; private set; }

        private readonly ITask task = TaskFactory.Create();

        public void Awake()
        {
            // ScriptableObject.Awake() functions are only called when the game is launched on Builds.
            // SceneManagerSettingsProvider.AwakeSettings() will force to call this function
            // when entering in Play Mode using the Editor.
            if (Application.isPlaying) CheckFaderInstance();
        }

        public bool HasLoadingScene() => !string.IsNullOrEmpty(loadingScene);

        public async Task LoadScene(string scene)
        {
            await Fader?.FadeOut();

            IProgress<float> progress = new Progress<float>(ReportProgress);

            if (HasLoadingScene())
            {
                // will automatically unload the previous Scene.
                var loadingSceneOperation = SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Single);
                await loadingSceneOperation.WaitUntilIsDone();

                progress.Report(0F);
                await Fader?.FadeIn();
            }

            await task.Delay(timeBeforeLoading);

            var loading = SceneManager.LoadSceneAsync(scene, LoadSceneMode.Single);
            // will prevent to automatically unload the Loading Scene.
            loading.allowSceneActivation = false;

            await loading.WaitUntilActivationProgress(progress);

            progress.Report(1F);
            await task.Delay(timeAfterLoading);

            if (HasLoadingScene()) await Fader?.FadeOut();

            // will automatically unload the Loading Scene.
            loading.allowSceneActivation = true;
            await Fader?.FadeIn();
        }

        private void ReportProgress(float progress) => OnProgressChanged?.Invoke(progress * 100F);

        private void CheckFaderInstance()
        {
            var abstractFader = FindObjectOfType<AbstractScreenFader>(includeInactive: true);
            if (abstractFader != null)
            {
                Fader = abstractFader;
                DontDestroyOnLoad(abstractFader.gameObject);
                return;
            }

            if (screenFader == null)
            {
                Debug.LogWarning("No Screen Fader Prefab set. " +
                    "Set one in Project Settings > ActionCode > Scene Manager.");
                return;
            }

            var prefab = screenFader.gameObject;
            var gameObject = Instantiate(prefab);

            Fader = gameObject.GetComponent<AbstractScreenFader>(); ;
            gameObject.name = prefab.name;

            DontDestroyOnLoad(gameObject);
        }
    }
}