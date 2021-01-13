using UnityEditor;

namespace ActionCode.SceneManagement.Editor
{
    /// <summary>
    /// Custom Editor class for <see cref="SceneLoadingSettings"/>.
    /// <para>It will display help messages for <see cref="SceneLoadingSettings.screenFaderPrefab"/>.</para>
    /// </summary>
    [CustomEditor(typeof(SceneLoadingSettings))]
    public sealed class SceneLoadingSettingsEditor : UnityEditor.Editor
    {
        private SceneLoadingSettings settings;

        private void OnEnable()
        {
            settings = target as SceneLoadingSettings;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            EditorGUILayout.Space();
            CheckScreenFaderPrefab();
        }

        private void CheckScreenFaderPrefab()
        {
            var type = MessageType.Info;
            var className = typeof(AbstractScreenFader).Name;
            var message = $"Screen Fader Prefab needs to have a component implementing the '{className}' class.";

            if (settings.HasScreenFaderPrefab())
            {
                var prefab = settings.screenFaderPrefab;
                var hasComponent = prefab.GetComponent<AbstractScreenFader>() != null;
                if (hasComponent)
                {
                    message = $"'{prefab.name}' is implementing the '{className}' class. Everything is set!";
                    type = MessageType.Info;
                }
                else
                {
                    message = $"'{prefab.name}' must have a component implementing the '{className}' class.";
                    type = MessageType.Error;
                }
            }

            EditorGUILayout.HelpBox(message, type, wide: true);
        }
    }
}