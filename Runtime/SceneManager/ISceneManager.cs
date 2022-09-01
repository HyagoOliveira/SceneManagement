using System;
using System.Threading.Tasks;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Interface used on objects able to be a Scene Manager.
    /// </summary>
    public interface ISceneManager
    {
        /// <summary>
        /// Action fired when the Scene Loading Progress changes.
        /// <para>
        /// The given float param is the progress in percentage, 
        /// i.e. a number between 0F and 100F.
        /// </para>
        /// </summary>
        event Action<float> OnProgressChanged;

        /// <summary>
        /// Whether a Scene Loading process is happening. 
        /// </summary>
        bool IsLoading { get; }

        Task LoadScene(string scene);
        Task LoadScene(string scene, SceneTransitionData data);
    }
}