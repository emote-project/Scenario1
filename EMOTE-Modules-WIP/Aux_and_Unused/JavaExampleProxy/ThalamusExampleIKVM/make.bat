set cSharkProxyAssembly=JavaExampleProxy
set cSharpProxyBinPath=C:\Emote\MercurialRepos\EMOTE-Modules-WIP\JavaExampleProxy\bin\Release\
set cSharkProxyInterfaces=ExampleClientsInterface
set javaClientClass=JavaExampleClient

rmdir /S /Q work
rmdir /S /Q bin
mkdir work

copy /y %cSharpProxyBinPath%*.dll work
copy /y ikvm\bin\*.* work
copy /y %javaClientClass%.java work

cd work

ikvmstub %cSharkProxyAssembly%.dll
ikvmstub Thalamus.dll
ikvmstub %cSharkProxyInterfaces%.dll

javac -classpath mscorlib.jar;%cSharkProxyAssembly%.jar;%cSharkProxyInterfaces%.jar;Thalamus.jar; %javaClientClass%.java

ikvmc -nojni -target:exe -platform:x86 -r:mscorlib.dll -r:Microsoft.CSharp.dll -r:System.Core.dll -r:%cSharkProxyAssembly%.dll -lib:.\ -r:%cSharkProxyInterfaces%.dll -r:Thalamus.dll %javaClientClass%.class %javaClientClass%$1.class

mkdir ..\bin
copy /y *.dll ..\bin
copy %javaClientClass%.exe ..\bin
cd ..
%rmdir /S /Q work

pause