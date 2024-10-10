using UnityEngine;
using UnityEngine.UI;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Updates a local <see cref="Slider"/> with the current loading progress. 
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
            slider.minValue = 0F;
            slider.maxValue = 100F;
            slider.value = 50F;
        }

        private void OnEnable() => SceneManager.OnProgressChanged += HandleProgressChanged;
        private void OnDisable() => SceneManager.OnProgressChanged -= HandleProgressChanged;

        private void HandleProgressChanged(float progress) => slider.value = progress;
    }
}