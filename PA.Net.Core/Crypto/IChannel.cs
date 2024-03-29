﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using PA.Comperssion.Core;
using PA.Net.Clients;
using PA.Net.Core;
using System;
using System.IO;
namespace PA.Crypto
{
    public interface IChannel
    {
        Stream BaseStream { get; }
        BufferManager<byte[]> SendBuffer { get; set; }
        INetClient Client { get; }
        bool Closed { get; }
        void Read();
        void Write();
        void Start();
        void Stop();
        bool IsEndPoint { get;}
        object Eccrypt(object data);
        object Decrypt(object data);

        //protected ReportCollection Log = new ReportCollection();
        event DataReceiveHandler DataReceived;
        event EventHandler SendingSuceeded;
        event ExceptionReportHandler SendingFailed;
        event ExceptionReportHandler ReceivingFailed;
        event EventHandler ReadStarted;
        event EventHandler ReadFinished;
        event EventHandler WriteStarted;
        event EventHandler WriteFinished;

    }
}


