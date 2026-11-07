# LAN STRIKE — Unity project foundation

Unity 6 project foundation for Android and Windows. This is NOT an APK yet.

## Included
- Unity project structure and package manifest
- Editor tool that generates a 3D military training-ground scene
- First-person movement, keyboard/mouse and touch look
- Hitscan rifle and basic target health
- Mobile joystick and hold-button components
- GitHub Actions Android build workflow template

## Create the scene
1. Open this folder in Unity 6.
2. Wait for packages to import.
3. Choose Tools > LAN STRIKE > Build Training Ground Scene.
4. Open Assets/Scenes/LanStrikeTrainingGround.unity and press Play.

## Build
Android builds require Android Build Support (SDK, NDK, OpenJDK) and a valid Unity license.
The GitHub Actions workflow requires a repository Actions secret named UNITY_LICENSE.
It uses GameCI; it does not bypass Unity licensing.

## Controls
PC: WASD, mouse, left mouse fire, Space jump, Left Shift sprint.
Mobile: on-screen touch controls; gyro toggle can be added to the HUD.

## Scope honestly
This is a prototype foundation. It does not yet contain completed LAN multiplayer,
finished weapon models/skins, production anti-cheat, polished animations/audio, or a verified APK.
