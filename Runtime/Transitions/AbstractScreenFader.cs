using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Abstract component used on objects able to fades the screen in and out.
    /// </summary>
    public abstract class AbstractScreenFader : MonoBehaviour
    {
        [SerializeField, Tooltip("Fade graphics image. It's used by the FadeColor.")]
        protected Graphic image;

        /// <summary>
        /// The screen fade color.
        /// </summary>
        public Color FadeColor
        {
            get => image.color;
            set => image.color = value;
        }

        protected virtual void Reset()
        {
            image = GetComponentInChildren<Graphic>();
        }

        /// <summary>
        /// Waits until the screen has completely fades in.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> instance to be used inside a Coroutine.</returns>
        public IEnumerator WaitToFadeIn()
        {
            FadeIn();
            yield return new WaitUntil(HasFadedIn);
        }

        /// <summary>
        /// Waits until the screen has completely fades out.
        /// </summary>
        /// <returns>An <see cref="IEnumerator"/> instance to be used inside a Coroutine.</returns>
        public IEnumerator WaitToFadeOut()
        {
            FadeOut();
            yield return new WaitUntil(HasFadedOut);
        }

        /// <summary>
        /// Fades in the screen.
        /// </summary>
        public abstract void FadeIn();

        /// <summary>
        /// Fades out the screen.
        /// </summary>
        public abstract void FadeOut();

        /// <summary>
        /// Checks if the screen has completely fades in.
        /// </summary>
        /// <returns>True if the screen has fades in. False otherwise.</returns>
        public abstract bool HasFadedIn();

        /// <summary>
        /// Checks if the screen has completely fades out.
        /// </summary>
        /// <returns>True if the screen has fades out. False otherwise.</returns>
        public abstract bool HasFadedOut();
    }
}