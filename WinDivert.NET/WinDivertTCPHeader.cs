namespace WinDivert
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WinDivertTCPHeader
    {
        public UInt16 SourcePort;
        public UInt16 DestinationPort;
        public UInt32 SequenceNumber;
        public UInt32 ACKNumber;
        private UInt16 Interface;
        public UInt16 Reserved1 // 4 bits
        {
            set => Interface = (UInt16)((Interface & 0xFFF0) | (Convert.ToUInt16(value) & 0xF));
            get => Convert.ToUInt16(Interface & 0xF); 
        } 
        public UInt16 HeaderLength // 4 bits
        {
            set => Interface = (UInt16)((Interface & 0xFF0F) | ((Convert.ToUInt16(value) & 0xF) << 4));
            get => Convert.ToUInt16((Interface & 0xF0) >> 4); 
        } 
        public Boolean FIN // 1 bit
        {
            set => Interface = (UInt16)((Interface & 0xFEFF) | (Convert.ToUInt16(value) << 8));
            get => Convert.ToBoolean(Interface & 0x100); 
        } 
        public Boolean SYN // 1 bit
        {
            set => Interface = (UInt16)((Interface & 0xFDFF) | (Convert.ToUInt16(value) << 9));
            get => Convert.ToBoolean(Interface & 0x200); 
        } 
        public Boolean RST // 1 bit
        {
            set => Interface = (UInt16)((Interface & 0xFBFF) | (Convert.ToUInt16(value) << 10));
            get => Convert.ToBoolean(Interface & 0x400); 
        } 
        public Boolean PSH // 1 bit
        {
            set => Interface = (UInt16)((Interface & 0xF7FF) | (Convert.ToUInt16(value) << 11));
            get => Convert.ToBoolean(Interface & 0x800); 
        } 
        public Boolean ACK // 1 bit
        {
            set => Interface = (UInt16)((Interface & 0xEFFF) | (Convert.ToUInt16(value) << 12));
            get => Convert.ToBoolean(Interface & 0x1000); 
        } 
        public Boolean URG // 1 bit
        {
            set => Interface = (UInt16)((Interface & 0xDFFF) | (Convert.ToUInt16(value) << 13));
            get => Convert.ToBoolean(Interface & 0x2000); 
        } 
        public UInt16 Reserved2 // 2 bits
        {
            set => Interface = (UInt16)((Interface & 0x3FFF) | ((Convert.ToUInt16(value) & 0x3) << 14));
            get => Convert.ToUInt16((Interface & 0xC000) >> 14); 
        } 

        public UInt16 Window;
        public UInt16 Checksum;
        public UInt16 URGPointer;
    }
}
