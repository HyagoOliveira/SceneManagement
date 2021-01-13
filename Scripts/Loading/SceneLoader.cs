using UnityEngine;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Component used only for starts coroutines inside <see cref="SceneManager"/>.
    /// </summary>
    internal sealed class SceneLoader : MonoBehaviour
    {
        private static SceneLoader instance;

        internal static SceneLoader FindOrInstanciate()
        {
            if (instance == null)
            {
                instance = FindObjectOfType<SceneLoader>();
                if (instance == null)
                {
                    var name = typeof(SceneLoader).Name;
                    var gameObject = new GameObject(name);
                    instance = gameObject.AddComponent<SceneLoader>();
                    DontDestroyOnLoad(gameObject);
                }
            }
            return instance;
        }
    }
}