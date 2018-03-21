# VA-AHK.Integration (WORK IN PROGRESS)

## What is it?
VoiceAttack profile and associated files that allow the use of AutoHotkey v1 functionality within the VoiceAttack environment through C# inline functions. AHK-VA.Integration expands VoiceAttack's capabilities, provides more control over AutoHotkey processing, and enables easy passing of information back and forth between AutoHotkey and VoiceAttack.

## How does it work?
This project employs a modified version of [amazing-andrew's AutoHotkey.Interop](https://github.com/amazing-andrew/AutoHotkey.Interop) wrapper around [HotKeyIt's ahkdll](https://github.com/HotKeyIt/ahkdll) (aka AutoHotkey_H). This provides the means to access AutoHotkey functionality (from AutoHotkey.dll) for running scripts, executing AutoHotkey code, etc. via a C# inline function within VoiceAttack. 

## How do I install it?
  <details>
    <summary>1. First you need VoiceAttack. If you've already got it then go to Step 2. Otherwise expand this section and read on.</summary><p>
  
1. According to www.voiceattack.com "VoiceAttack works with Windows 10 all the way back to Vista." So you've got to have one of those versions of Windows to even use VA. Note though that I've only tested VA-AHK.Integration in Windows 7 and Windows 10.
2. VA-AHK.Integration will work with VoiceAttack v1.6.9 and later. There are currently two versions of the VoiceAttack software available: a purchasable full version and a free limited trial version (the trial version of VoiceAttack "gives you one profile with up to twenty commands"). You will need the licensed version of VoiceAttack to import the *VA-AHK.Integration.vax* package file. The VoiceAttack software may be obtained at www.voiceattack.com (free trial and fully licensed versions) or through [Steam](http://store.steampowered.com/app/583010/VoiceAttack/) (licensed version only). I believe it would be possible to manually recreate the commands contained within the VA-AHK.Integration for use with the trial version, however I will not be covering that. In my opinion the low cost for the VoiceAttack license was totally worth it.
3. If you're unfamiliar with VoiceAttack this is a great time to check out the [VoiceAttack manual](http://voiceattack.com/VoiceAttackHelp.pdf) to acquaint yourself with the application. 
4. I'm going to assume you've already handled other VoiceAttack-related setup steps like training the voice profile, configuring your settings, etc. If you have not already done so then go read the manual so you can learn how to properly set up VoiceAttack. 
</p></details>

2. Navigate to [VA-AHK.Integration Releases](https://github.com/Exergist/VA-AHK.Integration/releases) and download the most recent version. All you'll need is the *.vax* package (VoiceAttack profile package), but feel free to check out the source if you wish.
3. Launch VoiceAttack (I'm assuming you have the licensed version). Go into the *Options* (wrench icon) and turn off (uncheck) the *Enable Plugin Support* setting within the *General* tab. The *.vax* file contains libraries (*.dll* files) and thus this option needs to be disabled in order to properly import all the data. Confirm the *Options* change. 
4. Open the *More Profile Actions* tool (one of the *Profile management buttons*...read the manual if you don't know what this is), and select *Import Profile*. Navigate to the folder where you downloaded the release file, select the release file (again, should be a *.vax* package file), and confirm the selection. You should receive a popup message confirming the profile import. Now you should have the profile *VA-AHK.Integration* (including version) available to you within VoiceAttack. The downloaded release file is no longer needed and you may delete it since the profile is now loaded in VoiceAttack's internal files.
5. Navigate to VoiceAttack's root directory (the same folder where VoiceAttack is installed and your VoiceAttack.exe is located). Then navigate through the folders *Apps* ==> *VA-AHK.Integration*. Here you will find all the files that were included with the *.vax* package in addition to the imported VoiceAttack profile. Locate the file *VA.AutoHotkey.Interop.dll* and move this file into VoiceAttack's root directory.
6. This completes the installation of VA-AHK.Integration. 

## How do I use it?
Comprehensive examples of the C# inline functions used to invoke the AutoHotkey functionality are given in the included VoiceAttack profile. The process basically comes down to three steps:
  1. Create an AutoHotkey thread
  2. Execute AutoHotkey raw code, scripts, and/or other related tasks using the existing thread (in parallel or series with other C# code execution)
  3. Terminate the AutoHotkey thread and thereby stop all AutoHotkey processing from within VoiceAttack
  
The command *Single Inline Function Example* 

You should note that VA-AHK.Integration does is not require installation of the actual AutoHotkey software. There is no issue with having both the actual AutoHotkey software and VA-AHK.Integration installed simultaneously. In fact they each can run the same or different versions of the base AutoHotkey software (for example, AutoHotkey at v1.1.27.00 but VA-AHK.Integration at v1.1.28.00). 

## How do I update it?
The version of AutoHotkey functionality available via VA-AHK.Integration is dictated by the *AutoHokey.dll* files within the *x86* and *x64* folders. So if you happen to also have the actual AutoHotkey software installed, updating the software will not update the AutoHotkey version available through VA-AHK.Integration. It appears that HotKeyIt is updating the *AutoHotkey.dll* files for both AutoHotkey v1 and the alpha v2 as new versions of AutoHotkey are released. The files may be found in the *Downloads* section at [HotKeyIt's AutoHotkey\_H Repository](https://github.com/HotKeyIt/ahkdll), and he also provides links to the source code. **VA-AHK.Integration was created with AutoHotkey v1 in mind, but there's no reason why it wouldn't also work (or be modified to work) with v2.** 

There are two ways to update the AutoHotkey version in VA-AHK.Integration:
  1. If new VA-AHK.Integration releases are available the *.vax* import process will automatically update your existing files (assuming the package includes a newer/different version of AutoHotkey compared to what you already installed).
  2. To update VA-AHK.Integration manually, take the updated *AutoHotkey.dll* files from the *Win32w* and *x64w* folders from *ahkdll* (see above link) and move them into VA-AHK.Integration's *x86* (32-bit) and *x64* (64-bit) folders, respectively. 

You can check the version of AutoHotkey employed by VA-AHK.Integration by creating an AutoHotkey thread and then running:
```C# 
ahk.ExecRaw("MsgBox, AHK version =  %A_AhkVersion%");
```
The Multi-Function Example include within the VoiceAttack profile fully demonstrates how to retrieve the installed AutoHotkey version. 

## Help I have issues!
First and foremost, **_[read the VoiceAttack manual](http://voiceattack.com/VoiceAttackHelp.pdf)_**. Yes it's long, but it covers most of what is needed for you to understand and use VoiceAttack. Plus it covers more advanced stuff which is great to know so you can do other super cool things. 

If you are having problems with the VA software itself there is a [VoiceAttack User Forum](http://voiceattack.com/SMF/index.php) where you may seek help. The community is active and full of dedicated users who will help you with your general VA issues. Plus it's a great place to learn more about VA so you can do other super cool things as well as check out profiles and commands that other users have shared. 

If you are having problems specifically with the VA-AHK.Integration commands or code then head over to the VoiceAttack User Forum, check out the "Issues" section to get an idea for how to provide enough information to request assistance, and then post to the "AutoHotkey Integration with VoiceAttack" thread within the "Inline Functions" section of the forum.

## Credits
 - Credit for the original AutoHotkey goes to Chris Mallet ([Source](https://autohotkey.com/foundation/history.html))
 - Credit for AutoHotkey_L (fork of AutoHotkey that is now the main branch) goes to "Lexikos" (Steve Gray) ([Source](https://github.com/Lexikos/AutoHotkey_L))
 - Additional credit for the continued support and development of the current version of AutoHotkey goes to The AutoHotkey Foundation and the AutoHotkey community ([Source](https://autohotkey.com/foundation/))
 - Credit for AutoHotkey_H (fork of AutoHotkey_L which provides the 32 and 64-bit AutoHotkey.dll) goes to tinku99 (inventor) and HotKeyIt (enhancements and continued development) ([Source](https://github.com/HotKeyIt/ahkdll))
 - Credit for AutoHotkey.Interop goes to "amazing-andrew" (Andrew Smith) ([Source](https://github.com/amazing-andrew/AutoHotkey.Interop))
 - Credit for the creation and continued development of VoiceAttack goes to Gary at www.voiceattack.com

## Full Disclosure
I am one of the moderators on the VoiceAttack User Forums, and I receive no benefits from the use or promotion of the VoiceAttack software. 

## Links
 - [amazing-andrew's AutoHotkey.Interop Repository](https://github.com/amazing-andrew/AutoHotkey.Interop)
 - [AutoHotKey\_H Documentation & Libraries](http://hotkeyit.github.io/v2/)
 - [HotKeyIt's AutoHotkey\_H Repository](https://github.com/HotKeyIt/ahkdll)
 - [AutoHotkey](https://autohotkey.com/)
 - [VoiceAttack](http://voiceattack.com/)
