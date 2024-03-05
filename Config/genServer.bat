set LUBAN_DLL=..\LubanTools\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t all ^
    -d json ^
    -c cs-dotnet-json ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputDataDir=D:\u3dPoj\BreakZone2\GameServer\StateSyncServer\StateSyncServer\Config ^
    -x outputCodeDir=D:\u3dPoj\BreakZone2\GameServer\StateSyncServer\StateSyncServer\LogicScripts\VirtualClient\Configer\Config

pause