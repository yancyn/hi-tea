/*
 * Created by SharpDevelop.
 * User: BeanBean
 * Date: 10/17/2013
 * Time: 9:26 PM
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace PosWPF
{
public class PrintFactory
    {
        public const short FILE_ATTRIBUTE_NORMAL = 0x80;
        public const short INVALID_HANDLE_VALUE = -1;
        public const uint GENERIC_READ = 0x80000000;
        public const uint GENERIC_WRITE = 0x40000000;
        public const uint CREATE_NEW = 1;
        public const uint CREATE_ALWAYS = 2;
        public const uint OPEN_EXISTING = 3;

        [DllImport("kernel32.dll", SetLastError = true)]
        static extern IntPtr CreateFile(string lpFileName, uint dwDesiredAccess,
            uint dwShareMode, IntPtr lpSecurityAttributes, uint dwCreationDisposition,
            uint dwFlagsAndAttributes, IntPtr hTemplateFile);

        public static void SendTextToLPT1(String receiptText)
        {
            IntPtr ptr = CreateFile("LPT1", GENERIC_WRITE, 0, IntPtr.Zero, OPEN_EXISTING, 0, IntPtr.Zero);

            /* Is bad handle? INVALID_HANDLE_VALUE */
            if (ptr.ToInt32() == -1)
            {
                /* ask the framework to marshall the win32 error code to an exception */
                Marshal.ThrowExceptionForHR(Marshal.GetHRForLastWin32Error());
            }
            else
            {
                FileStream lpt = new FileStream(ptr, FileAccess.ReadWrite);
                Byte[] buffer = new Byte[2048];
                //Check to see if your printer support ASCII encoding or Unicode.
                //If unicode is supported, use the following:
                buffer = System.Text.Encoding.Unicode.GetBytes(receiptText);
                //buffer = System.Text.Encoding.ASCII.GetBytes(receiptText);
                lpt.Write(buffer, 0, buffer.Length);
                lpt.Close();
            }
        }
    }
}