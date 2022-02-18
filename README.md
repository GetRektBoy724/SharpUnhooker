# SharpUnhooker
C# Based Universal API Unhooker - Automatically Unhook API Hives (ntdll.dll, kernel32.dll, advapi32.dll, and kernelbase.dll). SharpUnhooker helps you to evades user-land monitoring done by AVs and/or EDRs by cleansing/refreshing API DLLs that loaded on the process (Offensive Side) or remove API hooks from user-land rootkit (Defensive Side). SharpUnhooker can unhook EAT hooks, IAT hooks, and JMP/Hot-patch/Inline hooks.
SharpUnhooker also equipped with AMSI and ETW patcher to break/disable them.

### This tool is tested on Windows 10 v21H2 x64

# Note
- If you want to copy the code, Please dont forget to credit me.
- If you want to see a good demonstration of SharpUnhooker, go check this [blog post](https://roberreigada.github.io/posts/playing_with_an_edr/) by [Reigada](https://github.com/roberreigada).
- Github doesn't like my Sublime Text indentation settings, so if you see some "weirdness" on the indentation, Im sorry.

# Usage
Simply load the pre-compiled DLL or add the code function and call the Main function from the SharpUnhooker class.
You can load the pre-compiled DLL on Powershell with `Reflection.Assembly` too!
This code uses C# 5,so it can be compiled with the built-in CSC from Windows 10.
The `JMPUnhooker` function is used to unhook JMP/Hot-patch/Inline hooks, the `EATUnhooker` function is used to unhook EAT hooks, and the `IATUnhooker` function is used to unhook IAT hooks. For now, the `IATUnhooker` and `EATUnhooker` cant unhook function import/export that uses ordinals instead of name. The `Main` function is basically "wrapper" of all of the functionality of SharpUnhooker.
If you want to test the capability of SharpUnhooker, you can use the `UsageExample` function from `SUUsageExample` class. Its just a simple local shellcode injector.

### SharpUnhooker's Main function in action!
![SharpUnhookerInAction](https://user-images.githubusercontent.com/41237415/154688547-253dc73e-f2f1-46be-a376-ebd81b14e31b.png)


# To-Do List
- Add function ordinals ability on EATUnhooker and IATUnhooker
- Add ability to remove hooks from [CLRGuard](https://github.com/endgameinc/ClrGuard/)
