namespace WinDivert
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WinDivertIPHeader
    {
        private Byte Interface;
        public Byte HeaderLength // 4 bits
        {
            set => Interface = (Byte)((Interface & 0xF0) | (value & 0xF));
            get => Convert.ToByte(Interface & 0xF); 
        } 
        public Byte Version // 4 bits
        {
            set => Interface = (Byte)((Interface & 0xF) | ((value & 0xF) << 4));
            get => Convert.ToByte((Interface & 0xF0) >> 4); 
        } 

        public Byte TOS;
        public UInt16 Length;
        public UInt16 Id;
        public UInt16 FragOff0;
        public Byte TTL;
        public WinDivertProtocols Protocol;
        public UInt16 Checksum;
        public UInt32 SourceAddress;
        public UInt32 DestinationAddress;
    }
}
