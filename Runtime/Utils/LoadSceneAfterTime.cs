using UnityEngine;
using System.Collections;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// This component loads the <see cref="scene"/> after <see cref="time"/>.
    /// <para>You can customize the transition params by setting <see cref="transition"/>.</para>
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class LoadSceneAfterTime : MonoBehaviour
    {
        [Tooltip("The Scene Manager")]
        public SceneManager sceneManager;
        [Tooltip("The new scene to load."), Scene]
        public string scene;
        [Tooltip("The scene transition data to use. The default one will be used if none is set.")]
        public SceneTransition transition;
        [Tooltip("Time (in seconds) to wait before load the new scene."), Min(0F)]
        public float time = 1f;

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(time);

            if (transition) _ = sceneManager.LoadScene(scene, transition);
            else _ = sceneManager.LoadScene(scene);
        }
    }
}