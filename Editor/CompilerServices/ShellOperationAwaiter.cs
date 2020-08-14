using System;
using System.Runtime.CompilerServices;

namespace MS.Shell.Editor.CompilerServices{
    public struct ShellOperationAwaiter: ICriticalNotifyCompletion
    {

        private EditorShell.Operation _operation;
        public ShellOperationAwaiter(EditorShell.Operation operation){
            _operation = operation;
        }

        public int GetResult(){
            return _operation.exitCode;
        }

        public bool IsCompleted{
            get{
                return _operation.isDone;
            }
        }

        public void OnCompleted(Action continuation)
        {
            UnsafeOnCompleted(continuation);
        }

        public void UnsafeOnCompleted(Action continuation)
        {
            if(IsCompleted){
                continuation();
            }else{
                _operation.onExit += (code)=>{
                    continuation();
                };
            }
        }
    }
}
