using UnityEngine;
using System.Threading.Tasks;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Synchronously Task.
    /// </summary>
    public sealed class SynchronousTask : ITask
    {
        public async Task Delay(float seconds)
        {
            var fps = 1.0f / Time.deltaTime;
            var totalFrames = seconds * fps;
            for (int i = 0; i < totalFrames; i++)
                await Task.Yield();
        }
    }
}