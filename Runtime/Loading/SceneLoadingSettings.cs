using UnityEngine;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Data container for loading Scenes.
    /// </summary>
    [CreateAssetMenu(fileName = "SceneLoadingSettings", menuName = "ActionCode/Scene Management/Scene Loading Settings", order = 110)]
    public sealed class SceneLoadingSettings : ScriptableObject
    {
        [Min(0F), Tooltip("Time (in seconds) to wait before starts the loading process.")]
        public float timeBeforeLoading = 0F;
        [Min(0F), Tooltip("Time (in seconds) to wait after the loading process has finished.")]
        public float timeAfterLoading = 0F;
        [Scene, Tooltip("The Loading Scene.")]
        public string loadingScene;
        [Tooltip("Prefab for Screen Fader. It'll be instantiated at runtime.")]
        public GameObject screenFaderPrefab;

        /// <summary>
        /// Screen Fader runtime instance.
        /// </summary>
        public AbstractScreenFader ScreenFader { get; private set; }

        /// <summary>
        /// Gets <see cref="ScreenFader"/> or instantiates a new one based on the <see cref="screenFaderPrefab"/>. 
        /// <para>Note: <see cref="Object.DontDestroyOnLoad(Object)"/> will be applied on it.</para>
        /// </summary>
        /// <returns>An instance of <see cref="AbstractScreenFader"/> or null if <see cref="screenFaderPrefab"/> was not set.</returns>
        public AbstractScreenFader GetOrInstantiateScreenFader()
        {
            if (ScreenFader) return ScreenFader;
            if (!HasScreenFaderPrefab()) return null;
            var noScrenFaderInsidePrefab = screenFaderPrefab.GetComponent<AbstractScreenFader>() == null;
            if (noScrenFaderInsidePrefab)
            {
                var className = typeof(AbstractScreenFader).Name;
                var message = $"Prefab {screenFaderPrefab.name} has no component implementing {className} class.\n" +
                    $"Screen Fader will not work!";
                Debug.LogError(message);
                return null;
            }

            var instance = Instantiate(screenFaderPrefab);
            instance.name = screenFaderPrefab.name;
            ScreenFader = instance.GetComponent<AbstractScreenFader>();
            DontDestroyOnLoad(instance);

            return ScreenFader;
        }

        /// <summary>
        /// Checks if <see cref="ScreenFader"/> was set.
        /// </summary>
        /// <returns>Whether <see cref="ScreenFader"/> is not null.</returns>
        public bool HasScreenFader() => ScreenFader != null;

        /// <summary>
        /// Checks if <see cref="screenFaderPrefab"/> is set.
        /// </summary>
        /// <returns>True if <see cref="screenFaderPrefab"/> is set. False otherwise.</returns>
        public bool HasScreenFaderPrefab() => screenFaderPrefab != null;

        /// <summary>
        /// Checks if <see cref="loadingScene"/> was set.
        /// </summary>
        /// <returns>Whether <see cref="loadingScene"/> was set.</returns>
        public bool HasLoadingScene() => !string.IsNullOrEmpty(loadingScene);
    }
}