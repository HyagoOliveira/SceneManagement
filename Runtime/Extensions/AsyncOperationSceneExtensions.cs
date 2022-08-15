using System;
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
        /// <param name="progress">An implementation to report progress.</param>
        /// <returns>An operation that will be completed when the scene completes the loading process.</returns>
        public static async Task WaitUntilSceneLoad(this AsyncOperation operation, IProgress<float> progress = null)
        {
            // The real loading progress will be between 0F to 0.9F until the new scene is activated.
            const float maxLoadingProgress = 0.9F;
            while (operation.progress < maxLoadingProgress)
            {
                progress?.Report(operation.progress + 0.1F);
                await Task.Yield();
            }
        }

        /// <summary>
        /// Waits until the given single scene completes the loading process.
        /// </summary>
        /// <param name="operation"></param>
        /// <returns>An operation that will be completed when the single scene completes the loading process.</returns>
        public static async Task WaitUntilSingleSceneLoad(this AsyncOperation operation)
        {
            while (!operation.isDone)
                await Task.Yield();
        }
    }
}