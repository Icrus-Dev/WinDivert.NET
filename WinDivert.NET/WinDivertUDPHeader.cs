namespace WinDivert
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WinDivertUDPHeader
    {
        UInt16 SourcePort;
        UInt16 DestinationPort;
        UInt16 Length;
        UInt16 Checksum;
    }
}
