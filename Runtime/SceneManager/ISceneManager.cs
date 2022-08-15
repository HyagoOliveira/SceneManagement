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
        /// Action fired when the loading progress changed.
        /// <para>The progress is on percentage.</para>
        /// </summary>
        event Action<float> OnProgressChanged;

        /// <summary>
        /// The Loading Scene.
        /// </summary>
        string LoadingScene { get; }

        /// <summary>
        /// The Screen Fader used to fade the screen in/out before change Scenes.
        /// </summary>
        IScreenFader Fader { get; }

        /// <summary>
        /// Checks if a Loading Scene was set.
        /// </summary>
        /// <returns>True if a Loading Scene was set. False otherwise.</returns>
        bool HasLoadingScene();

        Task LoadScene(string scene);
    }
}