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
using System.IO;

class FileUtilsV3 : IDisposable
{
    private FileStream fs;
    private BinaryReader reader;
    bool disposed = false;

    public FileUtilsV3(String path, bool isWrite)
    {
        //fs = new FileStream(path, FileMode.OpenOrCreate, FileAccess.Read);
        //reader = new BinaryReader(fs);
        if (isWrite)
        {            
            FileMode fmode = !System.IO.File.Exists(path) ? FileMode.CreateNew : FileMode.Append;
            fs = new FileStream(path, fmode, FileAccess.Write);
        }
        else
        {
            fs = new FileStream(path, FileMode.Open, FileAccess.Read);
            reader = new BinaryReader(fs);
        }
    }

    public void WriteBytes(byte[] b)
    {
        fs.Write(b, 0, b.Length);
    }

    //public static byte[] GetBytesFromFile(string FileName, long start, int count)
    //{
    //    FileStream fs = new FileStream(FileName, FileMode.Open);
    //    BinaryReader reader = new BinaryReader(fs);

    //    reader.BaseStream.Position = start;
    //    byte[] Data = reader.ReadBytes(count);

    //    reader.Close();
    //    fs.Close();

    //    return Data;
    //}

    public byte[] GetBytesFromFile(long start, int count)
    {
        reader.BaseStream.Position = start;
        byte[] Data = reader.ReadBytes(count);

        return Data;
    }

    public void Dispose()
    {
        Dispose(true);
        // This object will be cleaned up by the Dispose method.
        // Therefore, you should call GC.SupressFinalize to
        // take this object off the finalization queue 
        // and prevent finalization code for this object
        // from executing a second time.
        GC.SuppressFinalize(this);
    }

    private void Dispose(bool disposing)
    {
        // Check to see if Dispose has already been called.
        if (!this.disposed)
        {
            // If disposing equals true, dispose all managed 
            // and unmanaged resources.
            if (disposing)
            {
                // Dispose managed resources.
                if(reader!=null)
                reader.Close();
                if (fs != null)
                fs.Close();               
            }
            // Call the appropriate methods to clean up 
            // unmanaged resources here.
        }
        disposed = true;
    }

    ~FileUtilsV3()
    {
        Dispose(false);
    }
}

