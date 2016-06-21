#region License - Disclaimer
/*
 * Coded by
 * Ahmet Yueksektepe
 * Nick Russler
 * 
 * License
 * 
 * Copyright (c) 2010 Ahmet Yueksektepe and Nick Russler Duisburg, Germany All rights reserved.
 * 
 * Redistribution and use of LeetSocket in source and binary forms, without modification,
 * are permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of conditions and the following disclaimer.
 * 
 * Redistributions in binary form must reproduce the above copyright notice, 
 * this list of conditions and the following disclaimer in the documentation and/or other materials provided with the distribution.
 * 
 * The names of the software authors or contributors may be used to endorse or promote products derived from this software,
 * without specific prior written permission.
 * 
 * You are not allowed to use the source code for commercial uses without specific prior written permission.
 * 
 * Any deviations from these conditions require written permission from the copyright holder in advance
 * 
 * Disclaimer
 * 
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS, AUTHORS, AND CONTRIBUTORS ``AS IS'' AND ANY EXPRESS OR IMPLIED WARRANTIES,
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED.
 * IN NO EVENT SHALL THE COPYRIGHT HOLDER OR ANY AUTHORS OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL,
 * EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES;
 * LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY,
 * OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Threading;
using System.Security.Cryptography;
using System.Xml;
using System.Reflection;


namespace LeetSocket
{
    class LeetSocket : Component
    {
        #region FIELD DECLARATIONS:
        private AsyncCallback pfnWorkerCallBack;
        private Socket internSocket;
        private Stream s = new MemoryStream();
        private int CHUNKSIZE = 15240;
        private int received = -1;
        private int AutoConnectSystemInterval = 3500;
        private int HeartBeatSystemInterval = 1500;
        internal ArrayList SendList = ArrayList.Synchronized(new ArrayList());
        internal ArrayList ReceiveList = ArrayList.Synchronized(new ArrayList());
        internal ArrayList ReceivePauseList = ArrayList.Synchronized(new ArrayList());
        internal ArrayList SendPauseList = ArrayList.Synchronized(new ArrayList());
        internal ArrayList ReserveList = ArrayList.Synchronized(new ArrayList());

#warning Sollte publik sein
        private ArrayList AdressList = new ArrayList();

        private const int BACKLOG = 5;  // Größe der ausstehenden Queue
        int current = 0;
        // Boolean autoConTry = false;
        Boolean isLast = false;
        Boolean isStarter = false;
        Boolean isFile = false;
        Boolean isF_ = false;
        //Boolean isC_F_ = false;
        Boolean isEndCallback = false;
        Boolean isCallback = false;
        //Boolean isHeartbeat = false;
        Boolean isSpecialCommand = false;
        Boolean schleife = true;
        Boolean autoStartListening = false;
        private Boolean isInitialized = false;
        private Boolean isAutoConnectSystemEnabled = false;
        private Boolean isHeartBeatSystemEnabled = false;

        int CustomHeaderLength = 0;
        String serverAsDNS = "127.0.0.1";
        int ExtraBytes = 0;

        BefehlIDVerwalter commandIDverwalter1 = new BefehlIDVerwalter();

        //String mtheory = "";
        enum MAGICBYTEZUSTAENDE : byte { HEARTBEAT = 0, PAUSE, ABBRECHEN, LASTCHUNK, SINGLECHUNK, STARTERCHUNK, NORMALCHUNK, F_LASTCHUNK, F_SINGLECHUNK, F_STARTERCHUNK, F_NORMALCHUNK, C_F_SINGLECHUNK, C_F_STARTERCHUNK, C_SINGLECHUNK, C_STARTERCHUNK, E_SINGLECHUNK, E_STARTERCHUNK, E_C_SINGLECHUNK, E_C_STARTERCHUNK, E_C_F_SINGLECHUNK, E_C_F_STARTERCHUNK, ENDCALLBACK };

        #region MAGICBYTEZUSTAENDE EXPLANATION:
        //isHeartbeat		              HEARTBEAT		    0
        //isPausiert		              PAUSE			    1
        //isAbgebrochen		              ABBRECHEN		    2


        //isLast    !isStarter 	          LASTCHUNK		    3
        //isLast    isStarter	          SINGLECHUNK       4
        //!isLast   isStarter	          STARTERCHUNK		5
        //!isLast   !isStarter	          NORMALCHUNK		6


        //isLast    !isStarter 	isFile    F_LASTCHUNK		7
        //isLast    isStarter	isFile    F_SINGLECHUNK		8
        //!isLast   isStarter	isFile    F_STARTERCHUNK	9
        //!isLast   !isStarter	isFile    F_NORMALCHUNK	    10

        //isLast    isStarter	isFile  isCallBack   C_F_SINGLECHUNK		11
        //!isLast   isStarter	isFile  isCallBack   C_F_STARTERCHUNK	    12

        //isLast    isStarter	isCallBack          C_SINGLECHUNK           13
        //!isLast   isStarter	isCallBack          C_STARTERCHUNK		    14


        //HEARTBEAT = 0, 
        //PAUSE 1, 
        //ABBRECHEN 2, 
        //LASTCHUNK 3, 
        //SINGLECHUNK 4, 
        //STARTERCHUNK 5, 
        //NORMALCHUNK 6, 
        //F_LASTCHUNK 7, 
        //F_SINGLECHUNK 8, 
        //F_STARTERCHUNK 9, 
        //F_NORMALCHUNK 10, 
        //C_F_SINGLECHUNK 11, 
        //C_F_STARTERCHUNK 12, 
        //C_SINGLECHUNK 13, 
        //C_STARTERCHUNK 14, 
        //E_SINGLECHUNK 15, 
        //E_STARTERCHUNK 16, 
        //E_C_SINGLECHUNK 17, 
        //E_C_STARTERCHUNK 18, 
        //E_C_F_SINGLECHUNK 19, 
        //E_C_F_STARTERCHUNK 20, 
        //ENDCALLBACK 21  // user also for special command


        #endregion

        //lol
        //bool jamaic = false;

        String currentPassword = "password";
        byte compression = 0;
        byte encryption = 0;

        IPEndPoint servEndPoint;

        private AsyncCallback SendeCallBack;
        public int sendingVar = 0;
        public Boolean sendingEnded = true;

        string server = "127.0.0.1";
        int servPort = 5678;
        public bool isConnected = false;

        bool isSocketConnected = false;

        // Thread connectThread = null;


        // int timer1v = 0;
        int SOCKETBUFFERSIZE = 30500;
        int FileCHUNKSIZE = 100000;

        //für sendstarter benötigte felder
        byte[] tempCHUNK = null;
        int anzahlDerZuSendendenBytes = 0;
        int indexOfCurrentBefehl = 0;

        AddressFamily addressFamily = AddressFamily.InterNetwork;
        SocketType socketType = SocketType.Stream;
        ProtocolType protocolType = ProtocolType.Tcp;

        System.Threading.Timer HeartBeatTimer = null;
        System.Threading.Timer AutoConnectTimer = null;
        System.Threading.Timer PermanentSendTimer = null;
        System.Threading.Timer WaitForDataTimer = null;

        #region DELEGATE DECLARATIONS:
        //special delegates
        public delegate void ObjectDelegate(Object ReceivedData);
        public delegate void ObjectArrayDelegate(Object[] objArray);
        public delegate void ExceptionDelegate(Exception e, String info);
        public delegate void integerDeletage(int value);
        public delegate void intboolaryDeletage(int value, bool bbb, byte[] customH);
        public delegate void ObjectAndByteArrayDelegate(object value, byte[] bArray);
        public delegate void IntAndByteArrayDelegate(int value, byte[] bArray);
        public delegate void BArrayULongIntBoolIntDelegate(byte[] CustomHeader, ulong TotalLength, int ReceivedBytes, ulong ReceivedBytesAddition, bool isFinished, int CommandID);
        public delegate void BArrayULongIntBoolInt2Delegate(byte[] CustomHeader, ulong TotalLength, int SentBytes, ulong SentBytesAddition, bool isFinished, int CommandID);

        #endregion

        #region EVENT DECLARATIONS:
        //event declarations
        public event ObjectAndByteArrayDelegate OnReceiveCompletedDataEVENT;
        public event ExceptionDelegate OnExceptionEVENT;
        public event integerDeletage OnReceiveData_GetIRX_EVENT;
        public event integerDeletage OnSendData_GetIRX_EVENT;
        public event MethodInvoker OnConnectedEVENT;
        public event MethodInvoker OnBeingConnectedToEVENT;
        public event MethodInvoker OnDisconnectEVENT;
        public event MethodInvoker OnHeartBeatReceivedEVENT;
        public event MethodInvoker OnHeartBeatSentEVENT;
        public event MethodInvoker OnAutoConnectTriesBeginEVENT;
        public event MethodInvoker OnRestartSocketBeginEVENT;
        public event MethodInvoker OnBeginReceiveDataCallEVENT;
        public event BArrayULongIntBoolIntDelegate OnReceiveChunkEVENT;
        public event BArrayULongIntBoolInt2Delegate OnSendChunkEVENT;
        public event intboolaryDeletage OnCommandPausedEVENT;
        public event intboolaryDeletage OnCommandResumedEVENT;
        public event intboolaryDeletage OnCommandCanceledEVENT;
        public event IntAndByteArrayDelegate OnCommandReceiveBeginsEVENT;

        #endregion

        #endregion

        #region EVENT CALLER DECLARATIONS:
        //event callers
        private void OnConnected()
        {
            this.startWaitingForData();

            if (OnConnectedEVENT != null)
            {
                OnConnectedEVENT();
            }
        }
        private void OnBeingConnectedTo()
        {
            this.startWaitingForData();

            if (OnBeingConnectedToEVENT != null)
            {
                OnBeingConnectedToEVENT();
            }
        }
        private void OnCommandReceiveBegins(int id, byte[] b)
        {
            if (OnCommandReceiveBeginsEVENT != null)
            {
                OnCommandReceiveBeginsEVENT(id, b);
            }
        }
        private void OnBeginReceiveDataCall()
        {
            if (OnBeginReceiveDataCallEVENT != null)
            {
                OnBeginReceiveDataCallEVENT();
            }
        }
        private void OnReceiveData_GetIRX(int i)
        {
            if (OnReceiveData_GetIRX_EVENT != null)
            {
                OnReceiveData_GetIRX_EVENT(i);
            }
        }
        private void OnSendData_GetIRX(int i)
        {

            if (OnSendData_GetIRX_EVENT != null)
            {
                OnSendData_GetIRX_EVENT(i);
            }
        }
        private void OnRestartSocketBegin()
        {
            if (OnRestartSocketBeginEVENT != null)
            {
                OnRestartSocketBeginEVENT();
            }
        }
        private void OnHeartBeatReceived()
        {
            if (OnHeartBeatReceivedEVENT != null)
            {
                OnHeartBeatReceivedEVENT();
            }
        }
        private void OnHeartBeatSent()
        {
            if (OnHeartBeatSentEVENT != null)
            {
                OnHeartBeatSentEVENT();
            }
        }
        private void OnAutoConnectTriesBegin()
        {
            if (OnAutoConnectTriesBeginEVENT != null)
            {
                OnAutoConnectTriesBeginEVENT();
            }
        }
        internal void OnException(Exception e, String info)
        {
            if (OnExceptionEVENT != null)
            {
                OnExceptionEVENT(e, info);
            }
        }
        public void OnDisconnect()
        {
            clearEverything();

            // autoConTry = false;

            if (OnDisconnectEVENT != null)
            {
                OnDisconnectEVENT();
            }

            if (autoStartListening)
            {
                startListening();
            }
        }
        internal void OnReceiveCompletedData(Object obj, byte[] b)
        {
            if (OnReceiveCompletedDataEVENT != null)
            {
                OnReceiveCompletedDataEVENT(obj, b);
            }
        }
        internal void OnReceiveChunk(byte[] CustomHeader, ulong TotalLength, int ReceivedBytes, ulong ReceivedBytesAddition, bool isFinished, int commandID)
        {
            if (OnReceiveChunkEVENT != null)
            {
                OnReceiveChunkEVENT(CustomHeader, TotalLength, ReceivedBytes, ReceivedBytesAddition, isFinished, commandID);
            }
        }
        internal void OnSendChunk(byte[] CustomHeader, ulong TotalLength, int SentBytes, ulong SentBytesAddition, bool isFinished, int commandID)
        {
            if (OnSendChunkEVENT != null)
            {
                OnSendChunkEVENT(CustomHeader, TotalLength, SentBytes, SentBytesAddition, isFinished, commandID);
            }
        }
        internal void OnCommandPaused(int ID, bool isSending, byte[] b)
        {
            if (OnCommandPausedEVENT != null)
            {
                OnCommandPausedEVENT(ID, isSending, b);
            }
        }
        internal void OnCommandResumed(int ID, bool isSending, byte[] b)
        {
            if (OnCommandResumedEVENT != null)
            {
                OnCommandResumedEVENT(ID, isSending, b);
            }
        }
        internal void OnCommandCanceled(int ID, bool isSending, byte[] b)
        {

            if (isSending) befreieBEFEHL(ID);

            if (OnCommandCanceledEVENT != null)
            {
                OnCommandCanceledEVENT(ID, isSending, b);
            }
        }

        #endregion

        #region PROPERTY DECLARATIONS:
        //properties

        [
        Category("Server Settings"), 
        DefaultValue("127.0.0.1"),
        Description("The Host IP.")
        ]
        public string ServerIpAsDNS
        {
            get { return serverAsDNS; } //this is property get for varI
            set
            {
                serverAsDNS = value;

                if (!this.DesignMode)
                {
                    server = serverAsDNS;
                    IPAddress servIPAddress = IPAddress.Parse(server);

                    servEndPoint = new IPEndPoint(servIPAddress, servPort);
                }

            }  //this is property set for varI
        }

        //public string ServerIP
        //{
        //    get { return server; } //this is property get for varI
        //}

        [
        Category("Server Settings"),
        DefaultValue(5678),
        Description("The Port to connect to or the Port we listen to.")
        ]        
        public int ServerPORT
        {         
            get { return servPort; } //this is property get for varI
            set
            {
                servPort = value;
                IPAddress servIPAddress = IPAddress.Parse(server);
                servEndPoint = new IPEndPoint(servIPAddress, servPort);

            }  //this is property set for varI
        }

        [
        Category("Automation Settings"),
        DefaultValue(false),
        Description("If enabled LeetSocket will listen on the given Port automatically.")
        ]
        public bool AutoListening
        {
            get
            {
                return autoStartListening;
            } //this is property get for varI
            set
            {
                autoStartListening = value;

                if (autoStartListening && AutoConnectSystem)
                {
                    throw new Exception("Can't be Client and Server at the same Time (AutoStartListening = true + AutoConnectSystem = true)");
                }

                //if (autoStartListening && this.isConnected)
                //{
                //    throw new Exception("Can't be Client and Server at the same Time (AutoStartListening = true + is already connected to a Server)");
                //}
                if (!this.DesignMode)
                {
                    if (autoStartListening)
                    {
                        this.startListening();
                    }
                    
                }

            }  //this is property set for varI
        }

        [
        Category("Automation Settings"),
        DefaultValue(false),
        Description("If enabled LeetSocket will try to connect to the given IP every AutoConnectInterval miliseconds.")
        ]
        public bool AutoConnectSystem
        {
            get
            {
                return isAutoConnectSystemEnabled;
            } //this is property get for varI
            set
            {
                isAutoConnectSystemEnabled = value;

                if (autoStartListening && AutoConnectSystem)
                {
                    throw new Exception("Can't be Client and Server at the same Time (AutoStartListening = true + AutoConnectSystem = true)");
                }

                if (!this.DesignMode)
                {
                    if (isAutoConnectSystemEnabled)
                    {
                        if (AutoConnectTimer == null)
                        {
                            AutoConnectTimer = new System.Threading.Timer(new TimerCallback(AutoConnectTimer_Tick), null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                        }
                        AutoConnectTimer.Change(0, System.Threading.Timeout.Infinite);
                    }
                    else if (AutoConnectTimer != null)
                    {
                        AutoConnectTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    }
                }
            }  //this is property set for varI
        }

        [
        Category("Automation Settings"),
        DefaultValue(3500),
        Description("If AutoConnectSystem is enabled LeetSocket will try to connect to the given IP every AutoConnectInterval miliseconds.")
        ]
        public int AutoConnectInterval
        {
            get
            {
                return AutoConnectSystemInterval;
            } //this is property get for varI
            set
            {
                AutoConnectSystemInterval = value;
            }  //this is property set for varI
        }

        [
        Category("Automation Settings"),
        DefaultValue(false),
        Description("If enabled LeetSocket will check if the connection is alive every HeartBeatInterval miliseconds, for further information Google: \"TCP Keepalive\".")
        ]
        public bool HeartBeatSystem
        {
            get
            {
                return isHeartBeatSystemEnabled;
            } //this is property get for varI
            set
            {
                isHeartBeatSystemEnabled = value;

                if (!this.DesignMode)
                {
                    if (isHeartBeatSystemEnabled)
                    {
                        if (HeartBeatTimer == null)
                        {
                            HeartBeatTimer = new System.Threading.Timer(new TimerCallback(HeartBeatTimer_Tick), null, System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                        }

                        HeartBeatTimer.Change(0, 1);
                    }
                    else if (HeartBeatTimer != null)
                    {
                        HeartBeatTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                    }
                }
            }  //this is property set for varI
        }

        [
        Category("Automation Settings"),
        DefaultValue(1500),
        Description("If HeartBeatSystem is enabled LeetSocket will check if the connection is alive every HeartBeatInterval miliseconds, for further information Google: \"TCP Keepalive\".")
        ]
        public int HeartBeatInterval
        {
            get
            {
                return HeartBeatSystemInterval;
            } //this is property get for varI
            set
            {
                HeartBeatSystemInterval = value;

            }  //this is property set for varI
        }

        [
        Category("Advanced Settings"),
        DefaultValue((byte)0),
        Description("0 for compression Off, 1 for gzip compression.")
        ]
        public byte CompressionLevel
        {
            get { return compression; } //this is property get for varI
            set { compression = value; }  //this is property set for varI
        }

        [
        Category("Advanced Settings"),
        DefaultValue((byte)0),
        Description("0 for encryption Off, 1 for RC4 encryption.")
        ]
        public byte EncryptionLevel
        {
            get { return encryption; } //this is property get for varI
            set { encryption = value; }  //this is property set for varI
        }

        [
        Category("Advanced Settings"),
        DefaultValue("password"),
        Description("The Password used for the Encryption, min. length is 8 characters.")
        ]
        public string EncryptionPassword
        {
            get { return currentPassword; } //this is property get for varI
            set { currentPassword = value; }  //this is property set for varI
        }

        [
        Category("Advanced Settings"),
        DefaultValue(15240),
        Description("The max. Size of a Data Chunk.")
        ]
        public int ChunkSize
        {
            get { return CHUNKSIZE; } //this is property get for varI
            set { CHUNKSIZE = value; }  //this is property set for varI
        }

        //public Socket InternSocket
        //{
        //    get { return internSocket; } //this is property get for varI
        //    set
        //    {
        //        internSocket = value;

        //    }  //this is property set for varI
        //}






        //public int SocketBufferSize
        //{
        //    get { return SOCKETBUFFERSIZE; } //this is property get for varI
        //    set { SOCKETBUFFERSIZE = value; }  //this is property set for varI
        //}

        [
        Category("Advanced Settings"),
        DefaultValue(100000),
        Description("The max. Size of a File Data Chunk.")
        ]
        public int FileChunkSize
        {
            get { return FileCHUNKSIZE; } //this is property get for varI
            set { FileCHUNKSIZE = value; }  //this is property set for varI
        }

        [
        Category("Advanced Settings"),
        DefaultValue(0),
        Description("Length of the Array you can send with the Objects/Files.")
        ]
        public int CustomHeaderLengthValue
        {
            get { return CustomHeaderLength; } //this is property get for varI
            set { CustomHeaderLength = value; }  //this is property set for varI
        }

        #endregion
        //contructor:
        #region CONSTRUCTORS:
        public LeetSocket()
        {

        }
        public LeetSocket(AddressFamily addressFamily, SocketType socketType, ProtocolType protocolType, String server, int servPort, byte compression, byte encryption, String currentPassword)
        {

            this.server = server;
            this.servPort = servPort;
            this.compression = compression;
            this.encryption = encryption;
            this.currentPassword = currentPassword;
            this.addressFamily = addressFamily;
            this.socketType = socketType;
            this.protocolType = protocolType;
            Initialize();
        }
        public LeetSocket(Socket AlreadyConnectedSocket, byte compression, byte encryption, String currentPassword)
        {


            this.compression = compression;
            this.encryption = encryption;
            this.currentPassword = currentPassword;


            //save socket and, save current ip and port
            internSocket = AlreadyConnectedSocket;
            server = ((IPEndPoint)internSocket.RemoteEndPoint).Address.ToString();
            servPort = ((IPEndPoint)internSocket.RemoteEndPoint).Port;
            IPAddress servIPAddress = IPAddress.Parse(server);
            servEndPoint = new IPEndPoint(servIPAddress, servPort);

            isSocketConnected = true;
            isConnected = true;

            //connectThread initen
            // connectThread = new Thread(new ThreadStart(_Connect));

            //permanentSendThread
            PermanentSendTimer = new System.Threading.Timer(new TimerCallback(permanentSend_Tick), null, 0, System.Threading.Timeout.Infinite);

        }
        #endregion

        private void Initialize()
        {
            try
            {
                server = serverAsDNS;
                //try
                //{
                //    server = Dns.GetHostEntry(serverAsDNS).AddressList[0].ToString();
                //}
                //catch (Exception e)
                //{
                //    MessageBox.Show(e.Message.ToString());
                //    //server = value;
                //}
                //MessageBox.Show("!" + server.ToString() + "!");
                IPAddress servIPAddress = (IPAddress)Dns.GetHostAddresses(server)[0];

                //IPAddress servIPAddress = IPAddress.Parse(server);
                servEndPoint = new IPEndPoint(servIPAddress, servPort);

                //erzeuge socket
                internSocket = new Socket(addressFamily, socketType, protocolType);
                internSocket.ReceiveBufferSize = SOCKETBUFFERSIZE;
                internSocket.SendBufferSize = SOCKETBUFFERSIZE;
                //permanentSendThread
                PermanentSendTimer = new System.Threading.Timer(new TimerCallback(permanentSend_Tick), null, 0, System.Threading.Timeout.Infinite);
            }
            catch (Exception e)
            {
                OnException(e, "Initialize() Error");
            }
        }

        private void addInSendList(object value)
        {
            //try
            //{
                SendList.Add(value);

            //}
            //catch (Exception e)
            //{
            //    OnException(e, "addInSendList() Error");
            //}
        }
        public void restartSocket()
        {
            try
            {
                OnRestartSocketBegin();

                try
                {
                    internSocket.Shutdown(SocketShutdown.Both);
                }
                catch { }

                try
                {
                    internSocket.Disconnect(false);
                }
                catch { }

                try
                {
                    internSocket.Close();
                }
                catch { }

                internSocket = null;

                internSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                internSocket.ReceiveBufferSize = SOCKETBUFFERSIZE;
                internSocket.SendBufferSize = SOCKETBUFFERSIZE;
                //IPAddress servIPAddress = IPAddress.Parse(server);
                IPAddress servIPAddress = (IPAddress)Dns.GetHostAddresses(server)[0];
                servEndPoint = new IPEndPoint(servIPAddress, servPort);
            }
            catch (Exception e) { OnException(e, "restartSocket() Error"); }
        }

        public object getObjectWithByteArray(MemoryStream theByteArrayAsMStream)
        {
            try
            {
                return new BinaryFormatter().Deserialize(theByteArrayAsMStream);
            }
            catch (Exception e) { OnException(e, "getObjectWithByteArray()2 Error"); return null; }

        }

        //private void _Connect()
        //{
        //    try
        //    {
        //        //OnAutoConnectTriesBeginEVENT  hierausführen
        //        OnAutoConnectTriesBegin();
        //        while (!isConnected)
        //        {
        //            if (AdressList != null)
        //            {
        //                foreach (Object[] adress in AdressList)
        //                {
        //                    String ip = (String)adress[0];
        //                    int port = (int)adress[1];
        //                    IPAddress servIPAddress = (IPAddress)Dns.GetHostAddresses(ip)[0];

        //                    //IPAddress servIPAddress = IPAddress.Parse(server);
        //                    servEndPoint = new IPEndPoint(servIPAddress, port);

        //                    ConnectE();
        //                    System.Threading.Thread.Sleep(15);
        //                }
        //            }
        //            else
        //            {

        //                ConnectE();

        //            }
        //            System.Threading.Thread.Sleep(1000);
        //        }
        //        // da wir jetzt connected sind gehts los mit dem receivenn, nihahaha

        //        //WaitForData(internSocket);//auskommentiert.. dem user im onconnected event listenen lassen

        //        //Thread tmp = connectThread;
        //        timer1v = 0;
        //        //connectThread = null;
        //        //connectThread.

        //        //OnConnectedEVENT  hierausführen
        //        OnConnected();
        //        //try
        //        //{
        //        //    tmp.Abort();
        //        //}
        //        //catch (Exception) { }
        //    }
        //    catch (Exception e) { OnException(e, "_Connect() Error"); }
        //}

        private void ConnectE()
        {
            try
            {

#warning neu isInitialized
                if (!isInitialized)
                {
                    isInitialized = true;
                    this.Initialize();
                }

                internSocket.Connect(servEndPoint);

                isSocketConnected = true;
                isConnected = true;

            }
            catch (Exception e)
            {
                //OnException(e,"couldnt connect: " + servEndPoint.ToString());  
            }
        }

        public void connect()
        {
            try
            {
#warning neu isInitialized
                if (!isInitialized)
                {
                    isInitialized = true;
                    this.Initialize();
                }

                if (!isSocketConnected)
                {
                    internSocket.Connect(servEndPoint);

                    isSocketConnected = true;
                    isConnected = true;



                    //WaitForData(internSocket); //auskommentiert.. dem user im onconnected event listenen lassen
                    // timer1v = 0;
                    //connectThread = null;
                    OnConnected();
                }
            }
            catch (Exception se)
            {
#warning EVENT: OnException  hierausführen
#warning warum diese zeiel nicht ausgeführt
                OnException(se, "Connect() Error");
                MessageBox.Show("couldnt connect: " + se.Message.ToString());
            }
        }

        public byte[] getByteArrayWithObject(Object o)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                BinaryFormatter bf1 = new BinaryFormatter();
                bf1.Serialize(ms, o);
                return ms.ToArray();
            }
            catch (Exception e) { OnException(e, "getByteArrayWithObject() Error"); return null; }

        }
         
        private void startWaitingForData()
        {
            //manually starts waiting for data
            try
            {
                WaitForDataTimer = new System.Threading.Timer(new TimerCallback(WaitForData));
                WaitForDataTimer.Change(0, System.Threading.Timeout.Infinite);               
            }
            catch (Exception e) { OnException(e, "startWaitingForData() Error"); }
        }

        //WaitThread
        private void WaitForData(Object state)
        {
            try
            {
                byte[] dataBuffer = new byte[SOCKETBUFFERSIZE];

                // now start to listen for any data...

                while (isConnected)
                {
                    //event call
                    OnBeginReceiveDataCall();

                    //anzahl der abgeholten bytes
                    int receivedBytes=internSocket.Receive(dataBuffer);

                    //datenverarbeitung
                    #region OnDataReceive
                    try
                    {      
                        //disconnect event, event call, da irx 0 ist
                        #region OnDisconnect
                        if (receivedBytes == 0)
                        {
                            OnDisconnect();

                            isConnected = false;
                            isSocketConnected = false;
                            return;
                        }
                        #endregion


                        OnReceiveData_GetIRX(receivedBytes);

                        //schreibe aus dem buffer ins stream so oft wie irx
                        s.Write(dataBuffer, 0, receivedBytes);

                        //resetten der Variablen
                        #region VARS
                        isLast = false;
                        isStarter = false;
                        isFile = false;
                        isF_ = false;
                        //isC_F_ = false;
                        isEndCallback = false;
                        isCallback = false;
                        //isHeartbeat = false;
                        isSpecialCommand = false;
                        received = -1;
                        schleife = true;
                        #endregion


                        //DATA INTERPRETATION UND BEHANDLUNG
                        #region WHILE_SCHLEIFE
                        while (schleife)
                        {
                            //IST IM STREAM ÜBERHAUPT ETWAS, falls ja, wird magic bytes ermittelt und danach versucht receive zu berechnen, 
                            //dafür müssen vielelicht auch so 7,8 oder mehr bytes schon da sein, z.b. wegen length of last chunk
                            #region IF ABFRAGE STREAM LÄNGE > 0
                            if (s.Length > 0)
                            {
                                //gets the first byte in the stream which is the magic byte, position must be restroed for maybe not reseting stream and new coming data
                                long pos = s.Position;
                                s.Position = 0;
                                byte magicByte = (byte)s.ReadByte();
                                s.Position = pos;

                                //MAGICBYTE SWITCH, magicbytezustände werden behandelt
                                #region SWITCH MAGICBYTE,
                                switch (magicByte)
                                {
#warning die reihenflge der case if abfragen könnte man für performance ändern, normal chunk kommt öfter vor als.. z.b.
                                    case (byte)MAGICBYTEZUSTAENDE.HEARTBEAT:
                                        #region HEARTBEAT CODE REGION
                                        deleteFirstByte();

                                        //OnHeartBeatReceivedEVENT wird ausgelöst
                                        OnHeartBeatReceived();
                                        continue;
                                        #endregion
                                    //break;

                                    case (byte)MAGICBYTEZUSTAENDE.PAUSE:
                                        #region PAUSE CODE REGION, Unfinished yet
                                        deleteFirstByte();
                                        Empfaenger e11 = (Empfaenger)ReceiveList[current];
                                        OnCommandPaused(e11.commandID, false, e11.CustomHeader);


                                        ReceivePauseList.Add(ReceiveList[current]);
                                        ReceiveList.Remove(ReceiveList[current]);



                                        if (ReceiveList.Count == 0) { current = 0; }
                                        else
                                            current = current % ReceiveList.Count;

                                        continue;
                                        #endregion
                                    //break;

                                    case (byte)MAGICBYTEZUSTAENDE.ABBRECHEN:
                                        #region ABBRECHEN CODE REGION, Unfinished yet
                                        deleteFirstByte();
                                        Empfaenger e22 = (Empfaenger)ReceiveList[current];
                                        OnCommandCanceled(e22.commandID, false, e22.CustomHeader);

                                        ReceiveList.Remove(ReceiveList[current]);

                                        if (ReceiveList.Count == 0) { current = 0; }
                                        else
                                            current = current % ReceiveList.Count;

                                        continue;
                                        #endregion
                                    //break;

                                    case (byte)MAGICBYTEZUSTAENDE.LASTCHUNK:
                                        #region LASTCHUNK CODE REGION
                                        isLast = true;
                                        isStarter = false;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.SINGLECHUNK:
                                        #region SINGLECHUNK CODE REGION
                                        isLast = true;
                                        isStarter = true;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.STARTERCHUNK:
                                        #region STARTERCHUNK CODE REGION
                                        isLast = false;
                                        isStarter = true;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.NORMALCHUNK:
                                        #region NORMALCHUNK CODE REGION
                                        isLast = false;
                                        isStarter = false;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.F_LASTCHUNK:
                                        #region F_LASTCHUNK CODE REGION
                                        isLast = true;
                                        isStarter = false;
                                        isFile = true;
                                        isF_ = true;
                                        //isC_F_ = false;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.F_SINGLECHUNK:
                                        #region F_SINGLECHUNK CODE REGION
                                        isLast = true;
                                        isStarter = true;
                                        isFile = true;
                                        isF_ = true;
                                        //isC_F_ = false;
                                        ExtraBytes = CustomHeaderLength * (-1);
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.F_STARTERCHUNK:
                                        #region F_STARTERCHUNK CODE REGION
                                        isLast = false;
                                        isStarter = true;
                                        isFile = true;
                                        isF_ = true;
                                        //isC_F_ = false;
                                        ExtraBytes = CustomHeaderLength * (-1);
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.F_NORMALCHUNK:
                                        #region F_NORMALCHUNK CODE REGION
                                        isLast = false;
                                        isStarter = false;
                                        isFile = true;
                                        isF_ = true;
                                        //isC_F_ = false;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.C_F_SINGLECHUNK:
                                        #region F_SINGLECHUNK CODE REGION
                                        isLast = true;
                                        isStarter = true;
                                        isFile = true;
                                        //isCallback = true;
                                        ExtraBytes = 5 + 1;
                                        isF_ = false;
                                        //isC_F_ = true;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.C_F_STARTERCHUNK:
                                        #region F_STARTERCHUNK CODE REGION
                                        isLast = false;
                                        isStarter = true;
                                        isFile = true;
                                        //isCallback = true;
                                        ExtraBytes = 5 + 1;
                                        isF_ = false;
                                        //isC_F_ = true;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.C_SINGLECHUNK:
                                        #region F_SINGLECHUNK CODE REGION
                                        isLast = true;
                                        isStarter = true;
                                        isCallback = true;
                                        ExtraBytes = 5;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.C_STARTERCHUNK:
                                        #region F_STARTERCHUNK CODE REGION
                                        isLast = false;
                                        isStarter = true;
                                        isCallback = true;
                                        ExtraBytes = 5;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.E_SINGLECHUNK:
                                        #region F_SINGLECHUNK CODE REGION
                                        isLast = true;
                                        isStarter = true;
                                        //isCallback = true;
                                        isEndCallback = true;
                                        ExtraBytes = 4;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.E_STARTERCHUNK:
                                        #region F_STARTERCHUNK CODE REGION
                                        isLast = false;
                                        isStarter = true;
                                        //isCallback = true;
                                        ExtraBytes = 4;
                                        isEndCallback = true;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.E_C_SINGLECHUNK:
                                        #region F_SINGLECHUNK CODE REGION
                                        isLast = true;
                                        isStarter = true;
                                        isCallback = true;
                                        ExtraBytes = 5 + 4;
                                        isEndCallback = true;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.E_C_STARTERCHUNK:
                                        #region F_STARTERCHUNK CODE REGION
                                        isLast = false;
                                        isStarter = true;
                                        isCallback = true;
                                        ExtraBytes = 5 + 4;
                                        isEndCallback = true;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.E_C_F_SINGLECHUNK:
                                        #region F_SINGLECHUNK CODE REGION
                                        isLast = true;
                                        isStarter = true;
                                        isFile = true;
                                        //isCallback = true;
                                        ExtraBytes = 5 + 1 + 4;
                                        isF_ = false;
                                        //isC_F_ = true;
                                        isEndCallback = true;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.E_C_F_STARTERCHUNK:
                                        #region F_STARTERCHUNK CODE REGION
                                        isLast = false;
                                        isStarter = true;
                                        isFile = true;
                                        //isCallback = true;
                                        ExtraBytes = 5 + 1 + 4;
                                        isF_ = false;
                                        //isC_F_ = true;
                                        isEndCallback = true;
                                        #endregion
                                        break;

                                    case (byte)MAGICBYTEZUSTAENDE.ENDCALLBACK:
                                        #region F_STARTERCHUNK CODE REGION
                                        isLast = true;
                                        isStarter = true;
                                        isSpecialCommand = true;
                                        ExtraBytes = CustomHeaderLength * (-1);
                                        #endregion
                                        break;






                                    //if default, smthin must go wrong here!?
                                    default:
                                        throw new Exception();

                                    //break;

                                }
                                #endregion

                                //SCHLEIFENBEDINGUNGEN, aber das sind nicht alle, weitere sind im code verteilt
                                #region SCHLEIFENBEDINGUNGEN

                                schleife = isStarter && (s.Length > (2 + CustomHeaderLength + ExtraBytes));
                                schleife = schleife || (isStarter && !isLast && s.Length > (CHUNKSIZE + (2 + CustomHeaderLength + ExtraBytes)));
                                schleife = schleife || ((!isStarter && !isLast) && (s.Length > CHUNKSIZE));
                                if (ReceiveList.Count > 0)
                                    schleife = schleife || ((!isStarter && isLast) && (((((Empfaenger)(ReceiveList[current])).lengthOfLastChunk) < s.Length)));
                                #endregion

                                //RECEIVE BERECHNUNG, receive wird berechnet je nachdem was für ein magic byte bzw. isstarter islast etc...
                                #region RECEIVE BERECHNUNG
                                if (isStarter)
                                {
                                    //starter or singlechunk

                                    if ((s.Length > (2 + CustomHeaderLength + ExtraBytes)))
                                    {

                                        if (isLast)
                                        {
                                            long pos1 = s.Position;
                                            s.Position = 0;
                                            byte[] data = new byte[3 + CustomHeaderLength + ExtraBytes];
                                            s.Read(data, 0, 3 + CustomHeaderLength + ExtraBytes);
                                            s.Position = pos;
                                            byte[] tmpk = new byte[2];
                                            tmpk[0] = data[1];
                                            tmpk[1] = data[2];
                                            short lengthOfLastChunk = BitConverter.ToInt16(tmpk, 0);

                                            received = lengthOfLastChunk + 3 + CustomHeaderLength + ExtraBytes;
                                            schleife = schleife || s.Length > received - 1;
                                        }
                                        else received = (CHUNKSIZE + 3 + CustomHeaderLength + ExtraBytes);
                                        //  nureinmal = false;

                                    }
                                    else
                                    {
                                        schleife = false;
                                    }

                                }
                                else
                                {
                                    if (!isLast)
                                    {
                                        //normalchunk
                                        received = CHUNKSIZE + 1;
                                    }
                                    else
                                    {
                                        //lastchunk
                                        received = ((Empfaenger)ReceiveList[current]).lengthOfLastChunk + 1;
                                    }
                                }
                                #endregion
                            }
                            else schleife = false;

                            #endregion

                            // IF RECEIVE WURDE ERMITTELT UND GENUG BYTES IM STREAM DA 
                            #region RECEIVE IF ABFRAGE
                            if ((received > -1) && (s.Length > (received - 1)))
                            {
                                //IF ISFILE, FILECHUNK BEHANDLUNG, liegt überhaupt eine datei vor, falls ja, starter oder nicht, entsprechende reaktionen
                                #region FILECHUNK BEHANDLUNG
                                if (isFile)
                                {

                                    //IF ABFRAGE
                                    #region NORMAL FILE CHUNK OR (SINGLE OR STARTER)
                                    if (isF_)
                                    {
                                        #region NORMAL FILE CHUNK

                                        //bestehende fileempfänger wird benutzt
                                        FileEmpfaenger e = (FileEmpfaenger)ReceiveList[current];

                                        //gets data with header etc.
                                        s.Position = 0;
                                        byte[] data = new byte[received];
                                        s.Read(data, 0, data.Length);

                                        //sos bytes
                                        ResetStreamAndSaveSosBytes();

                                        //adde
                                        e.addChunk(data);

                                        //fallse nachdem adden tod
                                        if (e.isDead)
                                        {
                                            ReceiveList.RemoveAt(current);
                                            if (ReceiveList.Count == 0) { current = 0; }
                                            else
                                                current = current % ReceiveList.Count;
                                        }
                                        else
                                        {
                                            current = (current + 1) % ReceiveList.Count;
                                        }
                                        #endregion
                                    }
                                    else
                                    {
                                        //BEHANDLUNG VON FILE SINGLE OR STARTER CHUNK, neue file übertragung fängt an, filebefehl wird erzeugt
                                        #region SINGLE OR STARTER CHUNK
                                        s.Position = 0;
                                        byte[] data = new byte[received];
                                        s.Read(data, 0, data.Length);

                                        //lengthoflastchunk wird bestimmt
                                        byte[] tmp = new byte[2];
                                        tmp[0] = data[1];
                                        tmp[1] = data[2];
                                        short lengthOfLastChunk = BitConverter.ToInt16(tmp, 0);

                                        //commandID wird entnommen
                                        int commandID1 = 1337;
                                        if (isEndCallback)
                                        {
                                            byte[] tmppp = new byte[4];
                                            tmppp[0] = data[1 + 2 + 1 + 5 + CustomHeaderLength];
                                            tmppp[1] = data[1 + 2 + 1 + 5 + CustomHeaderLength + 1];
                                            tmppp[2] = data[1 + 2 + 1 + 5 + CustomHeaderLength + 2];
                                            tmppp[3] = data[1 + 2 + 1 + 5 + CustomHeaderLength + 3];
                                            commandID1 = BitConverter.ToInt32(tmppp, 0);

                                        }
                                        //new FileEmpfänger
                                        FileEmpfaenger e = new FileEmpfaenger(this, lengthOfLastChunk, data, CustomHeaderLength, isEndCallback, commandID1);

                                        OnCommandReceiveBegins(e.commandID, e.CustomHeader);

                                        //sos bytes
                                        ResetStreamAndSaveSosBytes();

                                        if (e.isSingle)
                                        {
                                            //nothing needed to do
                                        }
                                        else
                                        {
                                            //#warning big veränderung
                                            //ReceiveList.Add(e);
                                            ReceiveList.Insert(current, e);
                                            current = (current + 1) % ReceiveList.Count;
                                        }
                                        #endregion
                                    }
                                    #endregion
                                }
                                #endregion

                                //SONST NORMAL BEFEHLCHUNK BEHANDLUNG
                                #region NORMAL BEFEHLCHUNK BEHANDLUNG
                                else if (isStarter)
                                {
                                    //STARTER or SINGLE .. a new entry in ReceiveList
                                    #region STARTER or SINGLE BEFEHLCHUNK BEHANDLUNG / VORBEREITUNG

                                    //get data
                                    s.Position = 0;
                                    byte[] data = new byte[received];
                                    s.Read(data, 0, data.Length);

                                    //lenghtoflastchunk bestimmung
                                    byte[] tmp = new byte[2];
                                    tmp[0] = data[1];
                                    tmp[1] = data[2];
                                    short lengthOfLastChunk = BitConverter.ToInt16(tmp, 0);


                                    int temp1 = 0;
                                    if (!isSpecialCommand) temp1 = CustomHeaderLength;
                                    Empfaenger e = new Empfaenger(lengthOfLastChunk, data, temp1, isCallback, isEndCallback, isSpecialCommand);

                                    if (!e.isSpecialCommand)
                                        OnCommandReceiveBegins(e.commandID, e.CustomHeader);


                                    if (e.isGetCallBack)
                                        OnReceiveChunk(e.CustomHeader, e.BefehlLength, ((int)(e.buffer.Length - e.tempBufferLength)), System.Convert.ToUInt64(e.buffer.Length),(System.Convert.ToUInt64(e.buffer.Length) == e.BefehlLength), e.commandID);


                                    if (!isLast)
                                    {
                                        //STARTER
                                        #region STARTER CHUNK BEHANDLUNG

                                        ResetStreamAndSaveSosBytes();

                                        //#warning big veränderung
                                        //ReceiveList.Add(e);
                                        ReceiveList.Insert(current, e);
                                        current = (current + 1) % ReceiveList.Count;

                                        #endregion
                                    }
                                    else
                                    {
                                        //SINGLE
                                        #region SINGLE CHUNK BEHANDLUNG

                                        ResetStreamAndSaveSosBytes();

                                        finishedTransfer(e);

                                        #endregion

                                    }
                                    #endregion

                                }
                                else
                                {
                                    if (isLast)
                                    {
                                        //LASTCHUNK
                                        #region LASTCHUNK BEHANDLUNG

                                        //get current emp
                                        Empfaenger e = (Empfaenger)ReceiveList[current];

                                        //get data
                                        s.Position = 0;
                                        byte[] data = new byte[received];
                                        s.Read(data, 0, received);

                                        //adde
                                        e.addChunk(data);

                                        //callback
                                        if (e.isGetCallBack)
                                            OnReceiveChunk(e.CustomHeader, e.BefehlLength, ((int)(e.buffer.Length - e.tempBufferLength)), System.Convert.ToUInt64(e.buffer.Length),(System.Convert.ToUInt64(e.buffer.Length) == e.BefehlLength), e.commandID);

                                        ResetStreamAndSaveSosBytes();

                                        ReceiveList.RemoveAt(current);

                                        finishedTransfer(e);

                                        if (ReceiveList.Count == 0) { current = 0; }
                                        else
                                            current = current % ReceiveList.Count;

                                        #endregion
                                    }
                                    else
                                    {
                                        //NORMAL CHUNK
                                        #region NORMALCHUNK BEHANDLUNG
                                        ///////////////// ///////////////////

                                        if (ReceiveList.Count > 0)
                                        {
                                            //current emp
                                            Empfaenger e = (Empfaenger)ReceiveList[current];

                                            //get data
                                            s.Position = 0;
                                            byte[] data = new byte[received];
                                            s.Read(data, 0, received);

                                            //adde
                                            e.addChunk(data);

                                            if (e.isGetCallBack)
                                                OnReceiveChunk(e.CustomHeader, e.BefehlLength, ((int)(e.buffer.Length - e.tempBufferLength)), System.Convert.ToUInt64(e.buffer.Length),(System.Convert.ToUInt64(e.buffer.Length) == e.BefehlLength), e.commandID);
                                            
                                            ResetStreamAndSaveSosBytes();

                                            current = (current + 1) % ReceiveList.Count;
                                        }
                                        #endregion
                                    }

                                }

                                #endregion

                            }
                            else schleife = false;

                            #endregion

                            //zurück setzen der variablen
                            #region VARS
                            isLast = false;
                            isStarter = false;
                            //isHeartbeat = false;
                            isSpecialCommand = false;
                            isFile = false;
                            isF_ = false;
                            //isC_F_ = false;
                            isEndCallback = false;
                            isCallback = false;
                            received = -1;
                            ExtraBytes = 0;
                            #endregion
                        }
                        #endregion


                        //Fange wieder an zu listenen mit einer neuen schleifen runde 
                    }
                    catch (Exception e)
                    {                       
                        OnDisconnect();
                        OnException(e, "OnDataReceived() Error");                        
                    }
                    #endregion
                }
            }
            catch (Exception se)
            {
                OnDisconnect();
                OnException(se, "WaitForData() Error");
            }
        }

        private void deleteFirstByte()
        {
            try
            {
                //lösche ein byte aus dem stream
                s.Position = 1;
                byte[] sos = new byte[(int)s.Length - 1];
                s.Read(sos, 0, sos.Length);
                s = new MemoryStream();
                s.Write(sos, 0, sos.Length);
            }
            catch (Exception e) { OnException(e, "deleteFirstByte() Error"); }
        }

        private void ResetStreamAndSaveSosBytes()
        {
            try
            {
                s.Position = received;
                byte[] sos = new byte[(int)s.Length - received];
                s.Read(sos, 0, sos.Length);
                s = new MemoryStream();
                s.Write(sos, 0, sos.Length);
            }
            catch (Exception e) { OnException(e, "ResetStreamAndSaveSosBytes() Error"); }
        }

        internal void sendEndCallback(int i)
        {
            try
            {
                byte[] b = new byte[5];
                byte[] id1 = BitConverter.GetBytes(i);

                b[0] = 0;
                b[1] = id1[0];
                b[2] = id1[1];
                b[3] = id1[2];
                b[4] = id1[3];

                deliverSpecialCommand(b);

            }
            catch (Exception e) { OnException(e, "sendEndCallback() Error"); }
        }

        public void PauseCommand_Request(int CommandID)
        {
            try
            {
                byte[] b = new byte[5];
                byte[] id1 = BitConverter.GetBytes(CommandID);

                b[0] = 1;
                b[1] = id1[0];
                b[2] = id1[1];
                b[3] = id1[2];
                b[4] = id1[3];

                deliverSpecialCommand(b);

            }
            catch (Exception e) { OnException(e, "sendPausierenVerlangung() Error"); }
        }
        public void AbortCommand_Request(int CommandID)
        {
            try
            {
                byte[] b = new byte[5];
                byte[] id1 = BitConverter.GetBytes(CommandID);

                b[0] = 2;
                b[1] = id1[0];
                b[2] = id1[1];
                b[3] = id1[2];
                b[4] = id1[3];

                deliverSpecialCommand(b);
            }
            catch (Exception e) { OnException(e, "sendAbbrechenVerlangung() Error"); }
        }
        public void ContinueCommand_Request(int CommandID)
        {
            try
            {
                byte[] b = new byte[5];
                byte[] id1 = BitConverter.GetBytes(CommandID);

                b[0] = 3;
                b[1] = id1[0];
                b[2] = id1[1];
                b[3] = id1[2];
                b[4] = id1[3];

                deliverSpecialCommand(b);
            }
            catch (Exception e) { OnException(e, "sendFortfuehrenVerlangung() Error"); }
        }

        public bool PauseCommand(int CommandID)
        {
            try
            {
                bool done = false;
                Befehl b = null;
                for (int m = 0; m < SendList.Count; m++)
                {
                    Befehl befehl1 = (Befehl)SendList[m];

                    if (befehl1.commandID == CommandID)
                    {
                        b = befehl1;
                        break;
                    }


                }
                if (b != null)
                {
                    b.downloadStatus = 1;
                    done = true;
                }
                return done;
            }
            catch (Exception e) { OnException(e, "pausiereBEFEHL() Error"); return false; }

        }
        public bool AbortCommand(int CommandID)
        {
            try
            {
                bool done = false;
                Befehl b = null;
                for (int m = 0; m < SendList.Count; m++)
                {
                    Befehl befehl1 = (Befehl)SendList[m];

                    if (befehl1.commandID == CommandID)
                    {
                        b = befehl1;
                        break;
                    }


                }
                if (b != null)
                {
                    b.downloadStatus = 2;

                    done = true;
                }
                else
                {
                    //villeicht in pausierliste
                    for (int m = 0; m < SendPauseList.Count; m++)
                    {
                        Befehl befehl1 = (Befehl)SendPauseList[m];

                        if (befehl1.commandID == CommandID)
                        {
                            b = befehl1;
                            break;
                        }


                    }
                    if (b != null)
                    {

                        byte[] bb = new byte[5];
                        byte[] id1 = BitConverter.GetBytes(CommandID);

                        bb[0] = 5;
                        bb[1] = id1[0];
                        bb[2] = id1[1];
                        bb[3] = id1[2];
                        bb[4] = id1[3];

                        deliverSpecialCommand(bb);

                        OnCommandCanceled(b.commandID, true, b.CustomHeader);
                        SendPauseList.Remove(b);

                        done = true;
                    }

                }
                return done;
            }
            catch (Exception e) { OnException(e, "abbrecheBEFEHL() Error"); return false; }
        }
        public bool ContinueCommand(int CommandID)
        {
            try
            {
                //MessageBox.Show("ID: " + i.ToString() + "  Count: " + SendPauseList.Count.ToString());
                bool done = false;
                Befehl b = null;
                for (int m = 0; m < SendPauseList.Count; m++)
                {
                    Befehl befehl1 = (Befehl)SendPauseList[m];

                    if (befehl1.commandID == CommandID)
                    {
                        b = befehl1;
                        break;
                    }


                }
                if (b != null)
                {
                    //MessageBox.Show("b not null, ID: " + i.ToString());
                    b.downloadStatus = 5;
                    b.isDead = false;

                    byte[] bb = new byte[5];
                    byte[] id1 = BitConverter.GetBytes(CommandID);

                    bb[0] = 4;
                    bb[1] = id1[0];
                    bb[2] = id1[1];
                    bb[3] = id1[2];
                    bb[4] = id1[3];

                    OnCommandResumed(b.commandID, true, b.CustomHeader);
                    deliverSpecialCommand(bb);
                    addInSendList(b);
                    SendPauseList.Remove(b);

                    done = true;
                }
                return done;
            }
            catch (Exception e) { OnException(e, "fortfuehreBEFEHL() Error"); return false; }
        }

        private void fortfuehreBEFEHLBEFEHL(int i)
        {
            try
            {
                Empfaenger b = null;
                for (int m = 0; m < ReceivePauseList.Count; m++)
                {
                    Empfaenger befehl1 = (Empfaenger)ReceivePauseList[m];

                    if (befehl1.commandID == i)
                    {
                        b = befehl1;
                        break;
                    }


                }
                if (b != null)
                {
                    OnCommandResumed(b.commandID, false, b.CustomHeader);
                    //MessageBox.Show("test");

                    //ReceiveList.Add(b);
                    //instead of adding , we insert at current
                    ReceiveList.Insert(current, b);

                    ReceivePauseList.Remove(b);

                }
            }
            catch (Exception e) { OnException(e, "fortfuehreBEFEHLBEFEHL() Error"); }
        }

        private void befreieBEFEHL(int i)
        {
            try
            {
                this.commandIDverwalter1.beendeAuftrag(i);
            }
            catch (Exception e) { OnException(e, "befreieBEFEHL() Error"); }
        }

        public bool startReserved(int i)
        {
            try
            {
                bool done = false;
                //MessageBox.Show("test111");
                Befehl b = null;
                for (int m = 0; m < ReserveList.Count; m++)
                {
                    Befehl befehl1 = (Befehl)ReserveList[m];

                    if (befehl1.commandID == i)
                    {
                        b = befehl1;
                        break;
                    }


                }
                if (b != null)
                {
                    //MessageBox.Show("test112");
                    done = true;
                    SendList.Add(b);
                    ReserveList.Remove(b);

                }

                return done;
            }
            catch (Exception e) { OnException(e, "startReserved() Error"); return false; }
        }

        public object getObjectWithByteArray(byte[] b)
        {
            try
            {
                MemoryStream m = new MemoryStream(b);
                m.Position = 0;
                return new BinaryFormatter().Deserialize(m);
            }
            catch (Exception e) { OnException(e, "getObjectWithByteArray()1 Error"); return null; }
        }

        private void finishedTransfer(Empfaenger e)
        {
            try
            {
                Object o = null;

                if (e.isSpecialCommand)
                {
                    o = finishedTransfer2(e);
                    if (o != null)
                    {

                        byte[] b1 = (byte[])o;

                        byte mode = b1[0];

                        byte[] commandID1 = new byte[4];

                        commandID1[0] = b1[1];
                        commandID1[1] = b1[2];
                        commandID1[2] = b1[3];
                        commandID1[3] = b1[4];

                        int commandID = BitConverter.ToInt32(commandID1, 0);

                        switch (mode)
                        {

                            case 0:
                                #region ENDCALLBACK
                                befreieBEFEHL(commandID);
                                #endregion
                                break;

                            case 1:
                                #region PAUSE
                                PauseCommand(commandID);
                                #endregion
                                break;
                            case 2:
                                #region ABBRECHEN
                                AbortCommand(commandID);
                                #endregion
                                break;

                            case 3:
                                #region FORTFÜHREN
                                ContinueCommand(commandID);
                                #endregion
                                break;
                            case 4:
                                #region FORTFÜHRENBEFEHL
                                fortfuehreBEFEHLBEFEHL(commandID);
                                #endregion
                                break;
                            case 5:
                                #region AusDerPausierListeLoeschen

                                Empfaenger b = null;
                                for (int m = 0; m < ReceivePauseList.Count; m++)
                                {
                                    Empfaenger befehl1 = (Empfaenger)ReceivePauseList[m];

                                    if (befehl1.commandID == commandID)
                                    {
                                        b = befehl1;
                                        break;
                                    }


                                }
                                if (b != null)
                                {
                                    OnCommandCanceled(b.commandID, false, b.CustomHeader);
                                    ReceivePauseList.Remove(b);
                                }
                                #endregion
                                break;


                            //if default, smthin must go wrong here!?
                            default:
                                throw new Exception();

                            //break;

                        }

                    }

                }
                else
                {

#warning traffic  wird durchs deserialisieren aufgehalten......
#warning thread.join würd finishedtransfer2
                    //new Thread(() =>
                    //{
                    if (o == null)
                    {
                        o = finishedTransfer2(e);
                    }
#warning fraghaft ob im thread oder außerhalb// ist eigentlich egal // oder??
                    if (e.isEndCallBack)
                        sendEndCallback(e.commandID);


                    if (o != null)
                    {

                        //führe OnReceiveCompletedDataEvent aus
                        OnReceiveCompletedData(o, e.CustomHeader);
                    }


                    //#warning hier vielleich e auf null setzen? am ende..
                    //}).Start();
                }
            }
            catch (Exception eee) { OnException(eee, "finishedTransfer() Error"); }
        }

        internal object finishedTransfer2(Empfaenger e)
        {
            try
            {
                MemoryStream m = e.getData();
                byte[] byData = m.ToArray();

                //addLogMessage(1, "schritt 1");
                switch (encryption)
                {
                    case 0: break;
                    case 1: Encrypter.RC4(ref byData, new System.Text.ASCIIEncoding().GetBytes(currentPassword)); break;

                }
                //addLogMessage(1, "schritt 2");
                switch (compression)
                {
                    case 0: break;
                    case 1: byData = Compressor.gZipDecompress(byData); break;

                }
                //addLogMessage(1, "schritt 3");
                Object o = getObjectWithByteArray(byData);

                //#warning ist es richtig hier garbage collector aufzurufen, villeicht im thread öffnen??, ?? // (leider) notwendig
                System.GC.Collect();
                return o;

            }
            catch (Exception ex)
            {

                OnException(ex, "finishedTransfer2() Error");
                //    //addLogMessage(0, ex.ToString());

                return null;
            }
        }

        internal int generateBefehlsID()
        {
#warning generierungsfehler berücksichtigen
            //public int deliverWrapper(Object obj)
            //{
            //    int BefehlsID = befehlsIDVerwalter.generateBefehlsID();
            //    if (BefehlsID == -2) { return -2; }
            //    else
            //        if (deliver(obj, BefehlsID, (byte)1))
            //        {
            //            return BefehlsID;
            //        }
            //        else
            //        {
            //            return -1;
            //        }

            //    //-1 bedeutet deliver liefert false, wegen nothandshaked yet, wahrscheinlich, oder keine verbindung etabiliert
            //    //-2 bedeutet keine befehlsID frei..
            //}
            try
            {
                return commandIDverwalter1.generateBefehlsID();
            }
            catch (Exception e) { OnException(e, "generateBefehlsID() Error"); return -3; }
        }

        /// <summary>
        /// Starts maually listening for incomming connections.
        /// </summary>
        public void startListening()
        {
            try
            {

#warning neu isInitialized
                if (!isInitialized)
                {
                    isInitialized = true;
                    this.Initialize();                    
                }

                //if (internSocket != null) internSocket = null;
                //// Erzeuge neuen IPv4 Tcp Socket
                //internSocket = new Socket(AddressFamily.InterNetwork,
                //                         SocketType.Stream,
                //                         ProtocolType.Tcp);

                //Exception x = null;
                //for (int i = 0; i < 10; i++)
                //{
                //    try
                //    {
                //        System.Threading.Thread.Sleep(300);

                        // Verwende den angegebenen Port
                if (!internSocket.IsBound)
                        internSocket.Bind(new IPEndPoint(IPAddress.Any, servPort));
                        
                //        x = null;
                //        break;
                //    }
                //    catch (Exception u)
                //    {
                //        x = u;
                //    }
                //}

                //if (x != null)
                //{
                //    throw x;
                //}

                // Starte das Listening
                internSocket.Listen(BACKLOG);

                // Rückruf für jeden Clienten
                internSocket.BeginAccept(new AsyncCallback(AcceptCallback), null);


            }
            catch (Exception e) { OnException(e, "startListening() Error"); }

        }
        
        /// <summary>
        /// Stops maually listening for incomming connections.
        /// </summary>
        public void stopListening()
        {
            try
            {
                if (internSocket != null)
                    internSocket.Close();
                internSocket = null;

            }
            catch (Exception e) { OnException(e, "stopListening() Error"); }
        }

        private void AcceptCallback(IAsyncResult asyncResult)
        {
            //try
            //{
                // Rückruf

                //adde in die sinliste m_socListen.EndAccept(asyncResult);

                if (internSocket == null)
                {



                }
                else
                {
                    
                    //da jetzt connected , wird das listenende socket zerstört, und das connectete übernimmt seine stelle als internSocket
                    Socket tmp = internSocket.EndAccept(asyncResult);                    

                    restartSocket();

                    internSocket = tmp;

                    isSocketConnected = true;
                    isConnected = true;

                    OnBeingConnectedTo();
                }


            //}
            //catch (Exception e) { OnException(e, "AcceptCallback() Error"); }

        }
        
        //Deliver Functions
        #region Deliver Functions

        #region ungenutzt-bisjetzt-Encrypted&Compressed	Bei jeder Sendung entscheiden können
        private int deliverObject(Object ObjectToSend, Byte[] CustomHeader, bool isInterruptAble, bool isWithSendCallBack, bool isWithGetCallBack, bool Reserve, byte compression, byte encryption)
        {
            try
            {
                bool b = false;
                int result = -2;


                if (isConnected)
                {

                    Befehl bf = new Befehl(this, ObjectToSend, CHUNKSIZE, 5, compression, encryption, currentPassword, CustomHeaderLength, CustomHeader, isWithSendCallBack, isWithGetCallBack, isInterruptAble, false);
                    result = bf.commandID;
                    if (!Reserve)
                    {
                        addInSendList(bf);
                    }
                    else
                    {
                        ReserveList.Add(bf);
                    }

                }
                else
                {
                    result = -4;
                }

                return result;



            }
            catch (Exception e) { OnException(e, "deliverObject() Error"); return -3; }
        }
        private int deliverFile(String FilePathHere, String FilePathThere, Byte[] CustomHeader, bool isInterruptAble, bool isWithSendCallBack, bool isWithGetCallBack, bool Reserve, byte compression, byte encryption)
        {
            try
            {

                bool b = false;
                int result = -2;


                if (isConnected)
                {

                    FileBefehl fb = new FileBefehl(this, FilePathHere, FilePathThere, FileCHUNKSIZE, CHUNKSIZE, compression, encryption, currentPassword, CustomHeaderLength, CustomHeader, isWithSendCallBack, isWithGetCallBack, isInterruptAble);
                    result = fb.commandID;
                    if (!Reserve)
                    {
                        addInSendList(fb);
                    }
                    else
                    {
                        ReserveList.Add(fb);
                    }

                }
                else
                {
                    result = -4;
                }

                return result;
            }
            catch (Exception e) { OnException(e, "deliverFile() Error"); return -3; }
        }
        #endregion

        /// <summary>
        /// Sends an Object to the connected LeetSocket
        /// </summary>
        /// <param name="ObjectToSend">The Object that will be sent</param>
        /// <param name="CustomHeader">A Byte array that will be send with the Object and a paramter of the Events</param>
        /// <param name="isInterruptAble">If true, this method returns a unique CommandID, which can be used for Pausing/Continueing/Abording the perding sending, else the command is not interruptable</param>
        /// <param name="isWithSendCallBack">if true, the OnSendChunkEVENT will be invoked, with the corresponding parameters of this sending, like CommandID,sentBytes,TotalLength of data needed to be sent, etc...</param>
        /// <param name="isWithGetCallBack">if true, the OnReceiveChunkEVENT on the receiver side will be invoked, with the corresponding parameters of this sending, like CommandID,receivedBytes,TotalLength of data needed to be received, etc...</param>
        /// <param name="Reserve">Creates a sending command, without starting to send. A reserved command can be later activated with the method: startReserved(int CommandID). This functionality can be used for example, to get the CommandID returned by the send method, without at the same time starting the sending.</param>
        /// <returns>-4 if not Connected, -3 if an error occured, -2 if succesfully started sending and the sending is not interruptable, -1 or greater if succesfully started sending and the sending is interruptable and therefore has an unique CommandID</returns>
        public int sendObject(Object ObjectToSend, Byte[] CustomHeader, bool isInterruptAble, bool isWithSendCallBack, bool isWithGetCallBack, bool Reserve)
        {
            try
            {
                bool b = false;
                int result = -2;


                if (isConnected)
                {

                    Befehl bf = new Befehl(this, ObjectToSend, CHUNKSIZE, 5, compression, encryption, currentPassword, CustomHeaderLength, CustomHeader, isWithSendCallBack, isWithGetCallBack, isInterruptAble, false);
                    result = bf.commandID;
                    if (!Reserve)
                    {
                        addInSendList(bf);
                    }
                    else
                    {
                        ReserveList.Add(bf);
                    }

                }
                else
                {
                    result = -4;
                }

                return result;



            }
            catch (Exception e) { OnException(e, "deliverObject() Error"); return -3; }
        }

        /// <summary>
        /// Sends a File to the connected LeetSocket
        /// </summary>
        /// <param name="FilePathHere">Path of the File that should be send</param>
        /// <param name="FilePathThere">Path where the sent File will be saved to</param>
        /// <param name="CustomHeader">A Byte array that will be send with the Object and a paramter of the Events</param>
        /// <param name="isInterruptAble">If true, this method returns a unique CommandID, which can be used for Pausing/Continueing/Abording the perding sending, else the command is not interruptable</param>
        /// <param name="isWithSendCallBack">if true, the OnSendChunkEVENT will be invoked, with the corresponding parameters of this sending, like CommandID,sentBytes,TotalLength of data needed to be sent, etc...</param>
        /// <param name="isWithGetCallBack">if true, the OnReceiveChunkEVENT on the receiver side will be invoked, with the corresponding parameters of this sending, like CommandID,receivedBytes,TotalLength of data needed to be received, etc...</param>
        /// <param name="Reserve">Creates a sending command, without starting to send. A reserved command can be later activated with the method: startReserved(int CommandID). This functionality can be used for example, to get the CommandID returned by the send method, without at the same time starting the sending.</param>
        /// <returns>-4 if not Connected, -3 if an error occured, -2 if succesfully started sending and the sending is not interruptable, -1 or greater if succesfully started sending and the sending is interruptable and therefore has an unique CommandID</returns>
        public int sendFile(String FilePathHere, String FilePathThere, Byte[] CustomHeader, bool isInterruptAble, bool isWithSendCallBack, bool isWithGetCallBack, bool Reserve)
        {
            try
            {

                bool b = false;
                int result = -2;


                if (isConnected)
                {

                    FileBefehl fb = new FileBefehl(this, FilePathHere, FilePathThere, FileCHUNKSIZE, CHUNKSIZE, compression, encryption, currentPassword, CustomHeaderLength, CustomHeader, isWithSendCallBack, isWithGetCallBack, isInterruptAble);
                    result = fb.commandID;
                    if (!Reserve)
                    {
                        addInSendList(fb);
                    }
                    else
                    {
                        ReserveList.Add(fb);
                    }

                }
                else
                {
                    result = -4;
                }

                return result;
            }
            catch (Exception e) { OnException(e, "deliverFile() Error"); return -3; }
        }

        #region easy-send

        /// <summary>
        /// Sends an Object to the connected LeetSocket. Advanced Info: Interruption, Callbacks and Reserve are false, CustomHeader is empty
        /// </summary>
        /// <param name="ObjectToSend">The Object that will be sent</param>
        /// <returns>-4 if not Connected, -3 if an error occured, -2 if succesfully started sending and the sending is not interruptable, -1 or greater if succesfully started sending and the sending is interruptable and therefore has an unique CommandID</returns>
        public int sendObject(Object ObjectToSend)
        {
            return sendObject(ObjectToSend, null, false, false, false, false);
        }

        /// <summary>
        /// Sends an Object to the connected LeetSocket. Advanced Info: Interruption, Callbacks and Reserve are false
        /// </summary>
        /// <param name="ObjectToSend">The Object that will be sent</param>
        /// <param name="CustomHeader">A Byte array that will be send with the Object and a paramter of the Events</param>
        /// <returns>-4 if not Connected, -3 if an error occured, -2 if succesfully started sending and the sending is not interruptable, -1 or greater if succesfully started sending and the sending is interruptable and therefore has an unique CommandID</returns>
        public int sendObject(Object ObjectToSend, Byte[] CustomHeader)
        {
            return sendObject(ObjectToSend, CustomHeader, false, false, false, false);
        }

        /// <summary>
        /// Sends an Object to the connected LeetSocket. Advanced Info: Reserve is false
        /// </summary>
        /// <param name="ObjectToSend">The Object that will be sent</param>
        /// <param name="CustomHeader">A Byte array that will be send with the Object and a paramter of the Events</param>
        /// <param name="isInterruptAble">If true, this method returns a unique CommandID, which can be used for Pausing/Continueing/Abording the perding sending, else the command is not interruptable</param>
        /// <param name="isWithSendCallBack">if true, the OnSendChunkEVENT will be invoked, with the corresponding parameters of this sending, like CommandID,sentBytes,TotalLength of data needed to be sent, etc...</param>
        /// <param name="isWithGetCallBack">if true, the OnReceiveChunkEVENT on the receiver side will be invoked, with the corresponding parameters of this sending, like CommandID,receivedBytes,TotalLength of data needed to be received, etc...</param>        
        /// <returns>-4 if not Connected, -3 if an error occured, -2 if succesfully started sending and the sending is not interruptable, -1 or greater if succesfully started sending and the sending is interruptable and therefore has an unique CommandID</returns>
        public int sendObject(Object ObjectToSend, Byte[] CustomHeader, bool isInterruptAble, bool isWithSendCallBack, bool isWithGetCallBack)
        {
            return sendObject(ObjectToSend, CustomHeader, isInterruptAble, isWithSendCallBack, isWithGetCallBack, false);
        }

        /// <summary>
        /// Sends an Object to the connected LeetSocket. Advanced Info: Reserve is false, CustomHeader is empty
        /// </summary>
        /// <param name="ObjectToSend">The Object that will be sent</param>        
        /// <param name="isInterruptAble">If true, this method returns a unique CommandID, which can be used for Pausing/Continueing/Abording the perding sending, else the command is not interruptable</param>
        /// <param name="isWithSendCallBack">if true, the OnSendChunkEVENT will be invoked, with the corresponding parameters of this sending, like CommandID,sentBytes,TotalLength of data needed to be sent, etc...</param>
        /// <param name="isWithGetCallBack">if true, the OnReceiveChunkEVENT on the receiver side will be invoked, with the corresponding parameters of this sending, like CommandID,receivedBytes,TotalLength of data needed to be received, etc...</param>        
        /// <returns>-4 if not Connected, -3 if an error occured, -2 if succesfully started sending and the sending is not interruptable, -1 or greater if succesfully started sending and the sending is interruptable and therefore has an unique CommandID</returns>
        public int sendObject(Object ObjectToSend, bool isInterruptAble, bool isWithSendCallBack, bool isWithGetCallBack)
        {
            return sendObject(ObjectToSend, null, isInterruptAble, isWithSendCallBack, isWithGetCallBack, false);
        }
        
       
        /// <summary>
        /// Sends a File to the connected LeetSocket.  Advanced Info: Interruption, Callbacks and Reserve are false, CustomHeader is empty
        /// </summary>
        /// <param name="FilePathHere">Path of the File that should be send</param>
        /// <param name="FilePathThere">Path where the sent File will be saved to</param>
        /// <returns>-4 if not Connected, -3 if an error occured, -2 if succesfully started sending and the sending is not interruptable, -1 or greater if succesfully started sending and the sending is interruptable and therefore has an unique CommandID</returns>
        public int sendFile(String FilePathHere, String FilePathThere)
        {
            return sendFile(FilePathHere, FilePathThere, null, false, false, false, false);
        }

        /// <summary>
        /// Sends a File to the connected LeetSocket. Advanced Info: Reserve is false
        /// </summary>
        /// <param name="FilePathHere">Path of the File that should be send</param>
        /// <param name="FilePathThere">Path where the sent File will be saved to</param>
        /// <param name="CustomHeader">A Byte array that will be send with the Object and a paramter of the Events</param>
        /// <param name="isInterruptAble">If true, this method returns a unique CommandID, which can be used for Pausing/Continueing/Abording the perding sending, else the command is not interruptable</param>
        /// <param name="isWithSendCallBack">if true, the OnSendChunkEVENT will be invoked, with the corresponding parameters of this sending, like CommandID,sentBytes,TotalLength of data needed to be sent, etc...</param>
        /// <param name="isWithGetCallBack">if true, the OnReceiveChunkEVENT on the receiver side will be invoked, with the corresponding parameters of this sending, like CommandID,receivedBytes,TotalLength of data needed to be received, etc...</param>        
        /// <returns>-4 if not Connected, -3 if an error occured, -2 if succesfully started sending and the sending is not interruptable, -1 or greater if succesfully started sending and the sending is interruptable and therefore has an unique CommandID</returns>
        public int sendFile(String FilePathHere, String FilePathThere,Byte[] CustomHeader, bool isInterruptAble, bool isWithSendCallBack, bool isWithGetCallBack)
        {
            return sendFile(FilePathHere, FilePathThere, CustomHeader, isInterruptAble, isWithSendCallBack, isWithGetCallBack, false);
        }

        /// <summary>
        /// Sends a File to the connected LeetSocket. Advanced Info: Reserve is false, CustomHeader is empty
        /// </summary>
        /// <param name="FilePathHere">Path of the File that should be send</param>
        /// <param name="FilePathThere">Path where the sent File will be saved to</param>        
        /// <param name="isInterruptAble">If true, this method returns a unique CommandID, which can be used for Pausing/Continueing/Abording the perding sending, else the command is not interruptable</param>
        /// <param name="isWithSendCallBack">if true, the OnSendChunkEVENT will be invoked, with the corresponding parameters of this sending, like CommandID,sentBytes,TotalLength of data needed to be sent, etc...</param>
        /// <param name="isWithGetCallBack">if true, the OnReceiveChunkEVENT on the receiver side will be invoked, with the corresponding parameters of this sending, like CommandID,receivedBytes,TotalLength of data needed to be received, etc...</param>        
        /// <returns>-4 if not Connected, -3 if an error occured, -2 if succesfully started sending and the sending is not interruptable, -1 or greater if succesfully started sending and the sending is interruptable and therefore has an unique CommandID</returns>
        public int sendFile(String FilePathHere, String FilePathThere, bool isInterruptAble, bool isWithSendCallBack, bool isWithGetCallBack)
        {
            return sendFile(FilePathHere, FilePathThere, null, isInterruptAble, isWithSendCallBack, isWithGetCallBack, false);
        }





        #endregion
        public bool deliverHeartBeat()
        {

            bool result = false;


            if (internSocket.Connected)
            {
                //früher stand hier sendlist.add ...
                addInSendList(new Befehl(this, 0));
                result = true;
            }


            return result;
        }

        private bool deliverSpecialCommand(Object obj)
        {

            bool result = false;


            if (internSocket.Connected)
            {

                //früher stand hier sendlist.add ...
                addInSendList(new Befehl(this, obj, CHUNKSIZE, 5, compression, encryption, currentPassword, 0, null, false,false,false,true));
                result = true;
            }


            return result;
        }

        #endregion
               
        private void clearLists()
        {
            try
            {
                SendList.Clear();
                ReceiveList.Clear();
                ReceivePauseList.Clear();
                SendPauseList.Clear();
#warning reserviertliste leeren wir nicht, nicht nötig
                //ReserveList.Clear();
            }
            catch (Exception e) { OnException(e, "clearLists() Error"); }

        }

        private void permanentSend_Tick(object state)
        {
            PermanentSendTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);

            try
            {
                    if (sendingEnded && (SendList.Count > 0))
                    {                       
                        sendingEnded = false;

                        while (SendList.Count != 0){
                            //repeated in this timer/thread, until nothing is left in the sendinglist
                            #region sendinglist handling(sending and removing commands from list)
                            try
                            {                                
                                    //get currentbefehl
                                    Befehl cmd = (Befehl)SendList[sendingVar];

                                    if (cmd.isDead){
                                        //dead command, remove from list, and go back to start
                                        #region dead command
                                        SendList.RemoveAt(sendingVar);                                       
                                        if (sendingVar == SendList.Count) sendingVar = 0;
                                        //sendStarternIntern();
                                        #endregion

                                    }
                                    else
                                    {
                                        //unfinished command delivery continueos or starts
                                        #region sending procedure
                                        tempCHUNK = cmd.getChunk();
                                            anzahlDerZuSendendenBytes = tempCHUNK.Length;

                                            sendingVar++;
                                            if (sendingVar == SendList.Count) sendingVar = 0;

                                            // an dieser stelle wird gesendet
                                            int sentBytes = 0;

                                            while (anzahlDerZuSendendenBytes != 0)
                                            {
                                                sentBytes += internSocket.Send(tempCHUNK, sentBytes, anzahlDerZuSendendenBytes, SocketFlags.None);
                                                anzahlDerZuSendendenBytes -= sentBytes;
                                            }                                            
                                                          
                                            //event aufruf für gesendete databytes (not including underlieing system traffic,like special commands) 
                                            if (cmd.downloadStatus > 4){  
                                            OnSendData_GetIRX(sentBytes);
                                            }

                                            //falls callback gewünscht, wird hier dafür gesorgt
                                            #region isSendCallBack

                                            if (cmd.isSendCallBack)
                                            {
                                                if (cmd.downloadStatus ==5)
                                                {
                                                    cmd.gesendeteChunks++;
                                                    int anzahlderbytesohneheader = sentBytes - cmd.CurrentHeaderLenght;
                                                    bool tmpIsDead = cmd.isDead;
                                                    UInt64 gesamtLaenge = cmd.BefehlLength;

                                                    ulong sentBytesAddition=0;
                                                    #warning unchecked(corection) calculation of the value SentBytesAddition
                                                    if (cmd.isDead)
                                                    {
                                                        //if (cmd.gesendeteChunks == 1) { sentBytesAddition = Convert.ToUInt64(anzahlderbytesohneheader); }
                                                        //else
                                                        //{
                                                        //    sentBytesAddition = Convert.ToUInt64((((cmd.gesendeteChunks - 1) * CHUNKSIZE) + anzahlderbytesohneheader));
                                                        //}

                                                        //all die calculation is nich nötig, da dead, ist fertig, also die gesamtlaenge erreicht:
                                                        sentBytesAddition = gesamtLaenge;
                                                    }
                                                    else {
                                                        sentBytesAddition = Convert.ToUInt64((((cmd.gesendeteChunks) * CHUNKSIZE) ));
                                                    }

                                                    
                                                    OnSendChunk(cmd.CustomHeader, gesamtLaenge, anzahlderbytesohneheader, sentBytesAddition, tmpIsDead, cmd.commandID);
                                                }
                                            }
                                            #endregion

                                            tempCHUNK = null;
                                            anzahlDerZuSendendenBytes = 0;
                                        #endregion

                                    }
                               

                            }
                            catch (Exception e)
                            {
                                OnException(e, "sendStarternIntern() Error");

                                OnDisconnect();
                                isConnected = false;
                                isSocketConnected = false;
                            }
#endregion 
                        
                        }

                        sendingVar = 0;
                        sendingEnded = true;
                    }
            }
            catch (Exception e) { OnException(e, "permanentSendStarter() Error"); }
#warning sicher das es auf 1 sein sollte ? CPU ???
            PermanentSendTimer.Change(5, System.Threading.Timeout.Infinite);
        }

        private void clearEverything()
        {
            clearLists();
            s = new MemoryStream();

            isConnected = false;
            restartSocket();
            current = 0;


            isLast = false;
            isStarter = false;
            //isHeartbeat = false;
            isSpecialCommand = false;
            isFile = false;
            isF_ = false;
            //isC_F_ = false;
            isEndCallback = false;
            isCallback = false;
            received = -1;
            ExtraBytes = 0;

            tempCHUNK = null;
            anzahlDerZuSendendenBytes = 0;
            indexOfCurrentBefehl = 0;

            schleife = true;
            isSocketConnected = false;
            sendingEnded = true;
        }

        private void AutoConnectTimer_Tick(object state)
        {
            AutoConnectTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);            

#warning neu isInitialized
            if (!isInitialized)
            {
                isInitialized = true;
                this.Initialize();
            }

            try
            {
               // MessageBox.Show("tick: " + autoConTry.ToString());

                //if (!autoConTry)
                //{
                    if (!internSocket.Connected)
                    {
                        // autoConTry = true;
                        // new Thread(() =>
                        {

                            //connec trys begin:


                            //OnAutoConnectTriesBeginEVENT  hierausführen
                            OnAutoConnectTriesBegin();

//                            while (!isConnected)
                            if (!isConnected)
                            {
                                if ((AdressList != null) && (AdressList.Count > 0))
                                {
                                    foreach (Object[] adress in AdressList)
                                    {
                                        if (!isConnected)
                                        {
                                            String ip = (String)adress[0];
                                            int port = (int)adress[1];
                                            IPAddress servIPAddress = (IPAddress)Dns.GetHostAddresses(ip)[0];

                                            //IPAddress servIPAddress = IPAddress.Parse(server);
                                            servEndPoint = new IPEndPoint(servIPAddress, port);

                                            ConnectE();
                                            //System.Threading.Thread.Sleep(1500);
                                        }
                                    }
                                }
                                else
                                {                                    
                                    ConnectE();
                                }
//                                System.Threading.Thread.Sleep(this.AutoConnectInterval);
                            }
                            // da wir jetzt connected sind gehts los mit dem receivenn, nihahaha

#warning WaitForDatamuss im gui thread sein ???
                            //WaitForData(internSocket);//auskommentiert.. dem user im onconnected event listenen lassen

                            //Thread tmp = connectThread;

                            //connectThread = null;
                            //connectThread.

                            //OnConnectedEVENT  hierausführen
                            if (internSocket.Connected)
                            {
                                isSocketConnected = true;
                                isConnected = true;
                                OnConnected();
                            }

                        }
                        // ).Start();

                    }
                // }
            }
            catch (Exception te) { OnException(te, "AutoConnectTimer_Tick() Error"); }

            AutoConnectTimer.Change(this.AutoConnectInterval, System.Threading.Timeout.Infinite);
        }

        private void HeartBeatTimer_Tick(object state)
        {
            HeartBeatTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
            try
            {
                // isSocketConnected = (Environment.TickCount - lastContact) < TIMEOUT;

                // addLogMessage(0, isSocketConnected.ToString() + " " + Environment.TickCount.ToString() + " " + lastContact.ToString() + " " + (Environment.TickCount - lastContact).ToString());

                if (isSocketConnected)
                    deliverHeartBeat();
                OnHeartBeatSent();

            }
            catch (Exception e4) { OnException(e4, "HeartBeatTimer_Tick() Error"); }


            HeartBeatTimer.Change(this.HeartBeatSystemInterval, 1);

        }

//        private class OwnTimer : System.Timers.Timer
//        {
//            public OwnTimer(int interval, bool enabled)
//            {
//                this.AutoReset = true;
//                this.Interval = (double)interval;
//                this.Enabled = enabled;
//#warning achtung
//                // this.Start();
//            }

//        }

        protected override void Dispose(bool disposing)
        {
            try
            {
                try
                {
                    AutoListening = false;
                    AutoConnectSystem = false;
                    HeartBeatSystem = false;
                    }
                catch (Exception) { }
                try
                {

                    if (AutoConnectTimer != null)
                    {
                        AutoConnectTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                        AutoConnectTimer.Dispose();
                    }

                    if (HeartBeatTimer != null)
                    {
                        HeartBeatTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                        HeartBeatTimer.Dispose();
                    }

                    if (PermanentSendTimer != null)
                    {
                        PermanentSendTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                        PermanentSendTimer.Dispose();
                    }

                    if (WaitForDataTimer != null)
                    {
                        WaitForDataTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
                        WaitForDataTimer.Dispose();
                    }



                
                }
                catch (Exception) { }


                try
                {
                    internSocket.Disconnect(false);
                }
                catch (Exception) { }
                try
                {
                    internSocket.Shutdown(SocketShutdown.Both);
                }
                catch (Exception) { }
                try
                {
                    internSocket.Close();
                }
                catch (Exception) { }
            }
            catch (Exception) { }

            base.Dispose(disposing);

        }

        //public void DisposeEx()
        //{
        //    try
        //    {
        //        try
        //        {
        //            AutoConnectTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
        //            AutoConnectTimer.Dispose();
        //            HeartBeatTimer.Change(System.Threading.Timeout.Infinite, System.Threading.Timeout.Infinite);
        //            HeartBeatTimer.Dispose();

        //            //if (!(connectThread == null))
        //            //    connectThread.Abort();

        //            //connectThread = null;
        //        }
        //        catch (Exception) { }

        //        try
        //        {
        //            internSocket.Disconnect(false);
        //        }
        //        catch (Exception) { }
        //        try
        //        {
        //            internSocket.Shutdown(SocketShutdown.Both);
        //        }
        //        catch (Exception) { }
        //        try
        //        {
        //            internSocket.Close();
        //        }
        //        catch (Exception) { }
        //    }
        //    catch (Exception) { }

        //    base.Dispose(true);

        //}

        //private bool isConnected2(Socket sck)
        //{
        //    // This is how you can determine whether a socket is still connected.
        //    bool blockingState = sck.Blocking;
        //    bool result = false;
        //    try
        //    {
        //        byte[] tmp = new byte[1];

        //        sck.Blocking = false;
        //        sck.Send(tmp, 0, 0);
        //        result = true;
        //        // Console.WriteLine("Connected!");
        //    }
        //    catch (SocketException)
        //    {
        //        // 10035 == WSAEWOULDBLOCK
        //        /*
        //        if (e.NativeErrorCode.Equals(10035))
        //            Console.WriteLine("Still Connected, but the Send would block");
        //        else
        //        {
        //            Console.WriteLine("Disconnected: error code {0}!", e.NativeErrorCode);
        //        }
        //         */
        //    }
        //    finally
        //    {
        //        sck.Blocking = blockingState;
        //    }

        //    return result;
        //    // Console.WriteLine("Connected: {0}", sck.Connected);
        //}
    }
}
