using UnityEngine;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Settings for Scenes loading.
    /// </summary>
    public sealed class SceneManagerSettings : ScriptableObject, ISceneManager
    {
        [Min(0F), Tooltip("Time (in seconds) to wait before starts the loading process.")]
        public float timeBeforeLoading = 0F;
        [Min(0F), Tooltip("Time (in seconds) to wait after the loading process has finished.")]
        public float timeAfterLoading = 0F;
        [Scene, Tooltip("The Loading Scene.")]
        public string loadingScene;
        [Tooltip("The Prefab containing an instance of AbstractScreenFader. It'll be instantiated at runtime.")]
        public AbstractScreenFader screenFader;

        private IScreenFader fader;

        /// <summary>
        /// Checks if <see cref="loadingScene"/> was set.
        /// </summary>
        /// <returns>Whether <see cref="loadingScene"/> was set.</returns>
        public bool HasLoadingScene() => !string.IsNullOrEmpty(loadingScene);

        public async Task LoadScene(string scene)
        {
            CheckScreenFaderInstance();
            await fader?.FadeOut();

            var loading = SceneManager.LoadSceneAsync(scene);
            loading.allowSceneActivation = false;

            await loading.WaitUntilSceneLoad();

            loading.allowSceneActivation = true;
            await fader?.FadeIn();
        }

        private void CheckScreenFaderInstance()
        {
            var fader = FindObjectOfType<AbstractScreenFader>(includeInactive: true);
            if (fader) return;

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