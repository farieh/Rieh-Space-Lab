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
using System.Windows.Forms;
using System.Text;

namespace LeetSocket
{
    class FileEmpfaenger : Empfaenger
    {
        Empfaenger innerEmp;
        public bool isSingle = false;
        bool isWithGetCallBack = false;
        UInt64 fullLengthOfTheFile;
        UInt64 countData = 0;
        LeetSocket main;
        bool firstTimeDone = false;
        String FilePathThere;
        public bool isDead = false;
        bool isEndCallback = false;
        
        delegate void UpdateStatusDelegate(byte[] m, String message);

        FileUtilsV3 fileUtil;
  
       


        public FileEmpfaenger(LeetSocket main, short lengthOfLastChunk, byte[] data, int NormalCustomHeaderLength, bool isEndCallback,int commandID)
        {
            this.main = main;
            this.isEndCallback = isEndCallback;
            this.commandID = commandID;
            


            //////////////////// GETTEN DER DATEN /////////////////////////
            //1. starter oder single, magicbyte also
            //2. und 3. byte lenghtofthelastchnunk
            //4. mit getcallback or not
            // 5. 6. 7. 8. 9. length of fullfile
            // 10. - X. CustomHeader


            isSingle = (data[0] == 11) || (data[0] == 19);
            isWithGetCallBack = data[3] == 1;


            byte[] fullLengthOfTheFileAsArray = new byte[5];
            fullLengthOfTheFileAsArray[0] = data[4];
            fullLengthOfTheFileAsArray[1] = data[5];
            fullLengthOfTheFileAsArray[2] = data[6];
            fullLengthOfTheFileAsArray[3] = data[7];
            fullLengthOfTheFileAsArray[4] = data[8];

            fullLengthOfTheFile = getUInt64(fullLengthOfTheFileAsArray);

            this.lengthOfLastChunk = lengthOfLastChunk;

            CustomHeader = new byte[NormalCustomHeaderLength];
            for (int k = 0; k < NormalCustomHeaderLength; k++)
            {
                CustomHeader[k] = data[9 + k];

            }

            int extraBytes = 0;
            if (isEndCallback) extraBytes = 4;

            innerEmp = new Empfaenger(lengthOfLastChunk, data, NormalCustomHeaderLength + 6 + extraBytes, false,false,false);



            if (isSingle)
            {
                object[] objArray = (object[])main.finishedTransfer2(innerEmp);

                FilePathThere = (String)objArray[0];
                byte[] filePart = (byte[])objArray[1];

                int i = filePart.Length;
                countData += System.Convert.ToUInt64(i);

                //MessageBox.Show("ww");

                //main.main.testCounter++;
                //main.main.UpdateTextbox1(" " + main.main.testCounter.ToString());
                //main.main.UpdateTextbox1("0");

#warning kontruktor könnte exceptions entstehn lassen
                fileUtil = new FileUtilsV3(FilePathThere,true);

                try
                {
                    fileUtil.WriteBytes(filePart);
                }
                catch (Exception e)
                {
                    main.OnException(e, "filePart appending exception");
                }
                //main.main.UpdateTextbox1(main.main.testCounter.ToString() + " ");
                //main.main.UpdateTextbox1("1");
                //MessageBox.Show("wwsolltedone sein");

                if (isWithGetCallBack)
                    callBack(i);

                if (countData == fullLengthOfTheFile)
                    endcallback();


            }

            //main.finishedTransfer2(innerEmp);



        }

