#if !UNITY_WEBGL || UNITY_EDITOR
#define ASYNCHRONOUS_PLATFORM
#endif

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Static factory class for <see cref="ITask"/>.
    /// </summary>
    public static class TaskFactory
    {
        /// <summary>
        /// Creates an <see cref="ITask"/> instance based on the current platform.
        /// <para>
        /// If current platform is <b>WebGL</b>, a <see cref="SynchronousTask"/> 
        /// instance will be returned since it does not support multi-threads.
        /// </para>
        /// </summary>
        /// <returns></returns>
        public static ITask Create()
        {
#if ASYNCHRONOUS_PLATFORM
            return new AsynchronousTask();
#else
            return new SynchronousTask();
#endif
        }
    }
}