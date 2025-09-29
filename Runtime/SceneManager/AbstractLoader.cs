using UnityEngine;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Abstract loader for scene.
    /// <para>
    /// Implement this component to execute an asynchronous Loading operation as soon 
    /// after the new Scene is loaded (and activated) and before the screen fades in 
    /// (the scene content appears).
    /// </para>
    /// </summary>
    /// <remarks>
    /// Check for <see cref="SceneManager.WaitForAllLoaders"/>.
    /// </remarks>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-1)]
    public abstract class AbstractLoader : MonoBehaviour
    {
        protected virtual void Awake() => _ = LoadAsync();

        /// <summary>
        /// Loads the remain Scene content asynchronously.
        /// </summary>
        /// <returns>An asynchronously operation.</returns>
        public abstract Awaitable LoadAsync();
    }
}