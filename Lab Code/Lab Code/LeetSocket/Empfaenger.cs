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

using System.Windows.Forms;

namespace LeetSocket
{
    public class Empfaenger
    {

        public short lengthOfLastChunk;
        public byte Downloadstatus = 1;
        public MemoryStream buffer = new MemoryStream();
        public byte[] CustomHeader;
        public UInt64 BefehlLength;
        
        int ExtraBytes = 0;
        public long tempBufferLength = 0;
        public bool isGetCallBack;
        public bool isEndCallBack = false;
        public bool isSpecialCommand = false;
        
        
        public int commandID = -1;


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

        public Empfaenger() { }
        public Empfaenger(short lengthOfLastChunk, byte[] data, int CustomHeaderLength, bool isGetCallBack,bool isEndCallBack,bool isSpecialCommand) // lol
        {
            this.lengthOfLastChunk = lengthOfLastChunk;
            this.isGetCallBack = isGetCallBack;
            this.isEndCallBack = isEndCallBack;
            this.isSpecialCommand = isSpecialCommand;


            CustomHeader = new byte[CustomHeaderLength];

            for (int i = 0; i < CustomHeaderLength; i++)
            {
                CustomHeader[i] = data[i + 3];
            }

            if (isGetCallBack)
            {
                ExtraBytes = 5;


                byte[] tempB = new byte[5];
                for (int i = 0; i < 5; i++)
                {
                    tempB[i] = data[i + 3 + CustomHeaderLength];
                    BefehlLength = getUInt64(tempB);

                }


            }
            if (isEndCallBack) {

                
                byte[] tempB1 = new byte[4];
                for (int i = 0; i < 4; i++)
                {
                    tempB1[i] = data[i + 3 + CustomHeaderLength + ExtraBytes];

                    commandID = BitConverter.ToInt32(tempB1,0);

                }
                ExtraBytes += 4;
            }



            int temp1 = 0;
            if (!isSpecialCommand) temp1 = CustomHeaderLength;


            buffer.Write(data, (3 + temp1 + ExtraBytes), data.Length - (3 + temp1 + ExtraBytes));
        }

        public virtual void addChunk(byte[] data)
        {
            tempBufferLength = buffer.Length;
            buffer.Write(data, 1, data.Length - 1);

        }

        public MemoryStream getData()
        {

            buffer.Position = 0;


            return buffer;
        }
    }
}
