namespace WinDivert
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WinDivertNetworkData
    {
        public UInt32 InterfaceIndex;                       /* Packet's interface index. */
        public UInt32 SubInterfaceIndex;                    /* Packet's sub-interface index. */
    }
}
