# UnityShell

在Editor下执行windows和osx的命令行

# Examples

* execute command  `ls`

        var task = EditorShell.Execute("ls");
        task.onExit += (exitCode)=>{
            
        };
        task.onLog += (EditorShell.LogType LogType,string log)=>{
            Debug.Log(log);
        };