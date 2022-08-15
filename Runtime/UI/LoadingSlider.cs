#if UI_MODULE
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
        [SerializeField, Tooltip("The Scene Manager settings.")]
        private SceneManagerSettings settings;

        private void Reset()
        {
            slider = GetComponent<Slider>();
            slider.interactable = false;
            slider.transition = Selectable.Transition.None;
            slider.minValue = 0F;
            slider.maxValue = 100F;
            slider.value = 50F;
        }

        private void OnEnable() => settings.OnProgressChanged += HandleProgressChanged;
        private void OnDisable() => settings.OnProgressChanged -= HandleProgressChanged;

        private void HandleProgressChanged(float progress) => slider.value = progress;
    }
}
#endif