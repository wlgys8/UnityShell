# UnityShell

Execute shell commands in Unity Editor, support osx & windows

在Editor下执行windows和osx的命令行


# Install 

add follow line to `Packages/manifest.json`

在项目 `Packages/manifest.json` 配置如下的依赖

    "com.ms.shell":"https://github.com/wlgys8/UnityShell.git",

# Examples

- execute command  `ls`

``` csharp

var operation = EditorShell.Execute("ls");
operation.onExit += (exitCode)=>{
    
};
operation.onLog += (EditorShell.LogType LogType,string log)=>{
    Debug.Log(log);
};

int exitCode = await operation; //support async/await
```

* use `Options`

``` csharp

var options = new EditorShell.Options(){
    workDirectory = "./",
    encoding = System.Text.Encoding.GetEncoding("GBK"), 
    environmentVars = new Dictionary<string, string>(){
        {"PATH","usr/bin"},
    }
};

var operation = EditorShell.Execute("ls",options);
operation.onExit += (exitCode)=>{
    
};
operation.onLog += (EditorShell.LogType LogType,string log)=>{
    Debug.Log(log);
};  

```  

# Advanced

## Encoding 编码

By default, EditorShell use `UTF8` for encoding. If you get unrecognizable characters, please check your shell app's (`bash` in osx and `cmd.exe` in windows) encoding, and config options.encoding same as that.

在中文版的Windows上运行时，可能会出现乱码。 因为cmd采用了GBK编码，而UnityShell默认使用UTF8编码。 只要在Options里将encoding设置为GBK编码即可


## Environment Vars 环境变量

By default, environment vars is empty. You can config it in options.environmentVars.

默认情况下， 使用EditorShell执行命令行时是不带环境变量的。 你可以通过Options.environmentVars 字段来配置自己需要的环境变量。


