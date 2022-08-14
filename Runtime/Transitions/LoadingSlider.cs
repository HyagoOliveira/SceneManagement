using UnityEngine;
using UnityEngine.UI;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Updates a local <see cref="Slider"/> with the 
    /// current <see cref="SceneManager.LoadingProgress"/>.
    /// </summary>
    [RequireComponent(typeof(Slider))]
    public sealed class LoadingSlider : MonoBehaviour
    {
        [SerializeField, Tooltip("The local Slider component.")]
        private Slider slider;

        private void Reset()
        {
            slider = GetComponent<Slider>();
            slider.interactable = false;
            slider.transition = Selectable.Transition.None;
        }

        private void Update()
        {
            slider.value = SceneManager.LoadingProgress;
        }
    }
}