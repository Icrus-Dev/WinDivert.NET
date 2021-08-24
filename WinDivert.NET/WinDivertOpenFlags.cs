namespace WinDivert
{
    public enum WinDivertOpenFlags : UInt64
    {
        Default = 0x00,
        Sniff = 0x01,
        Drop = 0x02,
        RecvOnly = 0x04,
        ReadOnly = RecvOnly,
        SendOnly = 0x08,
        WriteOnly = SendOnly,
        NoInstall = 0x10,
        Fragments = 0x20,
    }
}
