# SharpUnhooker
C# Based Universal API Unhooker - Automatically Unhook API Hives (ntdll.dll,kernel32.dll,user32.dll,and kernelbase.dll).
you might want to use the `SilentUnhooker` function instead of the `Unhooker` for stealth reasons.
This tool is based off [this tool](https://github.com/optiv/ScareCrow/).
```
When executed, ScareCrow will copy the bytes of the system DLLs stored on disk in C:\Windows\System32\. 
These DLLs are stored on disk “clean” of EDR hooks because they are used by the system to load an unaltered copy into a new process when it’s spawned. 
Since EDR’s only hook these processes in memory, they remain unaltered. ScareCrow does not copy the entire DLL file, instead only focuses on the .text section of the DLLs. 
This section of a DLL contains the executable assembly, and by doing this ScareCrow helps reduce the likelihood of detection as re-reading entire files can cause an EDR to detect that there is a modification to a system resource. 
The data is then copied into the right region of memory by using each function’s offset. Each function has an offset which denotes the exact number of bytes from the base address where they reside, providing the function’s location on the stack. 
In order to do this, ScareCrow changes the permissions of the .text region of memory using VirtualProtect. 
Even though this is a system DLL, since it has been loaded into our process (that we control), we can change the memory permissions without requiring elevated privileges.
```

# Notes
- If you want to copy the code,Pls dont change/remove the banner
- My Sublime kinda fucked up the Indentation so dont roast me,but its still works tho

# Usage
Simply load the pre-compiled DLL or add the code function and call the function with 
```
[SharpUnhooker]::Main();
```

# To-Do List
- Implement D\Invoke
- Add ability to unhook IAT hooks
