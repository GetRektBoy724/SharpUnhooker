# SharpUnhooker
C# Based NTAPI Unhooker - Automaticaly Unhook The Specified NTAPI Or Unhook Almost All NTAPIs In One.Currently it has ntdll hardcoded, but there is no reason why you couldn't use it to unhook any other API. Make sure you use the full path for the dll.
Use it as many times as needed in your code to unhook all the APIs you want/need to for whatever reason.
There are some debugging `Console.WriteLine` you might want to remove for stealth reasons, but I left them for your testing.
This tool is based off [this article](https://ired.team/offensive-security/defense-evasion/bypassing-cylance-and-other-avs-edrs-by-unhooking-windows-apis) by @spotheplanet.

# Notes
- If you want to copy the code,Pls dont change/remove the banner
- My Sublime kinda fucked up the Indentation so dont roast me,but its still works tho
- Dont be a script kiddie,lmao 😂
- It have NTAPI list hardcoded (which makes the file large,sorry)
- If you have the code to read all the available API in the NTDLL,pls contact me add Discord : DeadSec#4077

# Usage
Simply load the pre-compiled DLL or add the code function and call the function with 
```
[SharpUnhooker]::Main();
```
