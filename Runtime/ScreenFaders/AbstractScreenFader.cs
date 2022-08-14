using UnityEngine;
using System.Threading.Tasks;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Abstract component used on objects able to fades the screen in and out.
    /// </summary>
    [RequireComponent(typeof(Canvas))]
    public abstract class AbstractScreenFader : MonoBehaviour, IScreenFader
    {
        public abstract Task FadeIn();
        public abstract Task FadeOut();
    }
}