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
        /// Awaits until the operation reaches its activation progress.
        /// </summary>
        /// <param name="progress">An implementation to report progress.</param>
        /// <returns>An operation that will be completed when it reaches its activation progress.</returns>
        public static async Task WaitUntilActivationProgress(this AsyncOperation operation, IProgress<float> progress = null)
        {
            // The real loading progress will be between 0F to 0.9F until the new scene is activated.
            const float activationProgress = 0.9F;
            while (operation.progress < activationProgress)
            {
                progress?.Report(operation.progress + 0.1F);
                await Task.Yield();
            }
        }

        /// <summary>
        /// Awaits until the operation is marked as done.
        /// </summary>
        /// <param name="operation"></param>
        /// <returns>An operation that will be completed when it is done.</returns>
        public static async Task WaitUntilIsDone(this AsyncOperation operation)
        {
            while (!operation.isDone)
                await Task.Yield();
        }
    }
}