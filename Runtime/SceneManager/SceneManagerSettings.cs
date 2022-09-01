using System;
using System.Threading.Tasks;
using UnityEngine;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// The Scene Manager Settings.
    /// </summary>
    public sealed class SceneManagerSettings : ScriptableObject
    {
        [Tooltip("De default Scene Transition values used when none is provided.")]
        public SceneTransitionData defaultTransition;

        public ISceneTransition Transition => lazyTransition.Value;

        private readonly Lazy<ISceneTransition> lazyTransition =
            new Lazy<ISceneTransition>(CreateTransition);

        public async Task LoadScene(string scene)
        {
            defaultTransition.InitializeLazyFader();
            await Transition.LoadScene(scene, defaultTransition);
        }

        private static ISceneTransition CreateTransition() => new SceneTransition();
    }
}