        public override void addChunk(byte[] data)
        {



            //if (firstTimeDone)
            //{


            switch (data[0])
            {

                //F_LASTCHUNK		7
                case 7:
                    if (!firstTimeDone)
                    {
                        //STARTER WIRD VERVOLLSTÄNDIGT UND FILEpATH WIRD ERMITTELT
                        innerEmp.addChunk(data);
                        firstTimeDone = true;

                        object o = main.finishedTransfer2(innerEmp);
                        object[] objArray = (object[])o;

                        FilePathThere = (String)objArray[0];
                        byte[] filePart = (byte[])objArray[1];

                        int i = filePart.Length;
                        countData += System.Convert.ToUInt64(i);

                        //main.main.testCounter++;
                        //main.main.UpdateTextbox1(" "+main.main.testCounter.ToString());
                        //main.main.UpdateTextbox1("0");
#warning kontruktor könnte exceptions entstehn lassen
                        fileUtil = new FileUtilsV3(FilePathThere, true);
                        try
                        {
                            fileUtil.WriteBytes(filePart);
                        }
                        catch (Exception e)
                        {
                            main.OnException(e, "filePart appending exception");
                        }
                        //main.main.UpdateTextbox1(main.main.testCounter.ToString()+" ");
                        //main.main.UpdateTextbox1("1");

                        if (isWithGetCallBack)
                            callBack(i);

                        if (countData == fullLengthOfTheFile)
                            endcallback();
                    }
                    else
                    {
                        innerEmp.addChunk(data);
                        byte[] filePart = (byte[])main.finishedTransfer2(innerEmp);
                        int i = filePart.Length;
                        countData += System.Convert.ToUInt64(i);

                        //main.main.testCounter++;
                        //main.main.UpdateTextbox1(" " + main.main.testCounter.ToString());
                        //main.main.UpdateTextbox1("0");
                        try
                        {
                            fileUtil.WriteBytes(filePart);
                        }
                        catch (Exception e)
                        {
                            main.OnException(e, "filePart appending exception");
                        }
                        //main.main.UpdateTextbox1(main.main.testCounter.ToString() + " ");
                        //main.main.UpdateTextbox1("1");
                        //MessageBox.Show("write2");

                        if (isWithGetCallBack)
                            callBack(i);

                        if (countData == fullLengthOfTheFile)
                            endcallback();
                    }

                    break;

                //F_SINGLECHUNK		8
                case 8:

                    //lengthoflastchunk wird bestimmt
                    byte[] tmp = new byte[2];
                    tmp[0] = data[1];
                    tmp[1] = data[2];
                    short lengthOfLastChunk1 = BitConverter.ToInt16(tmp, 0);


                    innerEmp = new Empfaenger(lengthOfLastChunk1, data, 0, false,false,false);
                    byte[] filePart1 = (byte[])main.finishedTransfer2(innerEmp);
                    int i1 = filePart1.Length;
                    countData += System.Convert.ToUInt64(i1);

                    //main.main.testCounter++;
                    //main.main.UpdateTextbox1(" " + main.main.testCounter.ToString());
                    //main.main.UpdateTextbox1("0");
                    try
                    {
                        fileUtil.WriteBytes(filePart1);
                    }
                    catch (Exception e)
                    {
                        main.OnException(e, "filePart appending exception");
                    }
                    //main.main.UpdateTextbox1(main.main.testCounter.ToString() + " ");
                    //main.main.UpdateTextbox1("1");
                    //MessageBox.Show("write3");

                    if (isWithGetCallBack)
                        callBack(i1);

                    if (countData == fullLengthOfTheFile)
                        endcallback();

                    break;


                //F_STARTERCHUNK	9
                case 9:

                    //lengthoflastchunk wird bestimmt
                    byte[] tmp2 = new byte[2];
                    tmp2[0] = data[1];
                    tmp2[1] = data[2];
                    short lengthOfLastChunk2 = BitConverter.ToInt16(tmp2, 0);


                    innerEmp = new Empfaenger(lengthOfLastChunk2, data, 0, false,false,false);
                    this.lengthOfLastChunk = innerEmp.lengthOfLastChunk;
                    //MessageBox.Show("starter");
                    break;



                //F_NORMALCHUNK	    10
                case 10:
                    innerEmp.addChunk(data);

                    break;

            }





        }
        public void callBack(int i)
        {
            main.OnReceiveChunk( this.CustomHeader, this.fullLengthOfTheFile, i,countData, countData == fullLengthOfTheFile ,this.commandID);

        }
        public void endcallback()
        {
            fileUtil.Dispose();
            isDead = true;
            if (isEndCallback) main.sendEndCallback(commandID);
            //führe OnReceiveCompletedDataEvent aus
            main.OnReceiveCompletedData(null, this.CustomHeader);
        }





    }
}

