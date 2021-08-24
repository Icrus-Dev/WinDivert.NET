namespace WinDivert
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WinDivertICMPHeader
    {
        public Byte Type;
        public Byte Code;
        public UInt16 Checksum;
        public UInt32 Body;
    }
}
