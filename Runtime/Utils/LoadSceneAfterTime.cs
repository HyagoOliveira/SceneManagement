using UnityEngine;
using System.Collections;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// This component loads the <see cref="scene"/> after <see cref="time"/>.
    /// <para>You can customize the transition params by setting the local <see cref="LoadScene"/> component.</para>
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(LoadScene))]
    public sealed class LoadSceneAfterTime : MonoBehaviour
    {
        [Tooltip("The local LoadScene component.")]
        public LoadScene loader;
        [Tooltip("The new scene to load."), Scene]
        public string scene;
        [Tooltip("Time (in seconds) to wait before load the new scene."), Min(0F)]
        public float time = 1f;

        private void Reset() => loader = GetComponent<LoadScene>();

        private IEnumerator Start()
        {
            yield return new WaitForSeconds(time);
            loader.Load(scene);
        }
    }
}