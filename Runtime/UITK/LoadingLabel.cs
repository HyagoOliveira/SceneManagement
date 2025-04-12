using UnityEngine;
using UnityEngine.UIElements;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Updates a local UI Toolkit <see cref="Label"/> with the current Scene loading progress. 
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(UIDocument))]
    public sealed class LoadingLabel : MonoBehaviour
    {
        [SerializeField, Tooltip("The local UI Document component.")]
        private UIDocument document;
        [Tooltip("The name of the Progress Bar element.")]
        public string elementName = "Loading";
        [Tooltip("The format applied on the label text.")]
        public string format = "{0} %";

        /// <summary>
        /// The document Root Visual Element.
        /// </summary>
        public VisualElement Root { get; private set; }

        private Label label;

        private void Reset() => document = GetComponent<UIDocument>();
        private void Awake() => FindReferences();
        private void OnEnable() => SceneManager.OnProgressChanged += HandleProgressChanged;
        private void OnDisable() => SceneManager.OnProgressChanged -= HandleProgressChanged;

        private void FindReferences()
        {
            Root = document.rootVisualElement;
            label = Root.Q<Label>(elementName);
        }

        private void HandleProgressChanged(float progress) => label.text = string.Format(format, progress);
    }
}
