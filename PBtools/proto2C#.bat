@echo off 
rem set /p input=���϶�Ҫת�����ļ��У�
rem echo %input%�ȴ�ת��
set input=./proto
echo ��ʼת��%input%Ŀ¼�µ�proto�ļ�
for /r %input% %%f in (*) do (
    echo ��ת����[%%~nxf] to [./output/%%~nf.cs]
    call protoc.exe -I %input%  %%~nxf --csharp_out ./output

)


pause