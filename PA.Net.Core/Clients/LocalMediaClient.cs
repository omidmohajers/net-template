﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool
//     Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
using PA.Comperssion.Core;
using System.Collections.Generic;
using System.Linq;
using PA.Net.Core;
using System.Net;
using PA.Codec;
using Alvas.Audio;

namespace PA.Net.Clients
{
    public class LocalVideoClient : LocalTcpClient
    {
        public virtual long RoomID
        {
            get;
            set;
        }
        public List<Box<long, long>> ConfirmedUsers
        {
            get;
            private set;
        }
        public MediaConfirmationTypes MediaConfirmationType { get; set; }
        public VideoCodecTypes VideoCodecType { get; private set; }

        public LocalVideoClient(long userID,long roomID,VideoCodecTypes videoType) : base(userID)
        {
            RoomID = roomID;
            VideoCodecType = videoType;
            //-----------------------------------------------------------------
            ConfirmedUsers = new List<Box<long, long>>();
            //PackageReceived += LocalMediaClient_PackageReceived;
            //receiveBuffer.DataAdded += ReceiveBuffer_DataAdded;
            //sendBuffer.DataAdded += SendBuffer_DataAdded;
        }
        public bool IsConfirmed(long roomID, long userID)
        {
            bool yes = Exists(roomID, userID);
            if (MediaConfirmationType == MediaConfirmationTypes.AllowForAll && !yes)
            {
                ConfirmUser(roomID, userID, true);
                yes = true;
            }
            return yes;
        }
        private bool Exists(long roomID, long userID)
        {
            if (ConfirmedUsers == null)
                ConfirmedUsers = new List<Box<long, long>>();
            var data = from c in ConfirmedUsers where c.Key == roomID && c.Value == userID select c;
            return data.Count() > 0;
        }
        public void ConfirmUser(long roomID, long userID, bool allowForAll)
        {
            SendAccessCommand(roomID, userID, true);
            ConfirmedUsers.Add(new Core.Box<long, long>(roomID, userID));
            if (allowForAll)
                MediaConfirmationType = MediaConfirmationTypes.AllowForAll;
        }
        private void SendAccessCommand(long roomID, long userID, bool can)
        {
            Package pak = new Package(CommandType.MediaPermission, IPAddress.Broadcast, this.IP, can.ToString(), roomID);
            pak.UserID = userID;
            Send(Package.ToByteArray(pak));
        }

        //-----------------------------------1---------------------------------------
        public override void SayHello()
        {
            //Inform to all clients that this client is now online.
            Package pak = new Package(CommandType.Hello, IPAddress.Broadcast, IP, ((int)VideoCodecType).ToString(), RoomID);
            pak.UserID = UserID;
            pak.SenderIP = IP;
            pak.RoomID = RoomID;
            Send(Package.ToByteArray(pak));
        }
        //---------------------------------------------------------------------------
        //---------------------------------2-----------------------------------------
        public void StartVideoForAllInRoom()
        {
            Package pak = new Package(CommandType.ConfirmVideoForAll, IPAddress.Broadcast, IP, IP.ToString(), RoomID);
            pak.UserID = UserID;
            pak.SenderIP = IP;
            Send(Package.ToByteArray(pak));
        }
        //---------------------------------------------------------------------------
    }

    public class LocalAudioClient : LocalTcpClient
    {
        public virtual long RoomID
        {
            get;
            set;
        }
        public List<Box<long, long>> ConfirmedUsers
        {
            get;
            private set;
        }
        public MediaConfirmationTypes MediaConfirmationType { get; set; }
        public FormatDetails AudioCodecType { get; private set; }

        public LocalAudioClient(long userID, long roomID, FormatDetails audioType) : base(userID)
        {
            RoomID = roomID;
            AudioCodecType = audioType;
            //-----------------------------------------------------------------
            ConfirmedUsers = new List<Box<long, long>>();
            //PackageReceived += LocalMediaClient_PackageReceived;
            //receiveBuffer.DataAdded += ReceiveBuffer_DataAdded;
            //sendBuffer.DataAdded += SendBuffer_DataAdded;
        }
        public bool IsConfirmed(long roomID, long userID)
        {
            bool yes = Exists(roomID, userID);
            if (MediaConfirmationType == MediaConfirmationTypes.AllowForAll && !yes)
            {
                ConfirmUser(roomID, userID, true);
                yes = true;
            }
            return yes;
        }
        private bool Exists(long roomID, long userID)
        {
            if (ConfirmedUsers == null)
                ConfirmedUsers = new List<Box<long, long>>();
            var data = from c in ConfirmedUsers where c.Key == roomID && c.Value == userID select c;
            return data.Count() > 0;
        }
        public void ConfirmUser(long roomID, long userID, bool allowForAll)
        {
            SendAccessCommand(roomID, userID, true);
            ConfirmedUsers.Add(new Core.Box<long, long>(roomID, userID));
            if (allowForAll)
                MediaConfirmationType = MediaConfirmationTypes.AllowForAll;
        }
        private void SendAccessCommand(long roomID, long userID, bool can)
        {
            Package pak = new Package(CommandType.MediaPermission, IPAddress.Broadcast, this.IP, can.ToString(), roomID);
            pak.UserID = userID;
            Send(Package.ToByteArray(pak));
        }

        //-----------------------------------1---------------------------------------
        public override void SayHello()
        {
            //Inform to all clients that this client is now online.
            Package pak = new Package(CommandType.Hello, IPAddress.Broadcast, IP, ((int)AudioCodecType.FormatIndex).ToString(), RoomID);
            pak.UserID = UserID;
            pak.SenderIP = IP;
            pak.RoomID = RoomID;
            Send(Package.ToByteArray(pak));
        }
        //---------------------------------------------------------------------------
        //---------------------------------2-----------------------------------------
        public void StartAudioForAllInRoom()
        {
            Package pak = new Package(CommandType.ConfirmVideoForAll, IPAddress.Broadcast, IP, IP.ToString(), RoomID);
            pak.UserID = UserID;
            pak.SenderIP = IP;
            Send(Package.ToByteArray(pak));
        }
        //---------------------------------------------------------------------------
    }
}


