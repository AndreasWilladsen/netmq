using System;
using System.Net.Sockets;
using System.Runtime.InteropServices;

namespace NetMQ.Core.Utils
{
    public class TcpKeepAliveHelper
    {
        public static readonly SocketOptionName[] keepAliveOptionNames;
        public static readonly Func<Socket, SocketOptionName, int> GetSocketOption;
        public static readonly Action<Socket, SocketOptionName, int> SetSocketOption;
       
       static TcpKeepAliveHelper()
        {
            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {
                keepAliveOptionNames = new [] { (SocketOptionName)16, (SocketOptionName)3, (SocketOptionName)17 };
                SetSocketOption = (socket, keepAliveOptionName, keepAliveOptionValue) =>
                    socket.SetSocketOption(SocketOptionLevel.Tcp, keepAliveOptionName, keepAliveOptionValue);
                GetSocketOption = (socket, keepAliveOptionName) =>
                    (int)socket.GetSocketOption(SocketOptionLevel.Tcp, keepAliveOptionName);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {
                keepAliveOptionNames = new [] { (SocketOptionName)0x6, (SocketOptionName)0x4, (SocketOptionName)0x5 };
                SetSocketOption = (socket, keepAliveOptionName, keepAliveOptionValue) =>
                    Interop.SetSockOptSysCall(socket, keepAliveOptionName, keepAliveOptionValue);
                GetSocketOption = (socket, keepAliveOptionName) =>
                    Interop.GetSockOptSysCall(socket, keepAliveOptionName);
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
            {
                keepAliveOptionNames = new [] { (SocketOptionName)0x102, (SocketOptionName)0x10, (SocketOptionName)0x101 };
                SetSocketOption = (socket, keepAliveOptionName, keepAliveOptionValue) =>
                    Interop.SetSockOptSysCall(socket, keepAliveOptionName, keepAliveOptionValue);
                GetSocketOption = (socket, keepAliveOptionName) =>
                    Interop.GetSockOptSysCall(socket, keepAliveOptionName);
            }
        }
    }
}