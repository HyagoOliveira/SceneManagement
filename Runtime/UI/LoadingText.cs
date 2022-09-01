#if UI_MODULE
using UnityEngine;
using UnityEngine.UI;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Updates a local <see cref="Text"/> with the current loading progress.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public sealed class LoadingText : MonoBehaviour
    {
        [SerializeField, Tooltip("The local Text component.")]
        private Text text;
        [SerializeField, Tooltip("The Scene Manager settings.")]
        private SceneManagerSettings settings;

        public const string TEXT_FORMAT = "{0} %";

        private void Reset() => text = GetComponent<Text>();
        private void OnEnable() => settings.Transition.OnProgressChanged += HandleProgressChanged;
        private void OnDisable() => settings.Transition.OnProgressChanged -= HandleProgressChanged;

        private void HandleProgressChanged(float progress) =>
            text.text = string.Format(TEXT_FORMAT, progress);
    }
}
#endif