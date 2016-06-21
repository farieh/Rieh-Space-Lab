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
    class FileSplitter
    {
        int CHUNKSIZE;
        string filepath;
        FileUtilsV3 fileUtil;

        public long FilePosition = 0;
        long FileSize;
        public bool isDone = false;
        public bool isDisposed = false;

        public FileSplitter(string filepath, int CHUNKSIZE)
        {
            this.CHUNKSIZE = CHUNKSIZE;
            this.filepath = filepath;

#warning exception gefahr
            fileUtil = new FileUtilsV3(filepath, false);

            FileSize = new FileInfo(filepath).Length;
        }

        public byte[] getFileChunk()
        {
            byte[] r = fileUtil.GetBytesFromFile(FilePosition, CHUNKSIZE);
            FilePosition += CHUNKSIZE;
            isDone = FilePosition >= FileSize;
            return r;
        }


        public void DisposeInternStuff()
        {
            isDisposed = false;
            fileUtil.Dispose();
        }

    }
}