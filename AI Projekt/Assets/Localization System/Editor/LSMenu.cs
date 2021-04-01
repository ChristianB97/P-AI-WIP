using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;

namespace LS.Editor
{
    public class LSMenu
    {
        [MenuItem("Window/Localization System/XML Editor", priority = 0)]
        public static void GenerateMenu()
        {
            var editor = EditorWindow.GetWindow<XMLEditorWindow>(true);
            editor.Show();
        }

        [MenuItem("Window/Localization System/Support", priority = 1)]
        public static void Support()
        {
            System.Diagnostics.Process.Start(Constants.SupportUrl);
        }
    }
}
