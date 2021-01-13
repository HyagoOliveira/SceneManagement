using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace ActionCode.SceneManagement.Editor
{
    /// <summary>
    /// Custom property drawer for <see cref="SceneAttribute"/>.
    /// <para>Adds an Object Field for Scene assets to a string or int fields.</para>
    /// </summary>
    [CustomPropertyDrawer(typeof(SceneAttribute))]
    public sealed class SceneAttributeDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var height = base.GetPropertyHeight(property, label);
            var invalidPropertyType =
                property.propertyType != SerializedPropertyType.String &&
                property.propertyType != SerializedPropertyType.Integer;

            return invalidPropertyType ? 2F * height : height;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.propertyType == SerializedPropertyType.String)
            {
                var scenePath = property.stringValue;
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

                scenePath = DisplaySceneField(sceneAsset, position, label);
                property.stringValue = scenePath;
            }
            else if (property.propertyType == SerializedPropertyType.Integer)
            {
                var sceneIndex = property.intValue;
                var scenePath = SceneUtility.GetScenePathByBuildIndex(sceneIndex);
                var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

                scenePath = DisplaySceneField(sceneAsset, position, label);
                property.intValue = SceneUtility.GetBuildIndexByScenePath(scenePath);
            }
            else
            {
                const string msg = "Scene attribute is only valid for string or int fields.";
                position = EditorGUI.PrefixLabel(position, label);
                EditorGUI.HelpBox(position, msg, MessageType.Error);
            }
        }

        private static string DisplaySceneField(SceneAsset scene, Rect position, GUIContent label)
        {
            scene = EditorGUI.ObjectField(position, label, scene, typeof(SceneAsset), allowSceneObjects: false) as SceneAsset;
            return AssetDatabase.GetAssetPath(scene);
        }
    }
}