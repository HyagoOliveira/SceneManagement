using UnityEngine;
using UnityEngine.UI;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Updates a local <see cref="Text"/> with the 
    /// current <see cref="SceneManager.LoadingProgressPercentage"/>.
    /// </summary>
    [RequireComponent(typeof(Text))]
    public sealed class LoadingText : MonoBehaviour
    {
        [SerializeField, Tooltip("The local Text component.")]
        private Text text;

        public const string TEXT_FORMAT = "{0} %";

        private void Reset()
        {
            text = GetComponent<Text>();
        }

        private void Update()
        {
            //text.text = string.Format(TEXT_FORMAT, SceneManager.LoadingProgressPercentage);
        }
    }
}