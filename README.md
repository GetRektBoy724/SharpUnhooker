# SharpUnhooker
C# Based Universal API Unhooker - Automatically Unhook API Hives (ntdll.dll,kernel32.dll,user32.dll,and kernelbase.dll).
you might want to use the `SilentUnhooker` function instead of the `Unhooker` for stealth reasons.
This tool is inspired by [this article](https://www.ired.team/offensive-security/defense-evasion/how-to-unhook-a-dll-using-c++).This tool also included with AMSI and ETW patcher to break/disable them.

### This tool is tested on Windows 10 v20H2 

# How it works (only for non-skids)
1. It reads and copies the `.text section` of the original (in-disk) DLL using "PE parser stuff"
2. It patches the `.text section` of the loaded DLL using `Marshal.Copy` and `VirtualProtect`(to changes the permission of the memory)
3. It checks the patched in-memory DLL by reading it and compare it with the original one to see if its correctly patched.

# Note
- If you want to copy the code,Pls dont change/remove the banner

# Usage
Simply load the pre-compiled DLL or add the code function and call the main function from the SharpUnhooker class.
You can load the pre-compiled DLL on Powershell with `Reflection.Assembly` too!
This code uses C# 5,so it can be compiled with the built-in CSC from Windows 10.

![SharpUnhookerInAction](https://user-images.githubusercontent.com/41237415/121619620-1702e100-ca93-11eb-9f6d-0eb98d9a87cd.png)



# To-Do List
- Add ability to unhook EAT & IAT hooks
