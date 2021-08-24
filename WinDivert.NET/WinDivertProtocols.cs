namespace WinDivert
{
    public enum WinDivertProtocols : Byte
    {
        HOPOPTS = 0,
        ICMP = 1,
        TCP = 6,
        UDP = 17,
        ROUTING = 43,
        FRAGMENT = 44,
        AH = 51,
        ICMPV6 = 58,
        NONE = 59,
        DSTOPTS = 60,
        MH = 135,
    }
}
