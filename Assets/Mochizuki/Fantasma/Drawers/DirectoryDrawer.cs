using UnityEditor;

using UnityEngine;

namespace Mochizuki.Fantasma.Drawers
{
    [CustomPropertyDrawer(typeof(DirectoryDrawer))]
    public class DirectoryDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (property.objectReferenceValue != null)
            {
                var path = AssetDatabase.GetAssetPath(property.objectReferenceValue);
                if (AssetDatabase.IsValidFolder(path))
                {
                    EditorGUI.PropertyField(position, property, label);
                    return;
                }

                property.objectReferenceValue = null;
            }
        }
    }
}