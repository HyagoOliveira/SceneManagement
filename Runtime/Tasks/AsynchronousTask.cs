using System;
using System.Threading.Tasks;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Asynchronously Task.
    /// </summary>
    public sealed class AsynchronousTask : ITask
    {
        public async Task Delay(float seconds) =>
            await Task.Delay(TimeSpan.FromSeconds(seconds));
    }
}