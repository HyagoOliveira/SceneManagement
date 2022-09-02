using UnityEngine;
#if UI_MODULE
using UnityEngine.UI;
#endif

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Updates a local <see cref="Text"/> with the current loading progress.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public sealed class LoadingText : MonoBehaviour
    {
#if UI_MODULE
        [SerializeField, Tooltip("The local Text component.")]
        private Text text;
#endif
        [SerializeField, Tooltip("The Scene Manager.")]
        private SceneManager sceneManager;

        public const string TEXT_FORMAT = "{0} %";

        private void Reset() => text = GetComponent<Text>();
        private void OnEnable() => sceneManager.OnProgressChanged += HandleProgressChanged;
        private void OnDisable() => sceneManager.OnProgressChanged -= HandleProgressChanged;

        private void HandleProgressChanged(float progress) => SetText(string.Format(TEXT_FORMAT, progress));

        private void SetText(string text)
        {
#if UI_MODULE
            this.text.text = string.Format(TEXT_FORMAT, text);
#endif
        }
    }
}