using UnityEditor;
using UnityEngine;

namespace ActionCode.SceneManagement.Editor
{
    /// <summary>
    /// Custom property drawer for <see cref="Scene"/>.
    /// <para>Adds an Object Field for Scene assets.</para>
    /// </summary>
    [CustomPropertyDrawer(typeof(Scene))]
    public sealed class SceneDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var pathProperty = property.FindPropertyRelative(nameof(Scene.path));
            var scenePath = pathProperty.stringValue;
            var sceneAsset = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath);

            scenePath = DisplaySceneField(sceneAsset, position, label);
            pathProperty.stringValue = scenePath;
        }

        private static string DisplaySceneField(SceneAsset scene, Rect position, GUIContent label)
        {
            scene = EditorGUI.ObjectField(position, label, scene, typeof(SceneAsset), allowSceneObjects: false) as SceneAsset;
            return AssetDatabase.GetAssetPath(scene);
        }
    }
}