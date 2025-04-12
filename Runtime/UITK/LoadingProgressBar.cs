using UnityEngine;
using UnityEngine.UIElements;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Updates a local UI Toolkit <see cref="ProgressBar"/> with the current Scene loading progress. 
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UIDocument))]
    public sealed class LoadingProgressBar : MonoBehaviour
    {
        [SerializeField, Tooltip("The local UI Document component.")]
        private UIDocument document;
        [Tooltip("The name of the Progress Bar element.")]
        public string elementName = "LoadingProgress";

        /// <summary>
        /// The document Root Visual Element.
        /// </summary>
        public VisualElement Root { get; private set; }

        private ProgressBar progressBar;

        private void Reset() => document = GetComponent<UIDocument>();
        private void Awake() => FindReferences();
        private void OnEnable() => SceneManager.OnProgressChanged += HandleProgressChanged;
        private void OnDisable() => SceneManager.OnProgressChanged -= HandleProgressChanged;

        private void FindReferences()
        {
            Root = document.rootVisualElement;
            progressBar = Root.Q<ProgressBar>(elementName);
        }

        private void HandleProgressChanged(float progress) => progressBar.value = progress;
    }
}
