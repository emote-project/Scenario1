@echo off

pushd %~dp0\..


pushd Thalamus\Binaries\Release
start ThalamusStandalone.exe emote
popd

REM Give those both some time to start ...
echo Waiting for Thalamus and Enercities to get started
timeout 3

pushd EMOTE-Modules

pushd LearnerModelThalamus\LearnerModelThalamus\bin\Release
start LearnerModelThalamus.exe emote
popd

pushd MapInterface\MapInterface\bin\Release\
start MapInterface.exe emote
popd

timeout 2

pushd ControlPanel\ControlPanelV2\bin\Release\
start ControlPanel.exe emote
popd








