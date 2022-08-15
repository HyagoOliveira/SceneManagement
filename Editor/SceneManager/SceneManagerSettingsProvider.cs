using UnityEngine;
using UnityEditor;
using ActionCode.ScriptableSettingsProvider.Editor;

namespace ActionCode.SceneManagement.Editor
{
    public sealed class SceneManagerSettingsProvider :
        AbstractScriptableSettingsProvider<SceneManagerSettings>
    {
        private const string CONFIG_NAME = "com.actioncode.scene-management";

        public SceneManagerSettingsProvider() :
            base("ActionCode/Scene Manager")
        { }

        protected override string GetConfigName() => CONFIG_NAME;

        [SettingsProvider]
        private static SettingsProvider CreateProjectSettingsMenu() =>
            new SceneManagerSettingsProvider();

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void AwakeSettings()
        {
            var hasSettings = EditorBuildSettings.TryGetConfigObject(CONFIG_NAME, out SceneManagerSettings settings);
            if (hasSettings) settings.Awake();
        }
    }
}