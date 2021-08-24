namespace WinDivert
{
    [StructLayout(LayoutKind.Explicit)]
    public struct WinDivertAddress
    {
        [FieldOffset(0)]
        public Int64 Timestamp;

        [FieldOffset(8)]
        private UInt32 Interface;
        public UInt32 Layer // 8 bits
        {
            set => Interface = (Interface & 0xFFFFFF00) | (Convert.ToUInt32(value) & 0xFF);
            get => Convert.ToUInt32(Interface & 0xFF);
        } 
        public UInt32 Event // 8 bits
        {
            set => Interface = (Interface & 0xFFFF00FF) | ((Convert.ToUInt32(value) & 0xFF) << 8);
            get => Convert.ToUInt32((Interface & 0xFF00) >> 8); 
        } 
        public Boolean Sniffed // 1 bit
        {
            set => Interface = (Interface & 0xFFFEFFFF) | (Convert.ToUInt32(value) << 16);
            get => Convert.ToBoolean(Interface & 0x10000); 
        } 
        public Boolean Outbound // 1 bit
        {
            set => Interface = (Interface & 0xFFFDFFFF) | (Convert.ToUInt32(value) << 17);
            get => Convert.ToBoolean(Interface & 0x20000); 
        } 
        public Boolean Loopback // 1 bit
        {
            set => Interface = (Interface & 0xFFFBFFFF) | (Convert.ToUInt32(value) << 18);
            get => Convert.ToBoolean(Interface & 0x40000); 
        } 
        public Boolean Impostor // 1 bit
        {
            set => Interface = (Interface & 0xFFF7FFFF) | (Convert.ToUInt32(value) << 19);
            get => Convert.ToBoolean(Interface & 0x80000);
        } 
        public Boolean IPv6 // 1 bit
        {
            set => Interface = (Interface & 0xFFEFFFFF) | (Convert.ToUInt32(value) << 20);
            get => Convert.ToBoolean(Interface & 0x100000); 
        } 
        public Boolean IPChecksum // 1 bit
        {
            set => Interface = (Interface & 0xFFDFFFFF) | (Convert.ToUInt32(value) << 21);
            get => Convert.ToBoolean(Interface & 0x200000); 
        } 
        public Boolean TCPChecksum // 1 bit
        {
            set => Interface = (Interface & 0xFFBFFFFF) | (Convert.ToUInt32(value) << 22);
            get => Convert.ToBoolean(Interface & 0x400000); 
        }
        public Boolean UDPChecksum // 1 bit
        {
            set => Interface = (Interface & 0xFF7FFFFF) | (Convert.ToUInt32(value) << 23);
            get => Convert.ToBoolean(Interface & 0x800000); 
        } 
        public UInt32 Reserved1 // 8 bits
        {
            set => Interface = (Interface & 0x00FFFFFF) | ((Convert.ToUInt32(value) & 0xFF) << 24);
            get => Convert.ToUInt32((Interface & 0xFF000000) >> 24); 
        } 

        [FieldOffset(12)]
        public UInt32 Reserved2;

        [FieldOffset(16)]
        public unsafe fixed Byte Reserved3[64];

        [FieldOffset(16)]
        public WinDivertNetworkData Network;

        [FieldOffset(16)]
        public WinDivertFlowData Flow;

        [FieldOffset(16)]
        public WinDivertSocketData Socket;

        [FieldOffset(16)]
        public WinDivertReflectData Reflect;
    }
}
