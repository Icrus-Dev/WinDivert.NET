namespace WinDivert
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WinDivertIPv6FragmentHeader
    {
        public Byte NextHeader;
        public Byte Reserved;
        public UInt16 FragOff0;
        public UInt32 Id;
    }
}
