# SharpUnhooker
C# Based Universal API Unhooker - Automatically Unhook API Hives (ntdll.dll, kernel32.dll, advapi32.dll, and kernelbase.dll). SharpUnhooker helps you to evades user-land monitoring done by AVs and/or EDRs by cleansing/refreshing API DLLs that loaded on the process. There is 3 technique of user-land API hooking that i know, Inline/hot-patch hooking, IAT hooking, and EAT hooking. For now, SharpUnhooker only unhooks inline/hot-patch hooks and EAT hooks. The `SilentUnhooker`/`Unhooker` function unhooks inline/hot-patch hooks from DLLs, and the `EATCleansing` function unhooks EAT hooks.
This tool is inspired by [this article](https://www.ired.team/offensive-security/defense-evasion/how-to-unhook-a-dll-using-c++).This tool also included with AMSI and ETW patcher to break/disable them.

### This tool is tested on Windows 10 v20H2 x64

# Note
- If you want to copy the code,Pls dont change/remove the banner,or atleast dont forget to credit me
- If you want to see a good demonstration of SharpUnhooker,go check this [blog post](https://roberreigada.github.io/posts/playing_with_an_edr/) by [Reigada](https://github.com/roberreigada)
- Github dont like my Sublime indentation settings so dont roast me pls

# Usage
Simply load the pre-compiled DLL or add the code function and call the Main function from the SharpUnhooker class.
You can load the pre-compiled DLL on Powershell with `Reflection.Assembly` too!
This code uses C# 5,so it can be compiled with the built-in CSC from Windows 10.
If you want to only unhook single API DLL, you can call the `SilentUnhooker`/`Unhooker` and the `EATCleansing` function.You might want to use the `SilentUnhooker` function instead of the `Unhooker` for stealth reasons. You can use the `EATCleansing` function cleanse the export address table of an API DLL.
If you want to test the capability of SharpUnhooker, you can use the `UsageExample` function from `SUUsageExample` class. Its just a simple local shellcode injector.

### SharpUnhooker's Main function in action!
![SharpUnhookerInAction](https://user-images.githubusercontent.com/41237415/132009214-0c145b58-830f-45b2-b441-8534ede56541.png)


# To-Do List
- Add ability to unhook IAT hooks
