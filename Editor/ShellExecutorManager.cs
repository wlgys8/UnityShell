
namespace MS.Shell.Editor{
    
    using UnityEngine;
    using UnityEditor;
    using System.Linq;
    using System.Collections.Generic;

    public class ShellExecutorManager : EditorWindow {
    
        [MenuItem("Window/ShellExecutorManager")]
        private static void ShowWindow() {
            var window = GetWindow<ShellExecutorManager>();
            window.titleContent = new GUIContent("ShellExecutorManager");
            window.Show();
        }

        private List<ShellExecutor> _executors = new List<ShellExecutor>();
   
        private void OnFocus() {
            _executors = AssetDatabase.FindAssets("t:ShellExecutor").Select((guid)=>{
                var path = AssetDatabase.GUIDToAssetPath(guid);
                return AssetDatabase.LoadAssetAtPath<ShellExecutor>(path);
            }).ToList();
        }
        private void OnGUI() {

            EditorGUILayout.LabelField("Executors:");
            foreach(var executor in _executors){
                GUILayout.BeginHorizontal(EditorStyles.helpBox);
                GUIHelper.Status(executor.status);
                EditorGUILayout.LabelField(executor.displayName);
                if(GUILayout.Button("Edit")){
                    ShellExecutorEditWindow.Open(executor);
                }

                EditorGUI.BeginDisabledGroup(executor.status == ShellExecutor.Status.Running);
                if(GUILayout.Button("Execute")){
                    executor.Execute().onExit +=(exitCode)=>{
                        this.Repaint();
                    };
                }
                EditorGUI.EndDisabledGroup();
                GUILayout.EndHorizontal();
            }
        }
    }

    public class ShellExecutorEditWindow:EditorWindow{

        private static ShellExecutor _executor;
        private static ShellExecutorEditWindow _instance;
        public static void Open(ShellExecutor executor){
            if(_instance != null){
                _instance.Close();
            }
            _executor = executor;
            var win = ShellExecutorEditWindow.CreateInstance<ShellExecutorEditWindow>();
            win.titleContent = new GUIContent("Editor");
            win.ShowUtility();
            _instance = win;
        }


        private Editor _inspestor;
        private void OnEnable() {
            _inspestor = Editor.CreateEditor(_executor,typeof(ShellExecutorInspector));    
        }


        private void OnDisable() {
            _inspestor = null;
            _executor = null;    
        }

        void OnGUI(){
            if(_inspestor == null){
                return;
            }
            _inspestor.OnInspectorGUI();
        }
    }
}
