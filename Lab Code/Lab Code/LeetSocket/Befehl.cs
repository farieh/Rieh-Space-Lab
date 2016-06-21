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
using System.Text;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using System.Windows.Forms;
using System.Threading;

namespace LeetSocket
{
    class Befehl
    {
        private int CHUNKSIZE;
        bool isFirst = true;
        
        public byte downloadStatus = 5;
        int index = 0;
        public Boolean isDead = false;
        byte[] byData;
        byte compression;
        byte encryption;
       
        public UInt64 BefehlLength;
        public int commandID = -2;
        LeetSocket main;

        public bool isGetCallBack = false;
        public bool isEndCallback = false;
        public bool isSendCallBack = false;
        

        public byte typ1 = 4;
        public byte typ2 = 5;
        public byte typ3 = 3;
        public byte typ4 = 6;

        public int CurrentHeaderLenght = 0;

        //progress kram
        public int gesendeteChunks = 0;       

        short LastChunkLength;

        public byte[] CustomHeader;

        int CustomHeaderLength;

        #region BefehlLengthConverter
        public byte[] getBytes(UInt64 n)
        {
            byte[] a = BitConverter.GetBytes(n);

            byte[] k = new byte[5];
            for (int i = 0; i < 5; i++)
                k[i] = a[i];

            return k;
        }

        public UInt64 getUInt64(byte[] k)
        {
            byte[] p = new byte[8];

            for (int i = 0; i < 5; i++)
                p[i] = k[i];

            return BitConverter.ToUInt64(p, 0);
        }
        #endregion

        public byte[] getByteArrayWithObject(Object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf1 = new BinaryFormatter();
            bf1.Serialize(ms, o);
            bf1 = null;
            return ms.ToArray();
        }

        public byte[] addByteInFront(byte[] ba, byte b)
        {
            byte[] tmp = new byte[ba.Length + 1];
            tmp[0] = b;
            ba.CopyTo(tmp, 1);
            return tmp;
        }

        public Befehl() { }
        public Befehl(LeetSocket main, byte downloadStatus)
        {
        this.main = main;
        this.downloadStatus = downloadStatus; }
        
        public Befehl(LeetSocket main, Object obj, int Chunksizee, byte dStatus, byte compression, byte encryption, String password, int CustomHeaderLength, byte[] CustomHeader1, bool isSendCallBack, bool isGetCallback, bool isEndCallback, bool isSpecialCommand)
        {
            this.isSendCallBack = isSendCallBack;
            this.isGetCallBack = isGetCallback;
            this.isEndCallback = isEndCallback;

            //wichtig
            #region additional magicbyte setting
            if (isSpecialCommand)
            {
                typ1 = 21;
            }
            if (isEndCallback && isGetCallback)
            {
                commandID = main.generateBefehlsID();
                //magic byte änderung
                typ1 = 17;
                typ2 = 18;
            }
            else
            {
                if (isEndCallback)
                {
                    commandID = main.generateBefehlsID();
                    //magic byte änderung
                    typ1 = 15;
                    typ2 = 16;
                }
                if (isGetCallback)
                {
                    //magic byte änderung
                    typ1 = 13;
                    typ2 = 14;
                }
            }

            #endregion

            CHUNKSIZE = Chunksizee;
            

            this.main = main;
            this.downloadStatus = dStatus;
            this.compression = compression;
            this.encryption = encryption;
            this.CustomHeaderLength = CustomHeaderLength;


            if (CustomHeaderLength == 0)
            {
                CustomHeader = null;

            }
            else
            {
                if (CustomHeader1 == null)
                {
                    CustomHeader = new byte[CustomHeaderLength];

                }
                else
                {
                    CustomHeader = CustomHeader1;

                }

            }

            //heartbeat z.b.
            if (obj == null) return;



            byData = getByteArrayWithObject(obj);

            //if (TypID == 1)
            //    MessageBox.Show("");
            System.GC.Collect();
            switch (compression)
            {
                case 0: break;
                case 1: byData = Compressor.gZipCompress(byData); break;

            }
            switch (encryption)
            {
                case 0: break;
                case 1: Encrypter.RC4(ref byData, new System.Text.ASCIIEncoding().GetBytes(password)); break;

            }


            LastChunkLength = (short)(byData.Length % CHUNKSIZE);


            if (LastChunkLength == 0)

                LastChunkLength = (short)CHUNKSIZE;


            BefehlLength = (UInt64)byData.Length;

        }
        
