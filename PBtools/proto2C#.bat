@echo off 
rem set /p input=请拖动要转换的文件夹：
rem echo %input%等待转换
set input=./proto
echo 开始转换%input%目录下的proto文件
for /r %input% %%f in (*) do (
    echo 已转换：[%%~nxf] to [./output/%%~nf.cs]
    call protoc.exe -I %input%  %%~nxf --csharp_out ./output

)


pause