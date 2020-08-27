using System.IO;

using UnityEditor;

using UnityEngine;

namespace Mochizuki.Fantasma.Drawers
{
    [CustomPropertyDrawer(typeof(ClassLibraryDrawer))]
    public class ClassLibraryDrawer : PropertyDrawer
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
                    property.objectReferenceValue = null;
                    return;
                }

                if (Path.GetExtension(path) != ".dll")
                {
                    property.objectReferenceValue = null;
                    return;
                }
            }

            EditorGUI.PropertyField(position, property, label);
        }
    }
}