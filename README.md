# VA-AHK.Integration

## What is it?
This project includes a VoiceAttack profile and associated files that allow the use of AutoHotkey v1 functionality within the VoiceAttack environment through C# inline functions. AHK-VA.Integration expands VoiceAttack's capabilities, provides more control over AutoHotkey processing, and enables easy passing of information back and forth between AutoHotkey and VoiceAttack.

## How does it work?
This project employs a modified version of [amazing-andrew's AutoHotkey.Interop](https://github.com/amazing-andrew/AutoHotkey.Interop) wrapper around [HotKeyIt's ahkdll](https://github.com/HotKeyIt/ahkdll), which is also known as AutoHotkey_H and is a fork of [AutoHotkey_L](https://github.com/Lexikos/AutoHotkey_L) (which is the main project behind the AutoHotkey software). This provides the means to access AutoHotkey functionality (from *AutoHotkey.dll*) for running scripts, executing AutoHotkey code, etc. via a C# inline function within VoiceAttack. 

## How do I install it?
  <details>
    <summary>1. First you need VoiceAttack (licensed version for this guide). If you've already got it then go to Step 2. Otherwise expand this section and read on.</summary><p>
  
1. According to www.voiceattack.com "VoiceAttack works with Windows 10 all the way back to Vista." So you've got to have one of those versions of Windows to even use VoiceAttack. Note though that I've only tested VA-AHK.Integration in Windows 7 and Windows 10.
2. VA-AHK.Integration will work with VoiceAttack v1.7.2 and later. There are currently two versions of the VoiceAttack software available: a purchasable full version and a free limited trial version (the trial version of VoiceAttack "gives you one profile with up to twenty commands"). You will need the licensed version of VoiceAttack to import the *VA-AHK.Integration.vax* package file. The VoiceAttack software may be obtained at www.voiceattack.com (free trial and fully licensed versions) or through [Steam](http://store.steampowered.com/app/583010/VoiceAttack/) (licensed version only). I believe it would be possible to manually recreate the commands contained within the VA-AHK.Integration for use with the trial version, however I will not be covering that. In my opinion the low cost for the VoiceAttack license is totally worth it.
3. If you're unfamiliar with VoiceAttack this is a great time to check out the [VoiceAttack manual](http://voiceattack.com/VoiceAttackHelp.pdf) to acquaint yourself with the application. 
4. I'm going to assume you've already handled other VoiceAttack-related setup steps like training the voice profile, configuring your settings, etc. If you have not already done so then go read the manual so you can learn how to properly set up VoiceAttack. 
</p></details>

2. Navigate to [VA-AHK.Integration Releases](https://github.com/Exergist/VA-AHK.Integration/releases) and download the most recent version. All you'll need is the *.vax* file (VoiceAttack profile package), but feel free to check out the source if you wish.
3. Launch VoiceAttack (I'm assuming you have the licensed version). Go into the *Options* (wrench icon) and turn off (uncheck) the *Enable Plugin Support* setting within the *General* tab. The *.vax* file contains libraries (*.dll* files) and thus this option needs to be disabled in order to properly import all the data. Confirm the *Options* change. 
4. Expand the *More Profile Actions* menu (one of the *Profile management buttons*...read the manual if you don't know what this is), and select *Import Profile*. Navigate to the folder where you downloaded the release file, select the release file (again, should be a *.vax* package file), and confirm the selection. You should receive a popup message for confirming the profile import along with a warning message about the content of the imported data, which you can ignore. Now you should have the profile *VA-AHK.Integration* (including version) available to you within VoiceAttack. The downloaded release file is no longer needed and you may delete it since the profile is now loaded in VoiceAttack's internal files. If you turned off *Enable Plugin Support* you may turn it back on now, and you can change the profile name to whatever you like. 

This completes the installation of VA-AHK.Integration. 

## How do I use it?
Comprehensive examples of the C# inline functions used to invoke the AutoHotkey functionality are given in the included VoiceAttack profile. The process basically comes down to three steps:
  1. Create an AutoHotkey thread
  2. Execute AutoHotkey raw code, scripts, and/or other related tasks using the existing thread (in parallel or series with other C# code execution)
  3. Terminate the AutoHotkey thread and thereby stop all AutoHotkey processing from within VoiceAttack
  
The VoiceAttack command *Single Function Example* showcases how to perform a wide range of AutoHotkey actions using VA-AHK.Integration, and this command contains all the AutoHotkey functionality within a single C# inline function. The array of commands in the *Multi-Function Example* shows how to execute the above three steps in C# inline functions spread across multiple commands (which can be handy since an AutoHotkey thread will run within the VoiceAttack process instance until either are terminated). Both examples also demonstrate how to pass information back and forth between VoiceAttack, the C# inline function, and the AutoHotkey process invoked via the C# inline function.  The *Keypress Capture* command demonstrates how to use the functionality across multiple C# inline functions within one command. Finally the *Change Default Windows TTS Voice* command showcases the execution of a single AutoHotkey script from within one C# inline function. All the VoiceAttack commands and their C# inline functions are provided with detailed comments so you can follow along with what is happening line by line. 

When you create an AutoHotkey thread it runs in the background within the VoiceAttack process instance. The thread is terminated only via the associated VA-AHK.Integration code:
```C#
ahk.Terminate();
``` 
...or by shutting down VoiceAttack. Pressing the "Stop Commands" button on VoiceAttack's UI **will NOT** terminate an active AutoHotkey thread. 

You should note that VA-AHK.Integration does is not require installation of the actual AutoHotkey software. There is no issue with having both the actual AutoHotkey software and VA-AHK.Integration installed simultaneously. In fact they each can run the same or different versions of the base AutoHotkey software (for example, AutoHotkey at v1.1.27.00 but VA-AHK.Integration at v1.1.28.00). Additionally you can actually run both at the same time. Though keep in mind that a hierarchy will apply based on the order that scripts execute. For example, if you specify from VA-AHK.Integration that the hotkey CTRL+A creates a popup message and then specify from actual AutoHotkey that CTRL+A presses the "delete" key then the "delete" action will be performed (it overwrites the message box action). If you reverse the order of executing the scripts then the message box will take priority. Based on my testing general processing for VA-AHK.Integration and actual AutoHotkey are separate. For example you can run one script in actual AutoHotkey that has an ExitApp command (which will terminate the actual AutoHotkey thread), then run VA-AHK.Integration, and finally execute the ExitApp command (which will stop the actual AutoHotkey processing) without interfering with the VA-AHK.Integration processing. 

As far as I know whatever you can do natively in AutoHotkey you can do with VA-AHK.Integration. Though it's important to note that running ExitApp by any means from within VA-AHK.Integration will cause VoiceAttack to shut down. You should also be aware that certain AutoHotkey errors encountered during processing withing VoiceAttack will shut down VoiceAttack, so make sure your base AutoHotkey code works by itself before you start integrating it with VoiceAttack via VA-AHK.Integration. 

## How do I update it?
The version of AutoHotkey functionality available via VA-AHK.Integration is dictated by the *AutoHokey.dll* files within the *x86* and *x64* folders. You an access these folders by opening VoiceAttack's root directory in the File Explorer (contains the *VoiceAttack.exe* file) and then navigating to *Apps\VA-AHK.Integration.* If you happen to also have the actual AutoHotkey software installed, updating the software will not update the AutoHotkey version available through VA-AHK.Integration. It appears that HotKeyIt is updating the *AutoHotkey.dll* files, for both AutoHotkey v1 and the alpha v2, not long after new versions of AutoHotkey are released. The files may be found in the *Downloads* section at [HotKeyIt's AutoHotkey\_H Repository](https://hotkeyit.github.io/v2/), and the author also provides links to the source code. As previously mentioned, VA-AHK.Integration was created with AutoHotkey v1 in mind. More specifically, the provided examples work with the syntax used in AutoHotkey v1. However there's no reason why, with some modification to the input AutoHotkey syntax, that VA-AHK.Integration wouldn't work with v2. Please be aware though that all testing for VA-AHK.Integration was done using *AutoHotkey.dll* v1 files. 

There are two ways to update the AutoHotkey version in VA-AHK.Integration:
  1. If new VA-AHK.Integration releases are available the *.vax* import process will automatically update your existing files (assuming the package includes a newer/different version of AutoHotkey compared to what you already installed).
  2. To update VA-AHK.Integration manually download the AutoHotkey_H v1 *.zip* package and take the updated *AutoHotkey.dll* files from the *Win32w* (32-bit) and *x64w* (64-bit) folders from *ahkdll* (see above link) and move them into VA-AHK.Integration's *x86* (32-bit) and *x64* (64-bit) folders, respectively. 

You can check the version of AutoHotkey employed by VA-AHK.Integration by creating an AutoHotkey thread and then running:
```C# 
ahk.ExecRaw("MsgBox, AHK version =  %A_AhkVersion%");
```
...and the *Multi-Function Example* commands included within the VoiceAttack profile fully demonstrate how to retrieve the installed AutoHotkey version. 

## Help I have issues!
First and foremost, **_[read the VoiceAttack manual](http://voiceattack.com/VoiceAttackHelp.pdf)_**. Yes it's long, but it covers most of what is needed for you to understand and use VoiceAttack. Plus it covers more advanced stuff which is great to know so you can do other super cool things. 

If you are having problems with the VoiceAttack software itself there is a [VoiceAttack User Forum](http://voiceattack.com/SMF/index.php) where you may seek help. The community is active and full of dedicated users who will help you with your VoiceAttack issues. Plus it's a great place to learn more about VoiceAttack so you can do other super cool things as well as check out profiles and commands that other users have shared. 

If you are having problems specifically with the VA-AHK.Integration commands or code then head over to the VoiceAttack User Forum, check out the *Issues* section to get an idea for how to provide enough information to request assistance, and then post to the *AutoHotkey "Integration" with VoiceAttack* thread within the *Inline Functions* section of the forum.

## Credits
 - Credit for the original AutoHotkey goes to Chris Mallet ([Source](https://autohotkey.com/foundation/history.html))
 - Credit for AutoHotkey_L (fork of AutoHotkey that is now the main branch) goes to "Lexikos" (Steve Gray) ([Source](https://github.com/Lexikos/AutoHotkey_L))
 - Additional credit for the continued support and development of the current version of AutoHotkey goes to The AutoHotkey Foundation and the AutoHotkey community ([Source](https://autohotkey.com/foundation/))
 - Credit for AutoHotkey_H (fork of AutoHotkey_L and provides the 32 and 64-bit *AutoHotkey.dll*) goes to tinku99 (inventor) and HotKeyIt (enhancements and continued development) ([Source](https://github.com/HotKeyIt/ahkdll))
 - Credit for AutoHotkey.Interop goes to "amazing-andrew" (Andrew Smith) ([Source](https://github.com/amazing-andrew/AutoHotkey.Interop))
 - Credit for the creation and continued development of VoiceAttack goes to Gary at www.voiceattack.com
 
 ## Thanks
 (pending)

## Full Disclosure
I am one of the moderators on the VoiceAttack User Forums, and I receive no benefits from the use or promotion of the VoiceAttack software. 

## Primary Links
 - [amazing-andrew's AutoHotkey.Interop Repository](https://github.com/amazing-andrew/AutoHotkey.Interop)
 - [AutoHotKey\_H Documentation & Libraries](http://hotkeyit.github.io/v2/)
 - [HotKeyIt's AutoHotkey\_H Repository](https://github.com/HotKeyIt/ahkdll) (also see [v1 release](https://github.com/HotKeyIt/ahkdll-v1-release) and [v2 release](https://github.com/HotKeyIt/ahkdll-v2-release))
 - [AutoHotkey](https://autohotkey.com/) (and [AutoHotkey_L source code](https://github.com/Lexikos/AutoHotkey_L))
 - [VoiceAttack.com](http://voiceattack.com/)
