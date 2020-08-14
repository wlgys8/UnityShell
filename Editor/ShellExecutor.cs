using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MS.Shell.Editor;
using UnityEditor;

namespace MS.Shell.Editor{
    [CreateAssetMenu(menuName = "ShellExecutor")]
    public class ShellExecutor : ScriptableObject
    {
        
        [SerializeField]
        private string _displayName;

        [SerializeField]
        private string _command;

        [SerializeField]
        private string _workingDir;

        [SerializeField]
        private string _encoding = "";

        [SerializeField]
        private List<KeyValuePair> _customEnvironmentVars;

        private Status _status = Status.Idle;

        public void ClearStatus(){
            _status = Status.Idle;
        }

        public Status status{
            get{
                return _status;
            }
        }

        public EditorShell.Operation Execute(){
            if(_status == Status.Running){
                Debug.LogError("ShellExecutor is running. can not duplicate execute");
                return null;
            }
            _status = Status.Running;
            EditorShell.Options option = new EditorShell.Options();
            if(!string.IsNullOrEmpty(_workingDir)){
                option.workDirectory = _workingDir;
            }
            if(_customEnvironmentVars != null){
                foreach(var pair in _customEnvironmentVars){
                    option.environmentVars.Add(pair.key,pair.value);
                }
            }
            if(!string.IsNullOrEmpty(_encoding)){
                option.encoding = System.Text.Encoding.GetEncoding(_encoding);
            }
            var task = EditorShell.Execute(_command,option);
            task.onLog += (EditorShell.LogType logType,string log)=>{
                if(logType == EditorShell.LogType.Error){
                    Debug.LogError(log);
                }else{
                    Debug.Log(log);
                }
            };

            task.onExit += (exitCode)=>{
                Debug.Log("Execute done. Exit code = " + exitCode);
                _status = exitCode == 0?Status.Success:Status.Failed;
            };
            return task;
        }

        public string displayName{
            get{
                if(string.IsNullOrEmpty(_displayName)){
                    return this.name;
                }
                return _displayName;
            }
        }

        [System.Serializable]
        public class KeyValuePair{
            public string key;
            public string value;
        }

        public enum Status{
            Idle,
            Running,
            Success,
            Failed,
        }

    }

    [CustomEditor(typeof(ShellExecutor))]
    public class ShellExecutorInspector:UnityEditor.Editor{

        private bool _expandSystemEnvironments = false;
        public override void OnInspectorGUI(){
            base.OnInspectorGUI();
            _expandSystemEnvironments = EditorGUILayout.Foldout(_expandSystemEnvironments,"SystemEnvironmentVars");
            if(_expandSystemEnvironments){
                var envs = System.Environment.GetEnvironmentVariables();
                foreach(var key in envs.Keys){
                    var value = envs[key];
                    EditorGUILayout.LabelField((string)key,(string)value);
                }   
            }
            if(GUILayout.Button("Execute")){
                (target as ShellExecutor).Execute();
            }
        }
    }
}
