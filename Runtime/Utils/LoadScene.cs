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
        [Tooltip("The scene transition to use. The default one will be used if none is set.")]
        public SceneTransition transition;

        [Space]
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
        /// If the local <see cref="transition"/> is not set, 
        /// the <see cref="SceneManager.defaultTransition"/> will be used instead.
        /// </para>
        /// </summary>
        public void Load() => StartCoroutine(LoadCoroutine(scene));

        private IEnumerator LoadCoroutine(string scene)
        {
            yield return new WaitForSeconds(time);
            SceneManager.LoadScene(scene, transition);
        }
    }
}