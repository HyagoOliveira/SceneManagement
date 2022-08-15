#if ANIMATION_MODULE
using UnityEngine;
using System.Collections;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Fades the screen in and out using the local <see cref="Animation"/> component.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Animation))]
    public sealed class AnimationScreenFader : AbstractScreenFader
    {
        [SerializeField, Tooltip("The local Animation component.")]
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        private Animation animation;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        [SerializeField, Tooltip("The local animation name used to fades the screen in.")]
        private string fadeInName = "AnimationScreenFader@FadeIn";
        [SerializeField, Tooltip("The local animation name used to fades the screen out.")]
        private string fadeOutName = "AnimationScreenFader@FadeOut";

        private void Reset()
        {
            animation = GetComponent<Animation>();
            animation.playAutomatically = false;
        }

        public override IEnumerator FadeIn()
        {
            animation.Play(fadeInName);
            yield return WaitWhilePlaying(animation);
        }

        public override IEnumerator FadeOut()
        {
            animation.Play(fadeOutName);
            yield return WaitWhilePlaying(animation);
        }

        private static IEnumerator WaitWhilePlaying(Animation animation)
        {
            while (animation.isPlaying)
                yield return null;
        }
    }
}
#endif