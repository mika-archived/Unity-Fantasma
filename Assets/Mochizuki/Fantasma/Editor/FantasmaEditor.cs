using System.Diagnostics;

using Mochizuki.Fantasma.Attributes;

using UnityEditor;

using UnityEngine;

#pragma warning disable 649

namespace Mochizuki.Fantasma.Editor
{
    public class FantasmaEditor : EditorWindow
    {
        private const string Version = "0.1.0";
        private const string Product = "Mochizuki Fantasma";

        [Directory]
        [SerializeField]
        private DefaultAsset _baseDirectory;

        [ClassLibrary]
        [SerializeField]
        private DefaultAsset _classLibrary;

        [Directive]
        [SerializeField]
        private string _directive;

        [MenuItem("Mochizuki/Fantasma/Document")]
        public static void ShowDocument()
        {
            Process.Start("https://docs.mochizuki.moe/Unity/Fantasma/");
        }

        [MenuItem("Mochizuki/Fantasma/Editor")]
        public static void ShowWindow()
        {
            var window = GetWindow<FantasmaEditor>();
            window.titleContent = new GUIContent("Fantasma Editor");

            window.Show();
        }

        private void OnGUI()
        {
            EditorStyles.label.wordWrap = true;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField($"{Product} - {Version}");
            EditorGUILayout.Space();

            using (new EditorGUILayout.HorizontalScope(GUI.skin.box))
                EditorGUILayout.LabelField("Generate a new stub library based on the input class library.");

            PropertyField(this, nameof(_classLibrary));
            PropertyField(this, nameof(_baseDirectory));
            PropertyField(this, nameof(_directive));

            using (new EditorGUI.DisabledGroupScope(_classLibrary == null || _baseDirectory == null))
            {
                if (GUILayout.Button("Generate Classes by Roslyn and Reflection"))
                    OnSubmit();
            }
        }

        private void OnSubmit()
        {
        }

        private static void PropertyField(EditorWindow editor, string property)
        {
            var so = new SerializedObject(editor);
            so.Update();

            EditorGUILayout.PropertyField(so.FindProperty(property), true);

            so.ApplyModifiedProperties();
        }
    }
}