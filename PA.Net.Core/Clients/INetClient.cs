﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using PA.Crypto;
using PA.Net.Core;
using PA.Net.Core.Clients;
using System;
using System.Net;
using System.Net.Sockets;

namespace PA.Net.Clients
{
    public interface INetClient
    {
        int ID { get; set; }
        int Port { get; }
        IPAddress IP { get; }
        bool Connected { get; }
        long UserID { get; set; }
        string ClientName { get; set; }
        TcpClient Client { get; set; }
        IChannel Channel { get; }
        CryptoTypes ChannelType { get; }
        ClientState State { get; }
        bool Connect(IPAddress serverIP, int port,CryptoTypes channelType);
        bool Disconnect();
        void Init();
        void Start();
        void Stop();
        bool Send(byte[] pocket);
        event EventHandler DisconnectSuceeded;
        event EventHandler DisconnectFaild;
        event EventHandler ConnectionSuceeded;
        event EventHandler ConnectionFailed;
        event ExceptionReportHandler ExceptionReported;
    }
}


