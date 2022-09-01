using System;
using UnityEngine;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Serializable Scene Transition Data. Use to customize how Scene Transitions should behave.
    /// </summary>
    [Serializable]
    public class SceneTransitionData : ISceneTransitionData
    {
        [SerializeField, Min(0F), Tooltip("Time (in seconds) to wait before starts the loading process.")]
        private float timeBeforeLoading = 0F;
        [SerializeField, Min(0F), Tooltip("Time (in seconds) to wait after the loading process has finished.")]
        private float timeAfterLoading = 0F;
        [SerializeField, Scene, Tooltip("The Loading Scene (the scene that will display the loading process).")]
        private string loadingScene;
        [SerializeField, Tooltip("The Prefab containing an instance of AbstractScreenFader. It'll be instantiated at runtime.")]
        private AbstractScreenFader screenFaderPrefab;

        public float TimeBeforeLoading => timeBeforeLoading;
        public float TimeAfterLoading => timeAfterLoading;
        public string LoadingScene => loadingScene;
        public IScreenFader ScreenFader => lazyFader.Value;

        private Lazy<IScreenFader> lazyFader;

        /// <summary>
        /// <inheritdoc cref="SceneTransitionData"/>
        /// </summary>
        /// <param name="timeBeforeLoading">Time (in seconds) to wait before starts the loading process.</param>
        /// <param name="timeAfterLoading">Time (in seconds) to wait after the loading process has finished.</param>
        /// <param name="loadingScene">The Loading Scene name or path (the scene that will display the loading process).</param>
        /// <param name="screenFaderPrefab">The Prefab containing an instance of AbstractScreenFader. It'll be instantiated at runtime.</param>
        public SceneTransitionData(
            float timeBeforeLoading,
            float timeAfterLoading,
            string loadingScene,
            AbstractScreenFader screenFaderPrefab = null
        )
        {
            this.timeBeforeLoading = timeBeforeLoading;
            this.timeAfterLoading = timeAfterLoading;
            this.loadingScene = loadingScene;
            lazyFader = new Lazy<IScreenFader>(
                ScreenFaderPool.Create(screenFaderPrefab)
            );
        }

        internal void InitializeLazyFader()
        {
            if (lazyFader != null) return;
            lazyFader = new Lazy<IScreenFader>(
                ScreenFaderPool.Create(screenFaderPrefab)
            );
        }
    }
}