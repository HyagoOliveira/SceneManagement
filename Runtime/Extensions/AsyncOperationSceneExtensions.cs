using UnityEngine;
using System.Threading.Tasks;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Extension Methods for Scenes AsyncOperations.
    /// </summary>
    public static class AsyncOperationSceneExtensions
    {
        /// <summary>
        /// Waits until the given scene completes the loading process.
        /// </summary>
        /// <param name="operation"></param>
        /// <returns>An operation that will be completed when the scene completes the loading process.</returns>
        public static async Task WaitUntilSceneLoad(this AsyncOperation operation)
        {
            // The real loading progress will be between 0F to 0.9F until the new scene is activated.
            const float maxLoadingProgress = 0.9F;
            while (operation.progress < maxLoadingProgress)
                await Task.Yield();
        }
    }
}