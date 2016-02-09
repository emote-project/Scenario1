#!/bin/bash
cp /Users/tiagoribeiro/Secretaria/Projects/EMOTE-Modules-WIP/JavaExample/bin/Release/*.dll ./
cp /Users/tiagoribeiro/Secretaria/Projects/EMOTE-Modules-WIP/JavaExample/bin/Release/*.dll ikvm/bin/
mono ikvm/bin/ikvmstub.exe JavaExample.dll
mono ikvm/bin/ikvmstub.exe ExampleClientsInterface.dll
mono ikvm/bin/ikvmstub.exe Thalamus.dll
mono ikvm/bin/ikvmstub.exe ThalamusBMLInterfaces.dll
javac -g -classpath mscorlib.jar:JavaExample.jar:ExampleClientsInterface.jar:Thalamus.jar:ThalamusBMLInterfaces.jar JavaExampleClient.java
mono ikvm/bin/ikvmc.exe -nojni -target:exe  -platform:x86 -r:mscorlib.dll -r:/Library/Frameworks/Mono.framework/Versions/3.2.3/lib/mono/4.0/Microsoft.CSharp.dll -r:/Library/Frameworks/Mono.framework/Versions/3.2.3/lib/mono/4.0/System.Core.dll -r:JavaExample.dll -lib:/Users/tiagoribeiro/Dropbox/Sandbox/ikvm/ikvm/bin -r:ExampleClientsInterface.dll -r:Thalamus.dll -r:ThalamusBMLInterfaces.dll JavaExampleClient.class
cp JavaExampleClient.exe ikvm/bin/
mono ikvm/bin/JavaExampleClient.exe