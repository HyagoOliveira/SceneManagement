namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Interface used on objects able to have Scene Transition data.
    /// </summary>
    public interface ISceneTransitionData
    {
        /// <summary>
        /// Time (in seconds) to wait before starts the loading process.
        /// </summary>
        float TimeBeforeLoading { get; }

        /// <summary>
        /// Time (in seconds) to wait after the loading process has finished.
        /// </summary>
        float TimeAfterLoading { get; }

        /// <summary>
        /// The Loading Scene name (the scene that will display the loading process).
        /// </summary>
        string LoadingScene { get; }

        /// <summary>
        /// The Screen Fader instance used to fade the screen in/out before changing Scenes.
        /// </summary>
        IScreenFader ScreenFader { get; }
    }
}