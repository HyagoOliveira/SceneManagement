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
        /// <returns>An enumerator that will wait until the operation reaches its activation progress.</returns>
        public static IEnumerator WaitUntilActivationProgress(this AsyncOperation operation, IProgress<float> progress = null)
        {
            // The real loading progress will be between 0F to 0.9F until the new scene is activated.
            const float activationProgress = 0.9F;
            while (operation.progress < activationProgress)
            {
                progress?.Report(operation.progress + 0.1F);
                yield return null;
            }
        }
    }
}