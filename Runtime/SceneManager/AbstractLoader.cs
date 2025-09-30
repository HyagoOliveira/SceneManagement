using UnityEngine;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Abstract loader for scene.
    /// <para>
    /// Implement this component to execute any asynchronous operation when a Scene is finishing to load,
    /// as soon after the new Scene is activated and before the screen fades in (the scene content appears).
    /// </para>
    /// </summary>
    /// <remarks>
    /// Check for <see cref="SceneManager.WaitForAllLoaders"/>.
    /// </remarks>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(-1)]
    public abstract class AbstractLoader : MonoBehaviour
    {
        public bool IsLoaded { get; private set; }

        protected virtual void Awake() => Load();

        /// <summary>
        /// Loads the remain Scene content.
        /// </summary>
        public async void Load()
        {
            try
            {
                IsLoaded = false;
                await LoadAsync();
            }
            catch (System.Exception e)
            {
                Debug.LogException(e);
            }
            finally
            {
                IsLoaded = true;
            }
        }

        /// <summary>
        /// Loads the remain Scene content asynchronously.
        /// </summary>
        /// <returns>An asynchronously operation.</returns>
        protected abstract Awaitable LoadAsync();
    }
}