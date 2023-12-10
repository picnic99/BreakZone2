set LUBAN_DLL=..\LubanTools\Luban\Luban.dll
set CONF_ROOT=.

dotnet %LUBAN_DLL% ^
    -t all ^
    -d json ^
    -c cs-simple-json ^
    --conf %CONF_ROOT%\luban.conf ^
    -x outputDataDir=output\json ^
    -x outputCodeDir=..\Assets\LogicScripts\Configer\Config

pause