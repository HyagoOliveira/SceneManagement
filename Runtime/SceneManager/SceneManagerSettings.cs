using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Settings for Scenes loading.
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

        public string LoadingScene => loadingScene;

        private IScreenFader fader;
        private readonly ITask task = TaskFactory.Create();

        public bool HasLoadingScene() => !string.IsNullOrEmpty(loadingScene);

        public async Task LoadScene(string scene)
        {
            CheckScreenFaderInstance();
            await fader?.FadeOut();

            if (HasLoadingScene())
            {
                var loadingSceneOperation = SceneManager.LoadSceneAsync(loadingScene, LoadSceneMode.Single);
                await loadingSceneOperation.WaitUntilSceneLoad();
                await fader?.FadeIn();
            }

            await task.Delay(timeBeforeLoading);

            var loading = SceneManager.LoadSceneAsync(scene);
            loading.allowSceneActivation = false;

            await loading.WaitUntilSceneLoad();

            await task.Delay(timeAfterLoading);

            if (HasLoadingScene()) await fader?.FadeOut();
            loading.allowSceneActivation = true;
            await fader?.FadeIn();
        }

        private void CheckScreenFaderInstance()
        {
            if (fader != null) return;

            fader = FindObjectOfType<AbstractScreenFader>(includeInactive: true);
            if (fader != null) return;

            if (screenFader == null)
            {
                Debug.Log("No Screen Fader Prefab set.");
                return;
            }

            var prefab = screenFader.gameObject;
            var gameObject = Instantiate(prefab);

            fader = gameObject.GetComponent<AbstractScreenFader>(); ;
            gameObject.name = prefab.name;

            DontDestroyOnLoad(gameObject);
        }
    }
}