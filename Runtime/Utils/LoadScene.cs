using UnityEngine;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// This component was build to quickly load new Scenes. You may use it in Button actions.
    /// <para>You can customize the transition behaviour by setting <see cref="transition"/> attribute.</para>
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class LoadScene : MonoBehaviour
    {
        [Tooltip("The Scene Manager")]
        public SceneManager sceneManager;
        [Tooltip("The scene transition data to use. The default one will be used if none is set.")]
        public SceneTransition transition;

        /// <summary>
        /// Loads the given scene using <see cref="transition"/>. The default one will be used if none is set.
        /// </summary>
        /// <param name="scene">A Scene path or name to load.</param>
        public async void Load(string scene)
        {
            if (transition) await sceneManager.LoadScene(scene, transition);
            else await sceneManager.LoadScene(scene);
        }
    }
}