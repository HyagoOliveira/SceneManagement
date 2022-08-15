using UnityEditor;
using ActionCode.ScriptableSettingsProvider.Editor;

namespace ActionCode.SceneManagement.Editor
{
    public sealed class SceneManagerSettingsProvider :
        AbstractScriptableSettingsProvider<SceneManagerSettings>
    {
        public SceneManagerSettingsProvider() :
            base("ActionCode/Scene Manager")
        { }

        protected override string GetConfigName() => "com.actioncode.scene-management";

        [SettingsProvider]
        private static SettingsProvider CreateProjectSettingsMenu() =>
            new SceneManagerSettingsProvider();
    }
}