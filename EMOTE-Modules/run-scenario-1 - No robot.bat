@echo off

pushd %~dp0\..


pushd Thalamus\Binaries\Release
start ThalamusStandalone.exe emote
popd


pushd EMOTE-Modules

pushd ControlPanel\ControlPanelV2\bin\Release\
start ControlPanel.exe emote
popd


pushd IntManInterface\InteractionManagerInterface\bin\Release\
start IntManInterface.exe emote
popd

pushd MapInterface\MapInterface\bin\Release\
start MapInterface.exe emote
popd

pushd Skene\Skene\bin\Release
start Skene.exe emote
popd



pushd LearnerModelThalamus\LearnerModelThalamus\bin\Release
start LearnerModelThalamus.exe emote
popd

pushd Perception\bin\x64\Release
start Perception.exe emote
popd
timeout 2

REM echo Perception module loaded. Press any key to continue..
REM pause


REM pushd OKAOmodule\bin
REM start starter.bat
REM popd

pushd OKAOmodule\bin
start starter.bat
popd

timeout 1
pushd InteractionAnalysis\InteractionAnalysis\bin\Release
start InteractionAnalysis.exe emote
popd


popd

REM Load the web page for the map application
start http://localhost:8080/namshub


pushd Thalamus\Example Modules\ThalamusSpeechClient\bin\Release
start ThalamusSpeechClient.exe emote
popd


echo.
echo.
echo.
echo Press any key to start interaction manager when ready
echo.

REM pause

pushd EMOTE-Modules
REM start java -jar interaction-manager-java\interaction-manager-java.jar -scriptsDir .\interaction-manager-java -scriptFile scripts\scenario1emote4.xml -scenariosDir .\S1ScenarioXMLFiles
start java -jar interaction-manager-java\interaction-manager-java.jar -scriptsDir .\interaction-manager-java -scriptFile scripts\scenario1emote4.xml -scenarioDir .\S1ScenarioXMLFiles
popd

popd
