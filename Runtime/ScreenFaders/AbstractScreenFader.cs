using UnityEngine;
using System.Threading.Tasks;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Abstract component used on objects able to fade the screen in and out.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public abstract class AbstractScreenFader : MonoBehaviour
    {
        /// <summary>
        /// Fades the screen in (the content will appear).
        /// </summary>
        /// <returns>An operation that will be completed when all screen content appears.</returns>
        public abstract Task FadeIn();

        /// <summary>
        /// Fades the screen out (the content will disappear).
        /// </summary>
        /// <returns>An operation that will be completed when all screen content disappears.</returns>
        public abstract Task FadeOut();
    }
}