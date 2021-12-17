# About
This console app was created as a part of test task.

Framework used: .NET5

## Installation

1. .NET5 SDK should be installed.
Clone this repo locally, open a cmd/powershell window from the directory that contains WinProcessMonitor.csproj and finally run dotnet command:

```bash
dotnet publish --output Release --runtime win-x64 --configuration Release -p:PublishSingleFile=true -p:Version=1.0.1-preview2-final --self-contained false
```
You can run .exe form the 'Release' folder.

2. Without .NET5
3. 
If you want to run compiled .exe just download the build artifacts and run command (in the root folder):

(example)
```bash
WinProcessMonitor.exe -name notepad -lifetime 1 -frequency 1
```

## License
[MIT](https://choosealicense.com/licenses/mit/)
