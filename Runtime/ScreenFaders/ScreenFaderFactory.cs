using UnityEngine;
using System.Collections.Generic;

namespace ActionCode.SceneManagement
{
    public static class ScreenFaderFactory
    {
        private static Dictionary<int, GameObject> instances = new Dictionary<int, GameObject>();

        public static IScreenFader Create(AbstractScreenFader faderPrefab)
        {
            if (faderPrefab == null) return null;

            var prefabId = faderPrefab.gameObject.GetInstanceID();
            var hasInstance = instances.TryGetValue(prefabId, out GameObject prefab);
            if (hasInstance) return prefab.GetComponent<AbstractScreenFader>();

            prefab = faderPrefab.gameObject;
            var instance = Object.Instantiate(prefab);

            instance.name = prefab.name;

            instances.Add(prefabId, instance);
            Object.DontDestroyOnLoad(instance);

            return instance.GetComponent<AbstractScreenFader>();
        }
    }
}