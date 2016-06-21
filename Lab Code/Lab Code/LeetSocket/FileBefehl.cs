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
using System.Collections;
using System.Windows.Forms;

namespace LeetSocket
{


    class FileBefehl : Befehl
    {

        String FilePathHere;
        String FilePathThere;
        int FileChunkSize;
        FileSplitter fileSplitter1;
        Befehl CurrentBefehl;


        bool withGetCallBack = false;
        bool withProgressCallBack = false;
        byte compression;
        byte encryption;
        int Chunksize;
        String password;
        int CustomHeaderLength;


        bool firstFileChunk = true;
        LeetSocket main;

        int tempFileChunkSize = 0;


        //sinn#?=
        //String mtheory = "693\n";

        public FileBefehl(LeetSocket main, String FilePathHere, String FilePathThere, int FileChunkSize, int Chunksize, byte compression, byte encryption, String password, int CustomHeaderLength, byte[] CustomHeader1, bool isSendCallBack, bool isGetcallback, bool isEndCallback)
        {
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

            this.main = main;
            this.FilePathHere = FilePathHere;
            this.FilePathThere = FilePathThere;
            this.FileChunkSize = FileChunkSize;
            fileSplitter1 = new FileSplitter(FilePathHere, FileChunkSize);

            this.BefehlLength = Convert.ToUInt64(new FileInfo(FilePathHere).Length);

            this.compression = compression;
            this.encryption = encryption;
            this.Chunksize = Chunksize;
            this.password = password;
            this.CustomHeaderLength = CustomHeaderLength;            
            this.isEndCallback = isEndCallback;

            withGetCallBack = isGetcallback;
            withProgressCallBack = isSendCallBack;

            if (isEndCallback)
                this.commandID = main.generateBefehlsID();

            //first befehls gets inited
            updateCurrentBefehl(getHeader());



        }

        public byte[] getHeader()
        {
            #region müll
            //if (withProgressCallBack && withGetCallBack)
            //{
            //    //CurrentBefehl=new Befehl(nnn,Chunksize,1,compression,encryption,password,0,null,)
            //}
            //else
            //{
            //    if (withProgressCallBack && withGetCallBack)
            //    {
            //        //CurrentBefehl=new
            //    }
            //    else
            //    {
            //        if (withProgressCallBack)
            //        {
            //            //CurrentBefehl=new
            //        }
            //        else { 
            //        //CurrentBefehl=new
            //        }
            //    }
            //}
            #endregion
            byte[] header;
            if (isEndCallback)
            {
                header = new byte[1 + 5 + CustomHeaderLength + 4];
            }
            else { header = new byte[1 + 5 + CustomHeaderLength]; }

            byte magicbyte;
            byte[] laengeAsArray = this.getBytes(BefehlLength);

            if (withGetCallBack)
            {
                magicbyte = 1;
            }
            else
            {
                magicbyte = 0;
            }
            header[0] = magicbyte;

            header[1] = laengeAsArray[0];
            header[2] = laengeAsArray[1];
            header[3] = laengeAsArray[2];
            header[4] = laengeAsArray[3];
            header[5] = laengeAsArray[4];


            for (int z = 0; z < CustomHeaderLength; z++)
            {
                header[z + 1 + 5] = CustomHeader[z];
            }

            if (isEndCallback)
            {
                byte[] tempb1 = BitConverter.GetBytes(commandID);

                for (int h = 0; h < 4; h++)
                {
                    header[h + 1 + 5 + CustomHeaderLength] = tempb1[h];
                }
            }


            return header;

        }

        public void updateCurrentBefehl(byte[] customHed)
        {


            byte[] tempB = fileSplitter1.getFileChunk();
            tempFileChunkSize = tempB.Length;

            object a = tempB;

            if (this.firstFileChunk)
            {


                a = new object[] { FilePathThere, tempB };
            }

            int i = 0;
            if (customHed != null) i = customHed.Length;
            CurrentBefehl = new Befehl(main, a, Chunksize, 5, compression, encryption, password, i, customHed,false,false,false,false);

            if (this.firstFileChunk)
            {
                firstFileChunk = false;
                if (this.isEndCallback)
                {
                    CurrentBefehl.typ1 = 19;
                    CurrentBefehl.typ2 = 20;
                    CurrentBefehl.typ3 = 7;
                    CurrentBefehl.typ4 = 10;
                }
                else
                {
                    CurrentBefehl.typ1 = 11;
                    CurrentBefehl.typ2 = 12;
                    CurrentBefehl.typ3 = 7;
                    CurrentBefehl.typ4 = 10;
                }
            }
            else
            {

                CurrentBefehl.typ1 = 8;
                CurrentBefehl.typ2 = 9;
                CurrentBefehl.typ3 = 7;
                CurrentBefehl.typ4 = 10;

            }

        }

        public override byte[] getChunk()
        {

            byte[] result;

            if (downloadStatus == 5)
            {
                result = CurrentBefehl.getChunk();
                #region CurrentBefehl is Dead
                if (CurrentBefehl.isDead)
                {


                    if (withProgressCallBack)
                    {
                        #warning calculate Sentbytes Additon
                        ulong sentBytesAddition = 0;
                        sentBytesAddition = Convert.ToUInt64(fileSplitter1.FilePosition);
                        if (fileSplitter1.isDone)
                        {
                            sentBytesAddition=BefehlLength;

                            try
                            {
                                fileSplitter1.DisposeInternStuff();
                            }
                            catch (Exception e1) { main.OnException(e1, "fileSplitter1.DisposeIntern , commandID: " + commandID); }

                        }                       
                        
                        main.OnSendChunk(CustomHeader, BefehlLength, tempFileChunkSize, sentBytesAddition, fileSplitter1.isDone, commandID);
                        

                    }

                    if (fileSplitter1.isDone)
                    {
                        this.isDead = true;
                        if (!fileSplitter1.isDisposed)
                        {
                            try
                            {
                                fileSplitter1.DisposeInternStuff();
                            }
                            catch (Exception e1) { main.OnException(e1, "fileSplitter1.DisposeIntern , commandID: " + commandID); }
                        }

                    }

                    else
                    {

                        updateCurrentBefehl(null);

                    }
                }
                #endregion

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
