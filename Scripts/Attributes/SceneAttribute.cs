using UnityEngine;

namespace ActionCode.SceneManagement
{
    /// <summary>
    /// Adds an Object Field for Scene assets to a string or int fields.
    /// </summary>
    [System.AttributeUsage(System.AttributeTargets.Field, AllowMultiple = true)]
    public sealed class SceneAttribute : PropertyAttribute
    {
    }
}