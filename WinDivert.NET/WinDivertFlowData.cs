namespace WinDivert
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WinDivertFlowData
    {
        public UInt64 EndpointId;                  /* Endpoint ID. */
        public UInt64 ParentEndpointId;            /* Parent endpoint ID. */
        public UInt32 ProcessId;                   /* Process ID. */
        public unsafe fixed UInt32 LocalAddr[4];                /* Local address. */
        public unsafe fixed UInt32 RemoteAddr[4];               /* Remote address. */
        public UInt16 LocalPort;                   /* Local port. */
        public UInt16 RemotePort;                  /* Remote port. */
        public WinDivertProtocols Protocol;                    /* Protocol. */
    }
}
