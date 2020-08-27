using System.Text.RegularExpressions;

using Mochizuki.Fantasma.Attributes;

using UnityEditor;

using UnityEngine;

namespace Mochizuki.Fantasma.Drawers
{
    [CustomPropertyDrawer(typeof(DirectiveAttribute))]
    public class DirectiveDrawer : PropertyDrawer
    {
        private readonly Regex _directive = new Regex("^[A-Z_][A-Z0-9_]+$", RegexOptions.Compiled);
        private string _lastValidString = "";

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUI.GetPropertyHeight(property, label, true);
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            if (!string.IsNullOrWhiteSpace(property.stringValue))
            {
                if (!_directive.IsMatch(property.stringValue))
                {
                    property.stringValue = _lastValidString;
                    return;
                }

                _lastValidString = property.stringValue;
            }

            EditorGUI.PropertyField(position, property, label);
        }
    }
}