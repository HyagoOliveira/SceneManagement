using UnityEngine;
using ActionCode.AnimationParameters;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Fader component for a Screen transition.
    /// <para>Fades the screen in and out using the local <see cref="Animator"/> component.</para>
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public sealed class ScreenFaderAnimator : AbstractScreenFader
    {
        [SerializeField, Tooltip("The local animator component.")]
        private Animator animator;

        [Header("Parameters")]
        [Tooltip("Animator parameter triggered to make the screen fades in.")]
        public TriggerAnimatorParam fadeInParam = new TriggerAnimatorParam("fadeIn");
        [Tooltip("Animator parameter triggered to make the screen fades out.")]
        public TriggerAnimatorParam fadeOutParam = new TriggerAnimatorParam("fadeOut");

        [Header("Animations")]
        public string fadeInAnimation = "";
        public string fadeOutAnimation = "";
        [Min(0)]
        public int fadeLayerIndex = 0;

        protected override void Reset()
        {
            base.Reset();
            animator = GetComponent<Animator>();
        }

        /// <summary>
        /// Fades in the screen.
        /// </summary>
        public override void FadeIn() => animator.SetParam(fadeInParam, TriggerParamMode.Set);

        /// <summary>
        /// Fades out the screen.
        /// </summary>
        public override void FadeOut() => animator.SetParam(fadeOutParam, TriggerParamMode.Set);

        /// <summary>
        /// Checks if the screen has completely fades in.
        /// </summary>
        /// <returns>True if the screen has fades in. False otherwise.</returns>
        public override bool HasFadedIn() => HasCompletedAnimation(fadeInAnimation);

        /// <summary>
        /// Checks if the screen has completely fades out.
        /// </summary>
        /// <returns>True if the screen has fades out. False otherwise.</returns>
        public override bool HasFadedOut() => HasCompletedAnimation(fadeOutAnimation);

        /// <summary>
        /// Returns the current AnimatorStateInfo using the <see cref="fadeLayerIndex"/>.
        /// </summary>
        /// <returns>The current <see cref="AnimatorStateInfo"/> instance.</returns>
        public AnimatorStateInfo GetCurrentStateInfo()
            => animator.GetCurrentAnimatorStateInfo(fadeLayerIndex);

        private bool HasCompletedAnimation(string animationName)
        {
            var info = GetCurrentStateInfo();
            var inAnimation = info.IsName(animationName);
            var hasPlayedEntireAnimation = info.normalizedTime > .99F;
            return inAnimation && hasPlayedEntireAnimation;
        }
    }
}