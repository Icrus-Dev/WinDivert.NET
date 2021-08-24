namespace WinDivert
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WinDivertIPv6Header
    {
        private UInt16 Interface;
        public Byte TrafficClass0 // 4 bits
        {
            set => Interface = (UInt16)((Interface & 0xFFF0) | (Convert.ToUInt16(value) & 0xF));
            get => Convert.ToByte(Interface & 0xF); 
        } 
        public Byte Version // 4 bits
        {
            set => Interface = (UInt16)((Interface & 0xFF0F) | ((Convert.ToUInt16(value) & 0xF) << 4));
            get => Convert.ToByte((Interface & 0xF0) >> 4); 
        } 
        public Byte FlowLabel0 // 4 bits
        {
            set => Interface = (UInt16)((Interface & 0xF0FF) | ((Convert.ToUInt16(value) & 0xF) << 8));
            get => Convert.ToByte((Interface & 0xF00) >> 8); 
        } 
        public Byte TrafficClass1 // 4 bits
        {
            set => Interface = (UInt16)((Interface & 0x0FFF) | ((Convert.ToUInt16(value) & 0xF) << 12));
            get => Convert.ToByte((Interface & 0xF000) >> 12); 
        } 

        public UInt16 FlowLabel1;
        public UInt16 Length;
        public Byte NextHeader;
        public Byte HopLimit;
        public unsafe fixed UInt32 SourceAddress[4];
        public unsafe fixed UInt32 DestinationAddress[4];
    }
}
