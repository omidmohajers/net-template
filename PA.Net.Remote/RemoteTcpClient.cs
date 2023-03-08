﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using PA.Crypto;
using PA.Net.Core;
using PA.StockMarket.Data;
using PA.StockMarket.Data.DataAccess;
using System;
using System.Net;
using System.Net.Sockets;

namespace PA.Net.Clients
{

    public class RemoteTcpClient : TcpNetClient
    {
        public event OnStateReceive StatusChanged = null;
        public event OnChackExists CheckingClientExists = null;
        public event OnMessageReceive TextMessageReceived = null;
        public event OnStartBoardcastVideo BroadcastingVideoStarted = null;
        public event OnRequestClientList ClientListRequested = null;
        public event OnSayHello SayingHello = null;
        public override IPAddress IP
        {
            get
            {
                if (Connected)
                    return ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                else
                    return IPAddress.None;
            }
        }
        public override int Port
        {
            get
            {
                if (Connected)
                    return ((IPEndPoint)client.Client.RemoteEndPoint).Port;
                else
                    return -1;
            }
        }
        public RemoteTcpClient()
        {
            
        }
        public RemoteTcpClient(TcpClient client, int id, CryptoTypes securityType)
        {
            ID = id;
            Client = client;
            CreateChannel(securityType);
            // ---------------------------------- Channel Events-----------------------------------
            channel.DataReceived += Channel_DataReceived;
            // ------------------------------------------------------------------------------------
            Start();
        }

        private void Channel_DataReceived(object sender, INetClient client, byte[] data)
        {
            Package pak = Package.FromByteArray(data);
            RemoteTcpClient cln = client as RemoteTcpClient;
            pak.SenderPort = client.Port;
            switch (pak.CommandType)
            {
                case Net.Core.CommandType.BroadcastState:
                    UpdateStatus(pak);

                    if (StatusChanged != null)
                        StatusChanged(this, cln, Status);
                    break;
                case Net.Core.CommandType.IsNameExists:
                    BeginCkeckExists();
                    break;
                case Net.Core.CommandType.Message:
                    if (TextMessageReceived != null)
                        TextMessageReceived(this, cln, pak);
                    break;
                case CommandType.StartBroadcastingVideo:
                    if (BroadcastingVideoStarted != null)
                        BroadcastingVideoStarted(this, cln, pak);
                    break;
                case Net.Core.CommandType.SendClientList:
                    BeginRequestingClientList();
                    break;
                case Net.Core.CommandType.Hello:
                    if (SayingHello != null)
                        SayingHello(this, cln);
                    break;
                case Net.Core.CommandType.Goodbye:
                    bool done = Disconnect();
                    break;
                    //case CommandType.FileOrder:
                    //    Log.Add("Upload File Request : " + pak.SenderIP + " : " + pak.SenderPort);
                    //    RoomContentModel content = CreateFileUploadOrder(pak);
                    //    string[] fileInfo = Encoding.UTF8.GetString(pak.Data).Split(':');
                    //    FileTransferController ftc = cln.SendFileOrder(content.ID, fileInfo[0], long.Parse(fileInfo[1]), ServerIP);
                    //    fileConn.AddFile(ftc);
                    //    break;
            }
        }

        private void BeginRequestingClientList()
        {
            if (ClientListRequested != null)
                ClientListRequested(this, this);
        }

        public void EndRequestingClientList(string clients, IPAddress serverIP)
        {
            SendClientList(clients, serverIP);
        }

        private void BeginCkeckExists()
        {
            if (CheckingClientExists != null)
                CheckingClientExists(this, this);
        }

        public void EndCheckExists(bool exists, IPAddress serverIP)
        {
            this.SendExistanceCommand(exists, serverIP);
        }

        private void UpdateStatus(Package pak)
        {
            UserID = pak.UserID;
            Status = ContactStatus.FromBytes(pak.Data);
            if (UserProfile == null)
            {
                Account user = AccountDataProvider.Get(pak.UserID);
                if (user == null)
                    return;
                UserProfile = user;
            }
          //  UserProfile.StatusID = (int)Status.Type;
          //  UserProfile.StatusDescription = Status.Description;
        }

        public Account UserProfile { get; set; }
        public ContactStatus Status { get; private set; }

        public void SendExistanceCommand(bool isExixsts, IPAddress serverIP)
        {
            Package pak = new Package(CommandType.IsNameExists, this.IP, serverIP, isExixsts.ToString(), null);
            pak.SenderIP = serverIP;
            Send(Package.ToByteArray(pak));
        }
        public void SendClientList(string val, IPAddress serverIP)
        {
            Package pak = new Package(CommandType.SendClientList, this.IP, serverIP, val, null);
            pak.SenderIP = serverIP;
            Send(Package.ToByteArray(pak));
        }
        public FileTransferController SendFileOrder(long fileID, string fileName, long fileSize, IPAddress serverIP)
        {
            FileTransferController ftp = new FileTransferController();
            ftp.FileID = fileID;
            ftp.FileSize = fileSize;
            ftp.FileName = fileName;
            ftp.TransferSide = TransferSide.UploadToServer;
            ftp.Data = null;
            ftp.Start = 0;
            ftp.End = 0;


            Package pak = new Package(CommandType.FileControl, this.IP, serverIP, "", null);
            pak.Data = FileTransferController.ToByteArray(ftp);
            pak.SenderIP = serverIP;
            Send(Package.ToByteArray(pak));

            return ftp;
        }

        public override bool Disconnect()
        {
            if (Client != null && Client.Connected)
            {
                try
                {
                    Client.Client.Disconnect(true);
                    if (Client.Connected)
                    {
                        Client.GetStream().Close();
                        Client.Close();
                    }
                    RaiseDisconnectSuceeded();
                    return true;
                }
                catch (Exception ex)
                {
                    Log.Add(ReportType.Error, ClientName, string.Format("خطا در زمان قطع اتصال از سرور:{0}", ex.Message), ex, DateTime.Now);

                    RaiseDisconnectFaild();
                    return false;
                }
            }
            else
            {
                RaiseDisconnectSuceeded();
                return true;
            }
        }
    }
}

