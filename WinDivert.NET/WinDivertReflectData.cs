namespace WinDivert
{
    [StructLayout(LayoutKind.Sequential)]
    public struct WinDivertReflectData
    {
        public Int64 Timestamp;                   /* Handle open time. */
        public UInt32 ProcessId;                   /* Handle process ID. */
        public WinDivertLayer Layer;              /* Handle layer. */
        public UInt64 Flags;                       /* Handle flags. */
        public Int16 Priority;                    /* Handle priority. */
    }
}
