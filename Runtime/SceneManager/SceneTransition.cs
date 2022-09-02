using System;
using UnityEngine;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Data container used to customize how Scene Transitions should behave.
    /// </summary>
    [CreateAssetMenu(fileName = "NewSceneTransition", menuName = "ActionCode/SceneManager/Scene Transition", order = 110)]
    public sealed class SceneTransition : ScriptableObject
    {
        [SerializeField, Min(0F), Tooltip("Time (in seconds) to wait before starts the loading process.")]
        private float timeBeforeLoading = 0F;
        [SerializeField, Min(0F), Tooltip("Time (in seconds) to wait after the loading process has finished.")]
        private float timeAfterLoading = 0F;
        [SerializeField, Scene, Tooltip("The Loading Scene name (the scene that will display the loading process).")]
        private string loadingScene;
        [SerializeField, Tooltip("The Prefab containing an instance of AbstractScreenFader. It'll be instantiated at runtime.")]
        private AbstractScreenFader screenFaderPrefab;

        /// <summary>
        /// Time (in seconds) to wait before starts the loading process.
        /// </summary>
        public float TimeBeforeLoading => timeBeforeLoading;

        /// <summary>
        /// Time (in seconds) to wait after the loading process has finished.
        /// </summary>
        public float TimeAfterLoading => timeAfterLoading;

        /// <summary>
        /// The Loading Scene name (the scene that will display the loading process).
        /// </summary>
        public string LoadingScene => loadingScene;

        /// <summary>
        /// The ScreenFader instance ready to be used to fade the screen in/out.
        /// </summary>
        public IScreenFader ScreenFader => lazyFader.Value;

        private Lazy<IScreenFader> lazyFader = new Lazy<IScreenFader>();

        internal void Initialize()
        {
            if (lazyFader.IsValueCreated) return;
            lazyFader = new Lazy<IScreenFader>(FindOrCreateScreenFader);
        }

        /// <summary>
        /// Whether <see cref="LoadingScene"/> is valid Scene.
        /// </summary>
        /// <returns>True if <see cref="LoadingScene"/> is valid Scene. False otherwise.</returns>
        public bool HasValidLoadingScene() => IsValidScene(LoadingScene);

        private IScreenFader FindOrCreateScreenFader() => ScreenFaderPool.Create(screenFaderPrefab);

        private static bool IsValidScene(string path) => !string.IsNullOrEmpty(path) &&
            UnityEngine.SceneManagement.SceneManager.GetSceneByPath(path).IsValid();
    }
}