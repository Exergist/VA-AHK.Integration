# VA-AHK.Integration (WORK IN PROGRESS)


## What is it?
VoiceAttack profile and associated files that allow the use of AutoHotkey v1 functionality within the VoiceAttack environment through C# inline functions.

AHK-VA.Integration provides a means to expand VoiceAttack's capabilities, more directly control AutoHotkey scripts, as well as process and pass variables back and forth between AutoHotkey and VoiceAttack.

## How does it work?
This project employs a modified version of [amazing-andrew's AutoHotkey.Interop](https://github.com/amazing-andrew/AutoHotkey.Interop) wrapper around [HotKeyIt's ahkdll](https://github.com/HotKeyIt/ahkdll) (aka AutoHotkey_H). 

## Installation
  <details>
    <summary>1. You need VoiceAttack. If it's already up and running then go to Step 2. Otherwise expand this section and read on.</summary><p>
  
1. According to www.voiceattack.com "VoiceAttack works with Windows 10 all the way back to Vista." So you've got to have one of those versions of Windows to even use VA. Note though that I've only tested VA.Change-WSR-Profile in Windows 7 and Windows 10.
2. VA-AHK.Integration will work with VoiceAttack v1.6.9 and later. There are currently two versions of the VoiceAttack software available: a purchasable full version and a free limited trial version (the trial version of VoiceAttack "gives you one profile with up to twenty commands"). You will need the licensed version of VoiceAttack to import the "Change WSR Profile.vap" file. The VoiceAttack software may be obtained at www.voiceattack.com (free trial and fully licensed versions) or through [Steam](http://store.steampowered.com/app/583010/VoiceAttack/) (licensed version only). I believe it would be possible to manually recreate the commands contained within the VA-AHK.Integration for use with the trial version, however I will not be covering that. In my opinion the low cost for the VoiceAttack license was totally worth it.
3. If you're unfamiliar with VoiceAttack this is a great time to check out the [VoiceAttack manual](http://voiceattack.com/VoiceAttackHelp.pdf) to acquaint yourself with the application. 
4. I'm going to assume you've already handled other VoiceAttack-related setup steps like training the voice profile, configuring your settings, etc. If you have not already done so then go read the manual so you can learn how to properly set up VoiceAttack. 
</p></details>

2. Navigate to [VA-AHK.Integration Releases](https://github.com/Exergist/VA-AHK.Integration/releases) and download the most recent version. All you'll need is the .vax file (VoiceAttack profile package), but feel free to check out the source if you wish.
3. Launch VoiceAttack (I'm assuming you have the licensed version), open the "More Profile Actions" tool (one of the "Profile management buttons"...read the manual if you don't know what this is), and select "Import Profile." Navigate to the folder where you downloaded the release file, select the release file (again, should be a .vax file), and confirm the selection. **You should receive a popup message telling you that the profile contains actions that execute applications and/or kill processes.** This is okay (explained more later), so confirm the profile import. Now you should have the profile "VA-AHK.Integration" available to you within VoiceAttack. The downloaded release file is no longer needed and you may delete it since the profile is now loaded in VoiceAttack's internal files.
4. Navigate to the directory where you have VoiceAttack installed. Then navigate through the folders Apps ==> VA-AHK Integration ==> MORE HERE and locate the file VA.AutoHotkey.Interop.dll. Move this file into the same folder where your VoiceAttack.exe is located.
5. This completes the installation of VA-AHK.Integration. Please see the next section for more details on using the new functionality. 

## How do I use it?

## Links
 - [amazing-andrew's AutoHotkey.Interop Repository](https://github.com/amazing-andrew/AutoHotkey.Interop)
 - [AutoHotKey\_H Documentation & Libraries](http://hotkeyit.github.io/v2/)
 - [HotKeyIt's AutoHotkey\_H Repository](https://github.com/HotKeyIt/ahkdll)

## Credits
 - Credit for the original AutoHotkey goes to Chris Mallet ([Source](https://autohotkey.com/foundation/history.html))
 - Credit for AutoHotkey_L (fork of AutoHotkey that is now the main branch) goes to "Lexikos" (Steve Gray) ([Source](https://github.com/Lexikos/AutoHotkey_L))
 - Additional credit for the continued support and development of the current version of AutoHotkey goes to The AutoHotkey Foundation and the AutoHotkey community ([Source](https://autohotkey.com/foundation/))
 - Credit for AutoHotkey_H (fork of AutoHotkey_L which provides the 32 and 64-bit AutoHotkey.dll) goes to tinku99 (inventor) and HotKeyIt (enhancements and continued development) ([Source](https://github.com/HotKeyIt/ahkdll))
 - Credit for AutoHotkey.Interop goes to "amazing-andrew" (Andrew Smith) ([Source](https://github.com/amazing-andrew/AutoHotkey.Interop))
 - Credit for the continued development of VoiceAttack goes to Gary at www.voiceattack.com
