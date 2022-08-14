using System.Threading.Tasks;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Interface used on objects able to fades the screen in and out.
    /// </summary>
    public interface IScreenFader
    {
        /// <summary>
        /// Fades the screen in (the content will appear).
        /// </summary>
        /// <returns>An operation that will be completed when all screen content appears.</returns>
    	Task FadeIn();

        /// <summary>
        /// Fades the screen out (the content will disappear).
        /// </summary>
        /// <returns>An operation that will be completed when all screen content disappears.</returns>
    	Task FadeOut();
    }
}