using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Fades the screen in and out using the local <see cref="CanvasGroup"/> component.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CanvasGroup))]
    public sealed class CanvasScreenFader : AbstractScreenFader
    {
        [SerializeField, Tooltip("Fade graphics image. It's used by the FadeColor.")]
        private Graphic image;
        [SerializeField, Tooltip("Local Canvas Group component.")]
        private CanvasGroup canvasGroup;
        [Min(0F), Tooltip("Time (in seconds) to fade the screen.")]
        public float duration = 0.5F;

        public const float FADE_IN_ALPHA = 0F;
        public const float FADE_OUT_ALPHA = 1F;

        /// <summary>
        /// The screen fade color.
        /// </summary>
        public Color FadeColor
        {
            get => image.color;
            set => image.color = value;
        }

        private void Reset()
        {
            image = GetComponentInChildren<Graphic>();
            canvasGroup = GetComponent<CanvasGroup>();
        }

        public override IEnumerator FadeIn() => FadeScreen(FADE_OUT_ALPHA, FADE_IN_ALPHA);
        public override IEnumerator FadeOut() => FadeScreen(FADE_IN_ALPHA, FADE_OUT_ALPHA);

        private IEnumerator FadeScreen(float startAlpha, float finalAlpha)
        {
            var currentFadeTime = 0F;
            while (currentFadeTime < duration)
            {
                var interpolation = currentFadeTime / duration;
                canvasGroup.alpha = Mathf.Lerp(startAlpha, finalAlpha, interpolation);
                currentFadeTime += Time.deltaTime;
                yield return null;
            }
            canvasGroup.alpha = finalAlpha;
        }
    }
}