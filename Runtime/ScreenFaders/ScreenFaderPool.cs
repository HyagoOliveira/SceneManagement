using UnityEngine;
using System.Collections.Generic;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Pool factory for <see cref="IScreenFader"/>.
    /// </summary>
    public static class ScreenFaderPool
    {
        private readonly static Dictionary<int, GameObject> instances = new();

        /// <summary>
        /// Creates or returns an instance of <see cref="IScreenFader"/>.
        /// </summary>
        /// <param name="faderPrefab">The prefab containing a component of <see cref="AbstractScreenFader"/>.</param>
        /// <returns>Always a global instance of <see cref="IScreenFader"/>.</returns>
        public static AbstractScreenFader Create(AbstractScreenFader faderPrefab)
        {
            if (faderPrefab == null) return null;

            var prefabId = faderPrefab.gameObject.GetInstanceID();
            var hasInstance = instances.TryGetValue(prefabId, out GameObject prefab) && prefab != null;
            if (hasInstance) return prefab.GetComponent<AbstractScreenFader>();

            prefab = faderPrefab.gameObject;
            var instance = Object.Instantiate(prefab);

            instance.name = prefab.name;

            instances.Remove(prefabId);
            instances.TryAdd(prefabId, instance);

            Object.DontDestroyOnLoad(instance);

            return instance.GetComponent<AbstractScreenFader>();
        }
    }
}