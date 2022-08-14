using UnityEngine;
using System.Collections;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Fader component for a Screen transition.
    /// <para>Fades the screen in and out using the local <see cref="CanvasGroup"/> component.</para>
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class ScreenFaderCanvas : AbstractScreenFader
    {
        [SerializeField, Tooltip("Local Canvas Group component.")]
        private CanvasGroup canvasGroup;
        [Min(0F), Tooltip("Time (in seconds) to fade the screen.")]
        public float duration = 0.5F;

        public const float FADE_IN_FINAL_ALPHA = 0F;
        public const float FADE_OUT_FINAL_ALPHA = 1F;

        private bool hasFadedIn;
        private bool hasFadedOut;

        protected override void Reset()
        {
            base.Reset();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        /// <summary>
        /// Fades in the screen.
        /// </summary>
        public override void FadeIn()
        {
            var fadeInCourotine = FadeScreen(FADE_IN_FINAL_ALPHA, duration);
            StartCoroutine(fadeInCourotine);
        }

        /// <summary>
        /// Fades out the screen.
        /// </summary>
        public override void FadeOut()
        {
            var fadeOutCoroutine = FadeScreen(FADE_OUT_FINAL_ALPHA, duration);
            StartCoroutine(fadeOutCoroutine);
        }

        /// <summary>
        /// Checks if the screen has completely fades in.
        /// </summary>
        /// <returns>True if the screen has fades in. False otherwise.</returns>
        public override bool HasFadedIn() => hasFadedIn;

        /// <summary>
        /// Checks if the screen has completely fades out.
        /// </summary>
        /// <returns>True if the screen has fades out. False otherwise.</returns>
        public override bool HasFadedOut() => hasFadedOut;

        private IEnumerator FadeScreen(float finalAlpha, float duration)
        {
            var currentFadeTime = 0F;
            var startAlpha = canvasGroup.alpha;

            hasFadedIn = hasFadedOut = false;

            while (currentFadeTime < duration)
            {
                var interpolation = currentFadeTime / duration;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, finalAlpha, interpolation);
                currentFadeTime += Time.unscaledDeltaTime;
                yield return null;
            }

            canvasGroup.alpha = finalAlpha;
            hasFadedIn = Mathf.Approximately(canvasGroup.alpha, FADE_IN_FINAL_ALPHA);
            hasFadedOut = Mathf.Approximately(canvasGroup.alpha, FADE_OUT_FINAL_ALPHA);
        }
    }
}