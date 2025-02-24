# Plugin/mod configuration manager for [Star Trek Fleet Command - Community Patch](https://github.com/netniV/bob)
Designed to use with [BepInEx.Legacy](https://github.com/Plurimus/BepInEx.Legacy) framework and [STFC Community Patch](https://github.com/netniV/bob/releases/tag/v0.6.1.alpha.5)

Tested with STFC client v1.000.40895 (Update 75.1.0)

## How it works
This mod adds "Community Mod" section in game client setting menu to change Community Patch settings that will apply after reboot.
The mod's logic is 
- read running values from `community_patch_runtime.vars`
- show them in settings menu
- allow users to change them 
- write changes to `community_patch_settings.toml`. So, the actual changes applies ONLY after the client reboot

## How to install
- unpack BepInEx framework archive into STFC game folder which is usually located at `C:\Games\Star Trek Fleet Command\Star Trek Fleet Command\default\game`
- run the game client and wait until BepInEx will make unhollowed assemblies in `..\BepInEx\unhollowed\` folder
- unpack Optimus.STFC.ConfigManager archive to BepInEx folder so it will copy `Optimus.STFC.ConfigManager.dll` and `Tomlyn.dll` to `C:\Games\Star Trek Fleet Command\Star Trek Fleet Command\default\gameBepInEx\plugins\`
- make a link file to `prime.exe` and start the game client through it to avoid automatic updates 

## Result screenshots
![ComMod Configuration manager gif](Screenshot.gif)
