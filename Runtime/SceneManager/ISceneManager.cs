using System;
using System.Threading.Tasks;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Interface used on objects able to be a Scene Manager.
    /// </summary>
    public interface ISceneManager
    {
        /// <summary>
        /// Action fired when the Scene loading progress changes.
        /// <para>
        /// The given float param is the progress in percentage, 
        /// i.e. a number between 0F and 100F.
        /// </para>
        /// </summary>
        event Action<float> OnProgressChanged;

        /// <summary>
        /// Whether a Scene loading process is happening. 
        /// </summary>
        bool IsLoading { get; }

        /// <summary>
        /// Loads the given Scene asynchronously while showing a default LoadingScene and fade transitions.
        /// </summary>
        /// <param name="scene">The name or path of the Scene to load.</param>
        /// <returns>A task operation of the loading process.</returns>
        Task LoadScene(string scene);

        /// <summary>
        /// Loads the given Scene asynchronously while showing a LoadingScene and fade 
        /// transitions using the given transition data.
        /// </summary>
        /// <param name="scene"><inheritdoc cref="LoadScene(string)"/></param>
        /// <param name="transition">The transition data to use.</param>
        /// <returns><inheritdoc cref="LoadScene(string)"/></returns>
        Task LoadScene(string scene, SceneTransition transition);
    }
}