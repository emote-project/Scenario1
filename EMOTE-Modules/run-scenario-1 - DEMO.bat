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




REM pushd SpeakerRapport\SpeakerRapportGUI\bin\Release
REM start SpeakerRapportGUI.exe
REM popd

pushd LearnerModelThalamus\LearnerModelThalamus\bin\Release
start LearnerModelThalamus.exe emote
popd

REM OLD MODULE FROM LEE
REM pushd AffectPerception\AffectPerception\bin\Release
REM start AffectPerception.exe emote
REM popd


pushd Perception\bin\x64\Release
start Perception.exe emote
popd
timeout 2

REM echo Perception module loaded. Press any key to continue..
REM pause


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

REM pushd NAOBridges\NAOThalamusGUI\bin\Release
REM start NaoThalamusGUI.exe emote

REM ensure that you have started PUTTY and run the python script
REM change IP address of NAO below if necessary

pushd NAOBridges\NAOThalamusGUI\bin\Release
start NAOThalamusGUI.exe emote 192.168.1.20
popd


REM pushd Thalamus\Example Modules\ThalamusSpeechClient\bin\Release
REM start ThalamusSpeechClient.exe emote
REM popd


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
