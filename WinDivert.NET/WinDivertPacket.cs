namespace WinDivert
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WinDivertPacket
    {
        private UInt64 Interface;
        public UInt32 HeaderLength // 17 bits
        {
            set => Interface = (Interface & 0xFFFFFFFFFFFE0000) | (Convert.ToUInt64(value) & 0x1FFFF);
            get => Convert.ToUInt32(Interface & 0x1FFFF); 
        } 
        public UInt32 FragOff // 13 bits
        { 
            set => Interface = (Interface & 0xFFFFFFFFC001FFFF) | ((Convert.ToUInt64(value) & 0x1FFF) << 17);
            get => Convert.ToUInt32((Interface & 0x3FFE0000) >> 17); 
        } 
        public Boolean Fragment // 1 bit
        { 
            set => Interface = (Interface & 0xFFFFFFFFDFFFFFFF) | (Convert.ToUInt64(value) << 30);
            get => Convert.ToBoolean(Interface & 0x40000000); 
        } 
        public Boolean MF // 1 bit
        { 
            set => Interface = (Interface & 0xFFFFFFFFBFFFFFFF) | (Convert.ToUInt64(value) << 31);
            get => Convert.ToBoolean(Interface & 0x80000000); 
        } 
        public UInt32 PayloadLength // 16 bits
        { 
            set => Interface = (Interface & 0xFFFF0000FFFFFFFF) | ((Convert.ToUInt64(value) & 0xFFFF) << 32);
            get => Convert.ToUInt32((Interface & 0xFFFF00000000) >> 32); 
        } 
        public WinDivertProtocols Protocol // 8 bits
        {
            set => Interface = (Interface & 0xFF00FFFFFFFFFFFF) | ((Convert.ToUInt64(value) & 0xFFFF) << 48);
            get => (WinDivertProtocols)Convert.ToByte((Interface & 0xFF000000000000) >> 48); 
        } 
        public Boolean Truncated // 1 bit
        { 
            set => Interface = (Interface & 0xFEFFFFFFFFFFFFFF) | (Convert.ToUInt64(value) << 56);
            get => Convert.ToBoolean(Interface & 0x100000000000000); 
        } 
        public Boolean Extended // 1 bit
        {
            set => Interface = (Interface & 0xFDFFFFFFFFFFFFFF) | (Convert.ToUInt64(value) << 57);
            get => Convert.ToBoolean(Interface & 0x200000000000000); 
        } 
        public UInt32 Reserved1 // 6 bits
        {
            set => Interface = (Interface & 0x3FFFFFFFFFFFFFF) | ((Convert.ToUInt64(value) & 0x3F) << 58);
            get => Convert.ToUInt32((Interface & 0xFC00000000000000) >> 58); 
        } 

        public WinDivertIPHeader? IPHeader;
        public WinDivertIPv6Header? IPv6Header;
        public WinDivertICMPHeader? ICMPHeader;
        public WinDivertICMPv6Header? ICMPv6Header;
        public WinDivertTCPHeader? TCPHeader;
        public WinDivertUDPHeader? UDPHeader;
        public IntPtr Payload;
    }
}
