using System.Threading.Tasks;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Interface used on abjects able to be a Task.
    /// </summary>
    public interface ITask
    {
        /// <summary>
        /// Delays the task using the given time in seconds.
        /// </summary>
        /// <param name="seconds">The delay time in seconds.</param>
        /// <returns>A delay operation.</returns>
        Task Delay(float seconds);
    }
}