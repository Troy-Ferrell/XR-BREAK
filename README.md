# XR-BREAK

This is an example XR Application using Unity and the [Mixed Reality Toolkit](https://github.com/microsoft/MixedRealityToolkit-Unity). Tutorial series can be found [here](https://medium.com/@troyferrell/building-an-xr-application-in-unity-with-mrtk-cae49483e49d).

The application was designed for the HoloLens 2 device and makes use of hand tracking, voice commands, and Spatial Mapping. The player has a ball that can be launched using hands and gestures to collide with targets randomly placed in the environment. Bulls-eye targets must be hit by the ball while the player needs to shoot the ball through ring targets to score.

![Demo](./Documentation/XR-Break-GameDemo.gif)

## How to play

1. Launch application
1. Scan environment to build scene map
1. Targets will begin to randomly appear
1. Show and close either hand and then hold this gesture. The longer this gesture is held, the more powerful the ball will fire.
1. Open hand to launch the ball and try to capture the targets to score.
1. Quickly close/open your any hand to return the ball to your person

Say "Show scoreboard" or "Hide scoreboard" to toggle the scoring UI.
Say "Toggle Profiler" to toggle visibility of the diagnostics profiler HUD

## How to 

Built with MRTK 2.3, Unity 2018 LTS and Visual Studio 2019.
Requires Webcam and Spatial Perception capabilities in UWP appx.

[How to build](https://medium.com/@troyferrell/building-an-xr-application-in-unity-with-mrtk-part-7-building-the-app-d98d56425296)
