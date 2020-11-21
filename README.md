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

Built with MRTK 2.5.1, Unity 2019 LTS and Visual Studio 2019.
Requires Webcam and Spatial Perception capabilities in UWP appx.

### Hololens/Windows Mixed Reality:

#### Building from Unity
1. The first step is to build from the Unity editor to a Visual Studio solution for a UWP app.
1. Open the Build Settings Window (Cntrl+Shift+B shortcut) via File > Build Settings.
1. Ensure the Universal Windows Platform is applied and that the root scene is added to the Scenes in Build.
1. Click the Build button and select a folder for where the Visual Studio solution will be placed.
1. For faster build times, it is recommended to build to the same folder every iteration, disable anti-malware scanning software, and utilize an SSD.

#### Building Visual Studio
1. After Unity finished building the UWP Visual Studio solution, a file explorer should automatically appear with the build folder as focus. Open this folder and open the .sln file.
1. With Visual Studio open, one can change any UWP appx package changes such as icons and app name, etc., by double clicking the Package.appxmanifest file under the Universal Windows project.
1. Next step is to build the UWP Visual Studio solution to effectively create an Appx package that can be deployed to device.
1. Select Master or Release (Debug will run very slow on device).
1. Select ARM as the architecture type (HoloLens 2 has an ARM based processor, HoloLens 1 has an x86 processor).
1. Ensure build target is Device.
1. Select **Build tab** > **Build solution**.
1. Once the build completes, the app can be deployed via **Build tab** > **Deploy Solution**. This assumes you have developer [unlocked and paired you HoloLens device to your PC and that the HoloLens](https://docs.microsoft.com/en-us/windows/mixed-reality/develop/platform-capabilities-and-apis/using-visual-studio) is connect via USB.
1. [HoloLens Emulator notes](https://docs.microsoft.com/en-us/windows/mixed-reality/develop/platform-capabilities-and-apis/using-the-hololens-emulator)

