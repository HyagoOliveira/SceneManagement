#if UI_MODULE || TMP_MODULE // TextMeshPro was merged in package com.unity.ugui 2.0.0
#define TMP_AVAILABLE
#endif
#if UI_MODULE || LEGACY_UI_MODULE
#define TEXT_AVAILABLE
#endif
using UnityEngine;
#if TMP_AVAILABLE
using TMPro;
#endif
#if TEXT_AVAILABLE
using UnityEngine.UI;
#endif

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Updates a local Text component with the current loading progress.
    /// It will update a legacy Text or Text Mesh component if available.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class LoadingText : MonoBehaviour
    {
        [Tooltip("The format used on the text update.")]
        public string format = "{0} %";
#if TMP_AVAILABLE
        [SerializeField, Tooltip("The local Text Mesh component.")]
        private TMP_Text textMesh;
#endif
#if TEXT_AVAILABLE
        [SerializeField, Tooltip("The local Legacy Text component.")]
        private Text text;
#endif

        private void Reset()
        {
#if TMP_AVAILABLE
            textMesh = GetComponent<TMP_Text>();
#endif
#if TEXT_AVAILABLE
            text = GetComponent<Text>();
#endif
        }

        private void OnEnable() => SceneManager.OnProgressChanged += HandleProgressChanged;
        private void OnDisable() => SceneManager.OnProgressChanged -= HandleProgressChanged;

        private void HandleProgressChanged(float progress) => SetText(string.Format(format, progress));

        private void SetText(string value)
        {
#if TMP_AVAILABLE
            textMesh.text = value;
#endif
#if TEXT_AVAILABLE
            text.text = value;
#endif
        }
    }
}