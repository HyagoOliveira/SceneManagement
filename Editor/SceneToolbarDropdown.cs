using UnityEngine;
using UnityEditor;
using UnityEditor.Toolbars;
using UnityEditor.SceneManagement;

namespace ActionCode.SceneManagement.Editor
{
    /// <summary>
    /// Creates a Scene Selector Dropdown button in the Toolbar.
    /// </summary>
    /// <remarks>Only Scenes presents in the build will be available.</remarks>
    public static class SceneToolbarDropdown
    {
        [MainToolbarElement("Action Code/Scene Selector", defaultDockPosition = MainToolbarDockPosition.Left)]
        public static MainToolbarElement CreateSceneSelectorDropdown()
        {
            var icon = EditorGUIUtility.IconContent("SceneAsset Icon").image as Texture2D;
            var content = new MainToolbarContent("Scenes", icon, tooltip: "Open a Scene present in the Build");
            return new MainToolbarDropdown(content, ShowSceneDropdown);
        }

        private static void ShowSceneDropdown(Rect rect)
        {
            var menu = new GenericMenu();
            var guids = AssetDatabase.FindAssets("t:SceneAsset");
            var currentSceneName = EditorSceneManager.GetActiveScene().name;

            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (!IsBuildScene(path)) continue;

                var scene = AssetDatabase.LoadAssetAtPath<SceneAsset>(path);
                var item = new GUIContent(scene.name);
                var isCurrentScene = currentSceneName == scene.name;

                menu.AddItem(item, on: isCurrentScene, () => OpenScene(path));
            }

            menu.ShowAsContext();
        }

        private static void OpenScene(string path)
        {
            if (Application.isPlaying)
            {
                Debug.LogWarning("Cannot open Scene while in Play Mode.");
                return;
            }

            var openScene = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
            if (openScene) EditorSceneManager.OpenScene(path);
        }

        private static bool IsBuildScene(string path)
        {
            var index = UnityEngine.SceneManagement.SceneUtility.GetBuildIndexByScenePath(path);
            return index >= 0;
        }
    }
}