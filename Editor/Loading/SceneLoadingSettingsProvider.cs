using UnityEditor;
using ActionCode.ScriptableSettingsProvider.Editor;

namespace ActionCode.SceneManagement.Editor
{
    public sealed class SceneLoadingSettingsProvider :
        AbstractScriptableSettingsProvider<SceneLoadingSettings>
    {
        public SceneLoadingSettingsProvider() :
            base("ActionCode/Scene Loading")
        { }

        protected override string GetConfigName() =>
            "com.actioncode.scene-management";

        [SettingsProvider]
        private static SettingsProvider CreateProjectSettingsMenu() =>
            new SceneLoadingSettingsProvider();
    }
}