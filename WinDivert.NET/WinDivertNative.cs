namespace WinDivert
{
    public class WinDivertNative
    {
        [DllImport("WinDivert.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern IntPtr WinDivertOpen(
            String filter,
            WinDivertLayer layer,
            Int16 priority,
            WinDivertOpenFlags flags
            );

        [DllImport("WinDivert.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern Boolean WinDivertClose(
            IntPtr handle
            );

        [DllImport("WinDivert.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern Boolean WinDivertRecv(
            IntPtr handle,
            IntPtr packet, // IntPtr : Byte[]
            UInt32 packet_length,
            ref UInt32 recv_length,
            ref WinDivertAddress address // IntPtr : WinDivertAddress
            );

        [DllImport("WinDivert.dll", CallingConvention = CallingConvention.Cdecl, SetLastError = true)]
        public static extern Boolean WinDivertSend(
            IntPtr handle,
            IntPtr packet,
            UInt32 packet_length,
            ref UInt32 send_length,
            ref WinDivertAddress address
            );

        public static unsafe WinDivertPacket? WinDivertHelperParsePacketEx(IntPtr packet, UInt32 length)
        {
            if (packet == IntPtr.Zero || length < sizeof(WinDivertIPHeader))
            {
                return null;
            }

            WinDivertProtocols protocol = WinDivertProtocols.HOPOPTS;
            Byte* data = (Byte*)packet;
            UInt32 total_length = 0;
            UInt32 packet_length = 0;
            UInt32 header_length = 0;
            UInt32 data_length = length;
            UInt32 frag_off = 0;
            Boolean MF = false;
            Boolean fragment = false;
            Boolean is_extended_header = false;

            WinDivertIPHeader* ip_header = (WinDivertIPHeader*)data;
            WinDivertIPv6Header* ipv6_header = (WinDivertIPv6Header*)null;
            WinDivertIPv6FragmentHeader* ipv6_fragment_header = (WinDivertIPv6FragmentHeader*)null;
            WinDivertTCPHeader* tcp_header = (WinDivertTCPHeader*)null;
            WinDivertUDPHeader* udp_header = (WinDivertUDPHeader*)null;
            WinDivertICMPHeader* icmp_header = (WinDivertICMPHeader*)null;
            WinDivertICMPv6Header* icmpv6_header = (WinDivertICMPv6Header*)null;
            switch (ip_header->Version)
            {
                case 4:
                    if (ip_header->HeaderLength < 5)
                    {
                        return null;
                    }

                    total_length = (UInt32)IPAddress.NetworkToHostOrder((Int16)ip_header->Length);
                    protocol = ip_header->Protocol;
                    header_length = (UInt32)(ip_header->HeaderLength * sizeof(UInt32));
                    if (total_length < header_length || length < header_length)
                    {
                        return null;
                    }

                    frag_off = (UInt32)IPAddress.NetworkToHostOrder((Int16)(ip_header->FragOff0 & 0xFF1F));
                    MF = (ip_header->FragOff0 & 0x0020) != 0 ? true : false;
                    fragment = MF || frag_off != 0;
                    packet_length = total_length < length ? total_length : length;
                    data += header_length;
                    data_length = packet_length - header_length;
                    break;

                case 6:
                    ipv6_header = (WinDivertIPv6Header*)data;
                    if (length < sizeof(WinDivertIPv6Header))
                    {
                        return null;
                    }

                    protocol = (WinDivertProtocols)ipv6_header->NextHeader;
                    total_length = (UInt32)(IPAddress.NetworkToHostOrder((Int16)ipv6_header->Length) + sizeof(WinDivertIPv6Header));
                    packet_length = total_length < length ? total_length : length;
                    data += sizeof(WinDivertIPv6Header);
                    data_length = (UInt32)(packet_length - sizeof(WinDivertIPv6Header));

                    while (frag_off == 0 && data_length >= 2)
                    {
                        header_length = (UInt32)data[1];
                        is_extended_header = true;
                        switch (protocol)
                        {
                            case WinDivertProtocols.FRAGMENT:
                                header_length = 8;
                                if (fragment || data_length < header_length)
                                {
                                    is_extended_header = false;
                                    break;
                                }

                                ipv6_fragment_header = (WinDivertIPv6FragmentHeader*)data;
                                frag_off = (UInt32)(IPAddress.NetworkToHostOrder((Int16)(ipv6_fragment_header->FragOff0 & 0xF8FF)));
                                MF = (ipv6_fragment_header->FragOff0 & 0x0100) != 0 ? true : false;
                                fragment = true;
                                break;

                            case WinDivertProtocols.AH:
                                header_length += 2;
                                header_length *= 4;
                                break;

                            case WinDivertProtocols.HOPOPTS:
                            case WinDivertProtocols.DSTOPTS:
                            case WinDivertProtocols.ROUTING:
                            case WinDivertProtocols.MH:
                                header_length++;
                                header_length *= 8;
                                break;

                            default:
                                is_extended_header = false;
                                break;
                        }

                        if (!is_extended_header || data_length < header_length)
                        {
                            break;
                        }
                        protocol = (WinDivertProtocols)data[0];
                        data += header_length;
                        data_length -= header_length;
                    }
                    break;

                default:
                    return null;
            }

            if (frag_off != 0)
            {
                goto WinDivertHelperParsePacketExit;
            }

            switch (protocol)
            {
                case WinDivertProtocols.TCP:
                    tcp_header = (WinDivertTCPHeader*)data;
                    if (data_length < sizeof(WinDivertTCPHeader) || tcp_header->HeaderLength < 5)
                    {
                        tcp_header = null;
                        goto WinDivertHelperParsePacketExit;
                    }

                    header_length = (UInt32)(tcp_header->HeaderLength * sizeof(UInt32));
                    header_length = (header_length > data_length ? data_length : header_length);
                    break;

                case WinDivertProtocols.UDP:
                    if (data_length < sizeof(WinDivertUDPHeader))
                    {
                        goto WinDivertHelperParsePacketExit;
                    }

                    udp_header = (WinDivertUDPHeader*)data;
                    header_length = (UInt32)sizeof(WinDivertUDPHeader);
                    break;

                case WinDivertProtocols.ICMP:
                    if (ip_header == null || data_length < sizeof(WinDivertICMPHeader))
                    {
                        goto WinDivertHelperParsePacketExit;
                    }

                    icmp_header = (WinDivertICMPHeader*)data;
                    header_length = (UInt32)sizeof(WinDivertICMPHeader);
                    break;

                case WinDivertProtocols.ICMPV6:
                    if (ipv6_header == null || data_length < sizeof(WinDivertICMPv6Header))
                    {
                        goto WinDivertHelperParsePacketExit;
                    }

                    icmpv6_header = (WinDivertICMPv6Header*)data;
                    header_length = (UInt32)sizeof(WinDivertICMPv6Header);
                    break;

                default:
                    goto WinDivertHelperParsePacketExit;
            }
            data += header_length;
            data_length -= header_length;

        WinDivertHelperParsePacketExit:
            WinDivertPacket result = new WinDivertPacket();
            data = data_length == 0 ? null : data;
            result.Protocol = protocol;
            result.Fragment = fragment ? true : false;
            result.MF = MF ? true : false;
            result.FragOff = frag_off;
            result.Truncated = total_length > length ? true : false;
            result.Extended = total_length < length ? true : false;
            result.Reserved1 = 0;
            result.IPHeader = ip_header != null ? *ip_header : null;
            result.IPv6Header = ipv6_header != null ? *ipv6_header : null;
            result.ICMPHeader = icmp_header != null ? *icmp_header : null;
            result.ICMPv6Header = icmpv6_header != null ? *icmpv6_header : null;
            result.TCPHeader = tcp_header != null ? *tcp_header : null;
            result.UDPHeader = udp_header != null ? *udp_header : null;
            result.Payload = (IntPtr)data;
            result.HeaderLength = packet_length - data_length;
            result.PayloadLength = data_length;
            return result;
        }
    }
}
