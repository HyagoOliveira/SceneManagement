using UnityEngine;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Abstract component able to fade the screen in and out.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public abstract class AbstractScreenFader : MonoBehaviour, IScreenFader
    {
        public abstract Awaitable FadeInAsync();
        public abstract Awaitable FadeOutAsync();
    }
}