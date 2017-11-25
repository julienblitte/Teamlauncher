#define ApplicationVersion GetFileVersion('Teamlauncher.exe')

[Setup]
AppName=Teamlauncher
AppVersion={#ApplicationVersion}
VersionInfoVersion={#ApplicationVersion}
VersionInfoCopyright=Julien Blitte
DefaultDirName={pf}\Teamlauncher
DefaultGroupName=Teamlauncher
UninstallDisplayIcon={app}\Teamlauncher.exe
Compression=lzma2
SolidCompression=yes
OutputBaseFilename=Teamlauncher Setup
OutputDir=.

[Files]
Source: "Teamlauncher.exe"; DestDir: "{app}"
Source: "Teamlauncher.xml"; DestDir: "{app}"; Flags: onlyifdoesntexist
Source: "Teamlauncher.exe.config"; DestDir: "{app}"

[Icons]
Name: "{group}\Teamlauncher"; Filename: "{app}\Teamlauncher.exe"
Name: "{group}\Uninstall Teamlauncher"; Filename: "{uninstallexe}"

[Components]
Name: startup; Description: Starts automatically with Windows

[Registry]
Root: HKLM; Subkey: "SOFTWARE\Microsoft\Windows\CurrentVersion\Run"; ValueType: string; ValueName: "Teamlauncher"; ValueData: """{app}\Teamlauncher.exe"" -startup";  Components: startup
