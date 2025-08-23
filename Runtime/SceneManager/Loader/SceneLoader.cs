using UnityEngine;
using System.Linq;
using System.Threading.Tasks;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Loader for <see cref="ISceneLoadable"/> implementations in the current Scene.
    /// </summary>
    [DisallowMultipleComponent]
    [DefaultExecutionOrder(EXECUTION_ORDER)]
    public sealed class SceneLoader : MonoBehaviour
    {
        public bool IsLoaded { get; private set; }

        public const int EXECUTION_ORDER = -10;

        private void Awake() => Load();

        public async void Load()
        {
            IsLoaded = false;
            await LoadAsync(GetCurrentSceneRootGameObjects());
            IsLoaded = true;
        }

        private async Awaitable LoadAsync(GameObject[] roots)
        {
            var loadables = GetLoadables(roots);
            var lastSortOrder = loadables.Max(l => l.SortOrder);

            for (var i = 0; i <= lastSortOrder; i++)
            {
                var loadGroup = loadables.Where(l => l.SortOrder == i).ToArray();
                var loadTasks = loadGroup.Select(l => l.LoadAsync()).ToArray();

                await Task.WhenAll(loadTasks);
            }
        }

        private static ISceneLoadable[] GetLoadables(GameObject[] roots) => roots.
            SelectMany(root => root.GetComponentsInChildren<ISceneLoadable>()).
            ToArray();

        private static GameObject[] GetCurrentSceneRootGameObjects() =>
            UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();
    }
}