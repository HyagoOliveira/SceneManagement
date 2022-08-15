using UnityEngine;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Internal component to execute coroutines inside <see cref="SceneManagerSettings"/>.
    /// </summary>
    [DisallowMultipleComponent]
    internal sealed class SceneManagerBehaviour : MonoBehaviour
    {
        internal SceneManagerSettings Settings { get; set; }

        private void OnDestroy()
        {
            if (Settings) Settings.Dispose();
        }
    }
}