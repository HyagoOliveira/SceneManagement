using TMPro;
using UnityEngine;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Updates the local <see cref="TMP_Text"/> with the current loading progress.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public sealed class LoadingText : MonoBehaviour
    {
        [SerializeField, Tooltip("The local Text component.")]
        private TMP_Text textMesh;
        [SerializeField, Tooltip("The Scene Manager.")]
        private SceneManager sceneManager;
        [Tooltip("The format used on the text update.")]
        public string format = "{0} %";

        private void Reset() => textMesh = GetComponent<TMP_Text>();
        private void OnEnable() => sceneManager.OnProgressChanged += HandleProgressChanged;
        private void OnDisable() => sceneManager.OnProgressChanged -= HandleProgressChanged;

        private void HandleProgressChanged(float progress) => textMesh.text = string.Format(format, progress);
    }
}