        public virtual byte[] getChunk()
        {
            //###############################
            //## Teilung in Chunks         ##
            //###############################

            
            CurrentHeaderLenght = 0;

            byte[] result;
            int a;

            if ((downloadStatus == 5))
            {

                if (isFirst)
                {
                    // MessageBox.Show("isFirst");
                    if ((byData.Length - index) == LastChunkLength)
                    {
                        //isSingleChunk = true;

                        //  MessageBox.Show("isFirst und islast");
                        //        byData.Length - index

                        //a = 220 + downloadStatus;
                        a = typ1;


                        CurrentHeaderLenght = 3 + CustomHeaderLength;


                        if (isGetCallBack) CurrentHeaderLenght += 5;
                        if (isEndCallback) CurrentHeaderLenght += 4;

                        result = new byte[byData.Length - index + CurrentHeaderLenght];

                        byte[] tmp;
                        tmp = BitConverter.GetBytes(LastChunkLength);
                        result[0] = (byte)a;
                        result[1] = tmp[0];
                        result[2] = tmp[1];


                        //adden des CustomHeaders
                        for (int m = 0; m < CustomHeaderLength; m++)
                        {
                            result[m + 3] = CustomHeader[m];

                        }

                        //adden der 5 bytes
                        if (isGetCallBack)
                        {
                            byte[] tempB = getBytes(BefehlLength);

                            for (int j = 0; j < 5; j++)
                            {
                                result[j + CustomHeaderLength + 3] = tempB[j];
                            }
                        }

                        //adden der 4 bytes
                        if (isEndCallback)
                        {
                            byte[] tempB = BitConverter.GetBytes(commandID);

                            for (int j = 0; j < 4; j++)
                            {
                                result[j + CurrentHeaderLenght-4] = tempB[j];
                            }
                        }

                        System.Buffer.BlockCopy(byData, index, result, CurrentHeaderLenght, result.Length - (CurrentHeaderLenght));
                        index += (LastChunkLength);

                    }
                    else
                    {
                        //isStarterChunk = true;

                        CurrentHeaderLenght = 3 + CustomHeaderLength;
                        if (isGetCallBack) CurrentHeaderLenght += 5;
                        if (isEndCallback) CurrentHeaderLenght += 4;
                        //a = 120 + downloadStatus;
                        a = typ2;



                        // chunksize
                        result = new byte[CHUNKSIZE + CurrentHeaderLenght];

                        byte[] tmp;
                        tmp = BitConverter.GetBytes(LastChunkLength);
                        result[0] = (byte)a;
                        result[1] = tmp[0];
                        result[2] = tmp[1];

                        //adden des CustomHeaders
                        for (int m = 0; m < CustomHeaderLength; m++)
                        {
                            result[m + 3] = CustomHeader[m];

                        }

                        //adden der 5 bytes
                        if (isGetCallBack)
                        {
                            byte[] tempB = getBytes(BefehlLength);

                            for (int j = 0; j < 5; j++)
                            {
                                result[j + CustomHeaderLength + 3] = tempB[j];
                            }


                        }

                        //adden der 4 bytes
                        if (isEndCallback)
                        {
                            byte[] tempB = BitConverter.GetBytes(commandID);

                            for (int j = 0; j < 4; j++)
                            {
                                result[j + CurrentHeaderLenght - 4] = tempB[j];
                            }
                        }


                        System.Buffer.BlockCopy(byData, index, result, CurrentHeaderLenght, result.Length - (CurrentHeaderLenght));
                        index += CHUNKSIZE;
                    }
                    isFirst = false;
                }
                else
                {
                    //    MessageBox.Show("else");
                    if ((byData.Length - index) == LastChunkLength)
                    {
                        //isLastChunk = true;

                        result = new byte[byData.Length - index + 1];
                        //result[0] = (byte)(210 + downloadStatus);
                        result[0] = typ3;

                        CurrentHeaderLenght = 1;
                        //if (isGetCallBack) CurrentHeaderLenght += 5;



                        System.Buffer.BlockCopy(byData, index, result, 1, result.Length - 1);
                        index += LastChunkLength;
                    }
                    else
                    {
                        //isNormalChunk = true;

                        CurrentHeaderLenght = 1;
                        //if (isGetCallBack) CurrentHeaderLenght += 5;

                        result = new byte[CHUNKSIZE + 1];
                        //result[0] = (byte)(110 + downloadStatus);
                        result[0] = typ4;



                        System.Buffer.BlockCopy(byData, index, result, 1, result.Length - 1);
                        index += CHUNKSIZE;
                    }
                }


                isDead = (index == byData.Length);
                //  MessageBox.Show("index: "+index.ToString()+" length: " + byData.Length.ToString() );

                if (isDead) byData = null;
                // if (isDead) MessageBox.Show("isDead");



            }
            else
            {
                isDead = true;                
                if (downloadStatus == 0)
                {
                    result = new byte[] { (byte)0 };


                }
                else
                {
                    if (downloadStatus == 1)
                    {
                        result = new byte[] { (byte)1 };
                        main.SendPauseList.Add(this);
                        main.OnCommandPaused(commandID, true, CustomHeader);
                    }
                    else
                    {
                        if (downloadStatus == 2)
                        {
                            result = new byte[] { (byte)2 };
                            main.OnCommandCanceled(commandID, true, CustomHeader);
                        }
                        else result = new byte[] { (byte)2 };


                    }
                }
            }
            
            return result;
        }

    }

}
