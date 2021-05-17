using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

public class SharpUnhooker {
	// Import required Windows APIs
	[DllImport("kernel32.dll")]
    public static extern IntPtr LoadLibrary(string lpLibFileName);
    [DllImport("kernel32.dll", CharSet=CharSet.Ansi, ExactSpelling=true)]
    public static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
    [DllImport("kernel32.dll")]
    public static extern bool FreeLibrary(IntPtr hModule);
    [DllImport("kernel32.dll")]
	public static extern bool WriteProcessMemory(IntPtr hProcess, IntPtr lpBaseAddress, byte[] lpBuffer, Int32 nSize, out IntPtr lpNumberOfBytesWritten);
	[DllImport("kernel32.dll")]
	public static extern IntPtr GetModuleHandle(string lpModuleName);

    public static void UnhookNT(string IWantToUnhookThisAPI, bool RecheckAfterPatch) {
    	Console.WriteLine("----------------------------------------");
    	IntPtr originallib = LoadLibrary("C:/Windows/System32/ntdll.dll");
    	byte[] assemblyBytes = new byte[5];
    	if (originallib != IntPtr.Zero) {
    		Console.WriteLine("Yay!Got original ntdll handle.");
	    	IntPtr readOriginalDLL = GetProcAddress(originallib, IWantToUnhookThisAPI);
	    	if (readOriginalDLL != IntPtr.Zero) {
	    		Console.WriteLine(String.Format("Yay!got address for {0}", IWantToUnhookThisAPI));
	    		Console.WriteLine("Reading Original API...");
    			Marshal.Copy(readOriginalDLL, assemblyBytes, 0, 5);
	    		if (assemblyBytes != null && assemblyBytes.Length > 0) {
					Console.WriteLine("Yay!Original API Readed.");
					Console.WriteLine(BitConverter.ToString(assemblyBytes));
					Console.WriteLine("Applying patch...");
					IntPtr WPMOutput;
					bool patchdll = WriteProcessMemory(Process.GetCurrentProcess().Handle, GetProcAddress(GetModuleHandle("ntdll"), IWantToUnhookThisAPI), assemblyBytes, 5, out WPMOutput);
					if (patchdll) {
						Console.WriteLine("Yay!Patch applied!");
						FreeLibrary(originallib);
						if (RecheckAfterPatch) {
							Console.WriteLine("Rechecking Loaded API After Patching...");
							byte[] assemblyBytesAfterPatched = new byte[5];
							IntPtr readPatchedAPI = GetProcAddress(GetModuleHandle("ntdll"), IWantToUnhookThisAPI);
							Marshal.Copy(readPatchedAPI, assemblyBytesAfterPatched, 0, 5);
							Console.WriteLine(BitConverter.ToString(assemblyBytesAfterPatched));
							bool checkAssemblyBytesAfterPatched = assemblyBytesAfterPatched.SequenceEqual(assemblyBytes);
							if (!checkAssemblyBytesAfterPatched) {
								Console.WriteLine("Patched API Bytes Doesnt Match With Desired API Bytes! API Is Probably Still Hooked.");
							}else {
								Console.WriteLine("Chill Out,Everything Is Fine.Which Means API Is Unhooked!");
							}
						}
					}else {
						Console.WriteLine("Patch unsucessful!");
						FreeLibrary(originallib);
					}	
	    		}else {
	    			Console.WriteLine("Failed to read API.");
	    		}
    		}else {
    			Console.WriteLine("API doesnt exist.");
    		}
    	}else {
    		Console.WriteLine("WADAFOK NTDLL IS NOT IN THE RIGHT LOCATION!");
    	}
    }
    public static void Main() {
    	Console.WriteLine("----------------------------------------");
    	Console.WriteLine("SharpUnhookerV1 - C# Based API Unhooker.");
    	Console.WriteLine("        Written By GetRektBoy724        ");
    	UnhookNT("NtReadVirtualMemory", true);
    }
}