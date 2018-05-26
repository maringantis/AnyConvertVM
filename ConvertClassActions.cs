using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AnyConvertVM
{
    class ConvertClassActions
    {

        //qemu-img convert -f raw -O qcow2 image.img image.qcow2

        public enum FormatType { VDI,VHDX,RAW,QCOW2,VMDK,VHD };

        public static void ConvertQEMU(FormatType fromFormat, FormatType toFormat, String FromFolderWithFile, String FileName,String SaveFolderPath)
        {

            try
            {
                Console.WriteLine("Will start to convert " + fromFormat + " to " + toFormat);

                //string fromFormatTextLower = fromFormat.ToString().ToLower();

                //Log.Info(TAG + "Trying to update MTU values :" + Settings.MTU);
                String Arguments = "/c Tools\\qemu-img-win-x64-2_3_0\\qemu-img.exe convert -f " + fromFormat.ToString().ToLower() + " -O " + toFormat.ToString().ToLower() 
                    +" "+ @FromFolderWithFile +  " " + @SaveFolderPath+@"\"+FileName+"."+ toFormat.ToString().ToLower();
                Console.WriteLine("ARGs: " + Arguments);
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = Arguments,
                        UseShellExecute = false,
                        RedirectStandardOutput = false,
                        CreateNoWindow = false

                    }
                };
                proc.Start();
                var std_out_error_reader = proc.StandardError;
                var std_out_data_reader = proc.StandardOutput;
                WriteToLog(std_out_data_reader);
                WriteToLog(std_out_error_reader);

                Thread.Sleep(10000);
                proc.WaitForExit();
            }
            catch (Exception ex)
            {

            }

        }

        public static void WriteToLog(StreamReader std_out_reader)
        {

            while (!std_out_reader.EndOfStream)
            {
                // the point is that the stream does not end until the process has 
                // finished all of its output.
                var nextLine = std_out_reader.ReadLine();

                Console.WriteLine(nextLine);
                //if (level.Equals("e"))
                //{
                //    Log.Error(TAG + nextLine);
                //}
                //else Log.Debug(TAG + nextLine);

                //if (isVpnError(nextLine))
                //{
                //    Connection.UeAgent.Client.onIpsecError(nextLine);
                //}
                //else
                //{
                //    if (nextLine.Contains(OpenVPN_Parsing.VPN_CONNECTED))
                //    {
                //        Connection.UeAgent.Client.onIpsecConnected();
                //        Thread.Sleep(3000);
                //    }
                //}




                //CreateFile failed on TAP device:
            }
            //  Log.Debug(TAG+ line);
        }
    }

}
