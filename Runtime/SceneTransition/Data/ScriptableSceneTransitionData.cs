using System;
using UnityEngine;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Scriptable Scene Transition Data used to customize how Scene Transitions should behave.
    /// </summary>
    [CreateAssetMenu(fileName = "NewSceneTransition", menuName = "ActionCode/SceneManager/Scene Transition", order = 110)]
    public sealed class ScriptableSceneTransitionData : ScriptableObject
    {
        [SerializeField, Min(0F), Tooltip("Time (in seconds) to wait before starts the loading process.")]
        private float timeBeforeLoading = 0F;
        [SerializeField, Min(0F), Tooltip("Time (in seconds) to wait after the loading process has finished.")]
        private float timeAfterLoading = 0F;
        [SerializeField, Scene, Tooltip("The Loading Scene name (the scene that will display the loading process).")]
        private string loadingScene;
        [SerializeField, Tooltip("The Prefab containing an instance of AbstractScreenFader. It'll be instantiated at runtime.")]
        private AbstractScreenFader screenFaderPrefab;

        public float TimeBeforeLoading => timeBeforeLoading;
        public float TimeAfterLoading => timeAfterLoading;
        public string LoadingScene => loadingScene;
        public IScreenFader ScreenFader => lazyFader.Value;

        private Lazy<IScreenFader> lazyFader = new Lazy<IScreenFader>();

        internal void Initialize()
        {
            if (lazyFader.IsValueCreated) return;
            lazyFader = new Lazy<IScreenFader>(FindOrCreateScreenFader);
        }

        private IScreenFader FindOrCreateScreenFader() => ScreenFaderPool.Create(screenFaderPrefab);
    }
}