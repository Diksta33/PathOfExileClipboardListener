; .NET Install Script
; written in Inno Setup 5.5.4(a)

[Setup]
AppName=Exile Clipboard Listener
AppVerName=Exile Clipboard Listener
AppVersion=1.0.0.22
DefaultDirName={pf}\ExileClipboardListener
DefaultGroupName=ExileClipboardListener
UninstallDisplayIcon={app}\.exe
OutputDir=C:\Users\richan\Desktop\GitHub\PathOfExileClipboardListener\SetupPoEClipboardListener
OutputBaseFilename=SetupPoEClipboardListener

[Files]
Source: "C:\Users\richan\Desktop\GitHub\PathOfExileClipboardListener\ExileClipboardListenerExpress\bin\Release\ExileClipboardListenerExpress.exe"; DestDir: "{app}"; 
Source: "C:\Users\richan\Desktop\GitHub\PathOfExileClipboardListener\ExileClipboardListenerExpress\bin\Release\ExileStash.s3db"; DestDir: "{app}"; 
Source: "C:\Users\richan\Desktop\GitHub\PathOfExileClipboardListener\ExileClipboardListenerExpress\bin\Release\System.Data.SQLite.dll"; DestDir: "{app}"; 
;Source: "C:\Users\richan\Desktop\GitHub\PathOfExileClipboardListener\ExileClipboardListenerExpress\Externals\dotNetFx40_Client_x86.exe"; DestDir: "{tmp}"; Check: NeedsFramework

[Icons]
Name: "{group}\Exile Clipboard Listener"; Filename: "{app}\ExileClipboardListenerExpress.exe"

[Run]
;Filename: {tmp}\dotnetfx.exe; Parameters: "/q:a /c:""install /l /q"""; WorkingDir: {tmp}; Flags: skipifdoesntexist; StatusMsg: "Installing .NET Framework if needed"
Filename: {win}\Microsoft.NET\Framework\v2.0.50727\CasPol.exe; Parameters: "-q -machine -remgroup ""Exile Clipboard Listener"""; WorkingDir: {tmp}; Flags: skipifdoesntexist runhidden; StatusMsg: "Setting Program Access Permissions..."
Filename: {win}\Microsoft.NET\Framework\v2.0.50727\CasPol.exe; Parameters: "-q -machine -addgroup 1.2 -url ""file://{app}/*"" FullTrust -name ""Exile Clipboard Listener"""; WorkingDir: {tmp}; Flags: skipifdoesntexist runhidden; StatusMsg: "Setting Program Access Permissions..."

[UninstallRun]
Filename: {win}\Microsoft.NET\Framework\v2.0.50727\CasPol.exe; Parameters: "-q -machine -remgroup ""Exile Clipboard Listener"""; Flags: skipifdoesntexist runhidden;

[Code]

//Indicates whether .NET Framework 4.0 is installed.
//function IsDotNET40Detected(): boolean;
//var
//    success: boolean;
//    install: cardinal;
//begin
//    success := RegQueryDWordValue(HKLM, 'SOFTWARE\Microsoft\NET Framework Setup\NDP\v4.0\Client', 'Install', install);
//    Result := success and (install = 1);
//end;

//function NeedsFramework(): Boolean;
//begin
//  Result := (IsDotNET40Detected = false);
//end;

//function GetCustomSetupExitCode(): Integer;
//begin
//  if (IsDotNET40Detected = false) then
//    begin
//      MsgBox('.NET Framework was NOT installed successfully!',mbError, MB_OK);
//      result := -1
//    end
//end;

function InitializeSetup: Boolean;
begin
  //Make sure the user has Admin rights
  if IsAdminLoggedOn then
    begin
      result := true
        exit;
    end
  else
    begin
      MsgBox('You must have admin rights to perform this installation.' + {NL} +
             'Please log on with an account that has administrative rights,' + {NL} +
            'and run this installation again.', mbInformation, MB_OK);
        result := false;
    end
  end;
end.