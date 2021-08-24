namespace WinDivert
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WinDivertUDPHeader
    {
        public UInt16 SourcePort;
        public UInt16 DestinationPort;
        public UInt16 Length;
        public UInt16 Checksum;
    }
}
