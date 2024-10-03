using UnityEngine;
using System.Collections;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// This component was build to quickly load new Scenes. You may use it in Button actions.
    /// <para>You can customize the transition behaviour by setting <see cref="transition"/> field.</para>
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class LoadScene : MonoBehaviour
    {
        [Tooltip("The Scene Manager")]
        public SceneManager sceneManager;
        [Tooltip("The new scene to load."), Scene]
        public string scene;
        [Tooltip("Time (in seconds) to wait before load the new scene."), Min(0F)]
        public float time = 0f;
        [Tooltip("If enabled, the Loading process will happening in Start() function.")]
        public bool loadOnStart = false;

        private void Start()
        {
            if (loadOnStart) Load();
        }

        /// <summary>
        /// Loads the <see cref="scene"/>.
        /// <para>
        /// If the local<see cref="transition"/> is not set, 
        /// the <see cref="SceneManager.defaultTransition"/> will be used instead.
        /// </para>
        /// </summary>
        public void Load() => Load(scene);

        /// <summary>
        /// Loads the given scene.
        /// <para><inheritdoc cref="Load"/></para>
        /// </summary>
        /// <param name="scene">A Scene path or name to load.</param>
        public void Load(string scene) => StartCoroutine(LoadCoroutine(scene));

        private IEnumerator LoadCoroutine(string scene)
        {
            // Using Coroutine because Task.Delay() is not supported in some Platforms (WebGL).
            yield return new WaitForSeconds(time);
            sceneManager.LoadScene(scene);
        }
    }
}