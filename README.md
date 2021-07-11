# SharpUnhooker
C# Based Universal API Unhooker - Automatically Unhook API Hives (ntdll.dll,kernel32.dll,user32.dll,and kernelbase.dll).
I prefer you to use the Main function but if you want to only unhook single API DLL,you can use the `SilentUnhooker` and `Unhooker` function.You might want to use the `SilentUnhooker` function instead of the `Unhooker` for stealth reasons.
This tool is inspired by [this article](https://www.ired.team/offensive-security/defense-evasion/how-to-unhook-a-dll-using-c++).This tool also included with AMSI and ETW patcher to break/disable them.

### This tool is tested on Windows 10 v20H2 x64

# How it works (only for non-skids)
1. It reads and copies the `.text section` of the original (in-disk) DLL using "PE parser stuff"
2. It patches the `.text section` of the loaded DLL using `Marshal.Copy` and `NtProtectVirtualMemory` from D/Invoke (to changes the permission of the memory)
3. It checks the patched in-memory DLL by reading it again and compare it with the original one to see if its correctly patched.

# Note
- If you want to copy the code,Pls dont change/remove the banner
- If you want to see a good demonstration of SharpUnhooker,go check this [blog post](https://roberreigada.github.io/posts/playing_with_an_edr/) by [Reigada](https://github.com/roberreigada)
- Github dont like my Sublime indentation settings so dont roast me pls

# Usage
Simply load the pre-compiled DLL or add the code function and call the main function from the SharpUnhooker class.
You can load the pre-compiled DLL on Powershell with `Reflection.Assembly` too!
This code uses C# 5,so it can be compiled with the built-in CSC from Windows 10.

### SharpUnhooker's Main function in action!
![SharpUnhookerInAction](https://user-images.githubusercontent.com/41237415/125193982-76facc00-e279-11eb-8af0-357c784f2a69.png)


# To-Do List
- Add ability to unhook EAT & IAT hooks
