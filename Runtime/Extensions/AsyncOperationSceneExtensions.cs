using System;
using System.Collections;
using UnityEngine;

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
        /// <param name="additionalProgress">Additional progress to report.</param>
        /// <returns>An enumerator that will wait until the operation reaches its activation progress.</returns>
        public static IEnumerator WaitUntilActivationProgress(this AsyncOperation operation, IProgress<float> progress = null, float additionalProgress = 0F)
        {
            // The real loading progress will be between 0F to 0.9F until the new scene is activated.
            const float activationProgress = 0.9F;
            while (operation.progress < activationProgress)
            {
                progress?.Report(operation.progress + 0.1F + additionalProgress);
                yield return null;
            }
        }

        /// <summary>
        /// Awaits until all operations reach their activation progress.
        /// </summary>
        /// <param name="operations"><inheritdoc cref="WaitUntilActivationProgress(AsyncOperation, IProgress{float}, float)"/></param>
        /// <param name="progress"><inheritdoc cref="WaitUntilActivationProgress(AsyncOperation, IProgress{float}, float)"/></param>
        /// <returns>An enumerator that will wait until all operations reach their activation progress.</returns>
        public static IEnumerator WaitUntilActivationProgress(this AsyncOperation[] operations, IProgress<float> progress)
        {
            for (int i = 0; i < operations.Length; i++)
            {
                var operation = operations[i];
                var operationProgress = i * operations.Length;
                yield return operation.WaitUntilActivationProgress(progress, operationProgress);
            }
        }
    }
}