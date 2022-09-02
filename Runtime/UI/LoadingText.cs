using UnityEngine;
#if UI_MODULE
using UnityEngine.UI;
#endif
#if TEXT_MESH_PRO_MODULE
using TMPro;
#endif

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Updates a local <see cref="Text"/> with the current loading progress.
    /// </summary>
    public sealed class LoadingText : MonoBehaviour
    {
#if UI_MODULE
        [SerializeField, Tooltip("The local Text component.")]
        private Text textUI;
#endif
#if UI_MODULE
        [SerializeField, Tooltip("The local Text component.")]
        private TMP_Text textMesh;
#endif
        [SerializeField, Tooltip("The Scene Manager.")]
        private SceneManager sceneManager;

        public const string TEXT_FORMAT = "{0} %";

        private void Reset()
        {
#if UI_MODULE
            textUI = GetComponent<Text>();
#endif
#if UI_MODULE
            textMesh = GetComponent<TMP_Text>();
#endif
        }

        private void OnEnable() => sceneManager.OnProgressChanged += HandleProgressChanged;
        private void OnDisable() => sceneManager.OnProgressChanged -= HandleProgressChanged;

        private void HandleProgressChanged(float progress) => SetText(string.Format(TEXT_FORMAT, progress));

        private void SetText(string text)
        {
#if UI_MODULE
            if (textUI) textUI.text = text;
#endif
#if UI_MODULE
            if (textMesh) textMesh.text = text;
#endif
        }
    }
}