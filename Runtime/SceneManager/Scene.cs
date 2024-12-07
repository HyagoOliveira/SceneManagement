using System;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Data for Scene.
    /// </summary>
    [Serializable]
    public sealed class Scene
    {
        public string path;

        public Scene(string path)
        {
            this.path = path;
        }

        /// <summary>
        /// Returns the Scene name without any path or extension.
        /// <para>
        /// If the Scene path is <c>Assets/Scenes/Level/StartLevel.unity</c>, 
        /// this function will return <c>StartLevel</c>.
        /// </para>
        /// </summary>
        /// <returns>Always a string (can be empty if invalid Scene).</returns>
        public string GetName() => GetNameFrom(path);

        public override string ToString() => path;

        private static string GetNameFrom(string path)
        {
            var splited = path.Split(
                new string[] { "/", ".unity" },
                StringSplitOptions.RemoveEmptyEntries
            );
            return splited.Length > 0 ? splited[^1] : string.Empty;
        }
    }
}