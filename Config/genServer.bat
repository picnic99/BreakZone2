set LUBAN_DLL=..\LubanTools\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t all ^
    -d json ^
    -c cs-dotnet-json ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputDataDir=..\GameServer\StateSyncServer\StateSyncServer\Config ^
    -x outputCodeDir=..\GameServer\StateSyncServer\StateSyncServer\LogicScripts\VirtualClient\Configer\Config

pause