using System.Collections;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Interface used on objects able to fade the screen in and out.
    /// </summary>
    public interface IScreenFader
    {
        /// <summary>
        /// Fades the screen in (the Scene content will appear).
        /// </summary>
        /// <returns>An enumerator that will be completed when all screen content appears.</returns>
        IEnumerator FadeIn();

        /// <summary>
        /// Fades the screen out (the Scene content will disappear).
        /// </summary>
        /// <returns>An enumerator that will be completed when all screen content disappears.</returns>
        IEnumerator FadeOut();
    }
}