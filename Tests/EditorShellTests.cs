using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NUnit.Framework;
using UnityEngine.TestTools;
using System.Threading.Tasks;

namespace MS.Shell.Editor.Tests{
    public class EditorShellTests
    {


        [UnityTest]
        public IEnumerator EchoHelloWorld(){
            var task = EditorShell.Execute("echo hello world",new EditorShell.Options());
            task.onLog += (logType,log)=>{
                Debug.Log(log);
                LogAssert.Expect(LogType.Log,"hello world");
            };
            task.onExit += (code)=>{
                Debug.Log("Exit with code = " + code);
                Assert.True(code == 0);
            };
            yield return new ShellOperationYieldable(task);
        }

        [UnityTest]
        public IEnumerator EchoAsync(){
            var task = ExecuteShellAsync("echo hello world");
            yield return new TaskYieldable<int>(task);
            Assert.True(task.Result == 0);
        }


        [UnityTest]
        public IEnumerator ExitWithCode1Async(){
            var task = ExecuteShellAsync("exit 1");
            yield return new TaskYieldable<int>(task);
            Debug.Log("exit with code = " + task.Result);
            Assert.True(task.Result == 1);
        }

        [UnityTest]
        public IEnumerator KillAsyncOperation(){
            var operation = EditorShell.Execute("sleep 5",new EditorShell.Options());
            KillAfter1Second(operation);
            var task = GetOperationTask(operation);
            yield return new TaskYieldable<int>(task);
            Debug.Log("exit with code = " + task.Result);
            Assert.True(task.Result == 137);
        }

        private async void KillAfter1Second(EditorShell.Operation operation){
            await Task.Delay(1000);
            operation.Kill();
        }

        private async Task<int> GetOperationTask(EditorShell.Operation operation){
            int code = await operation; 
            return code;
        }

        private async Task<int> ExecuteShellAsync(string cmd){
            var task = EditorShell.Execute(cmd,new EditorShell.Options());
            int code = await task; 
            return code;  
        }


        private class ShellOperationYieldable:CustomYieldInstruction{
            private EditorShell.Operation _operation;
            public ShellOperationYieldable(EditorShell.Operation operation){
                _operation = operation;
            } 
            public override bool keepWaiting {
                get{
                    return !_operation.isDone;
                }
            }
        }


        private class TaskYieldable<T>:CustomYieldInstruction{

            private Task<T> _task;
            public TaskYieldable(Task<T> task){
                _task = task;
            }
            public override bool keepWaiting {
                get{
                    return !_task.IsCompleted;
                }
            }

        }
    }
}
