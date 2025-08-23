using System.Threading.Tasks;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Interface used on objects able to load something asynchronously when a Scene is finishing to load.
    /// </summary>
    public interface ISceneLoadable
    {
        /// <summary>
        /// The sort order used to determine the load sequence.
        /// </summary>
        uint SortOrder { get; }

        /// <summary>
        /// Whether this object has finished loading.
        /// </summary>
        bool IsLoaded { get; }

        /// <summary>
        /// Loads the object asynchronously.
        /// </summary>
        /// <returns>An asynchronously operation.</returns>
        Task LoadAsync();
    }
}