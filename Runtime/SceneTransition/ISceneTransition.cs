using System;
using System.Threading.Tasks;

namespace ActionCode.SceneManagement
{
    public interface ISceneTransition
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

        Task LoadScene(string scene, ScriptableSceneTransitionData data);
    }
}