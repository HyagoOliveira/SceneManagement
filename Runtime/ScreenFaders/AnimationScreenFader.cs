#if ANIMATION_MODULE
using UnityEngine;
using System.Threading.Tasks;

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

        public override async Task FadeIn()
        {
            animation.Play(fadeInName);
            await WaitWhilePlaying(animation);
        }

        public override async Task FadeOut()
        {
            animation.Play(fadeOutName);
            await WaitWhilePlaying(animation);
        }

        private static async Task WaitWhilePlaying(Animation animation)
        {
            while (animation.isPlaying)
                await Task.Yield();
        }
    }
}
#endif