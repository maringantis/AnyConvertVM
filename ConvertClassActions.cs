using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using log4net;

namespace AnyConvertVM
{
    class ConvertClassActions
    {

        
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string TAG = "ConvertClassActions: ";

        public enum FormatType { VDI,VHDX,RAW,QCOW2,VMDK,VHD,QED };

        public static bool ErrorFlag = false;
        public static int ConvertQEMU(FormatType fromFormat, FormatType toFormat, String FromFolderWithFile, String FileName,String SaveFolderPath)
        {

            try
            {
                //qemu-img convert -f raw -O qcow2 image.img image.qcow2
                // String args = null;

                Log.Debug(TAG+"Will start to convert " + fromFormat + " to " + toFormat);

                string fromFromatArgs = fromFormat.ToString().ToLower();
                string toFromatArgs = toFormat.ToString().ToLower();

                if (fromFromatArgs.Equals("vhd")) { fromFromatArgs = "vpc"; }
                if (toFromatArgs.Equals("vhd")) { toFromatArgs = "vpc"; }

                String Arguments = "/c Tools\\qemu-img-win-x64-2_3_0\\qemu-img.exe convert -f " + fromFromatArgs + " -O " + toFromatArgs
                    + " " +"\""+ FromFolderWithFile +"\"" +  " " + "\"" + SaveFolderPath+@"\"+FileName+"."+ toFormat.ToString().ToLower() + "\"";

                //Misc.Utils.StartProcess(Arguments,"Tools\\qemu-img-win-x64-2_3_0\\qemu-img.exe");

                Log.Debug(TAG+"Command: " + Arguments);
                ErrorFlag = false;
                var proc = new Process
                {
                    StartInfo = new ProcessStartInfo
                    {
                        FileName = "cmd.exe",
                        Arguments = Arguments,
                    }
                };

                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.CreateNoWindow = true;
                proc.EnableRaisingEvents = true;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.RedirectStandardError = true;
                proc.Start();

                var std_out_error_reader = proc.StandardError;
                var std_out_data_reader = proc.StandardOutput;
                WriteToLog(std_out_data_reader, "d");
                WriteToLog(std_out_error_reader, "e");

                proc.WaitForExit();

                if (ErrorFlag)
                {
                    Log.Error(TAG+ "Failed to convert the disks.");
                    return 1;
                }
                else
                {
                    Log.Debug(TAG+ "Converted "+toFormat +" disk from "+fromFormat+". Converted disk saved at :"
                        +SaveFolderPath + @"\" + FileName + "." + toFormat.ToString().ToLower());

                    MessageBox.Show("Converted "+toFormat +" disk from "+fromFormat+".Converted disk saved at: "
                        + SaveFolderPath + @"\" + FileName + "." + toFormat.ToString().ToLower());
                    return 0;
                }

            }
            catch (Exception ex)
            {
                Misc.Utils.ExceptionHandleMsg(TAG, "Could not convert the image.", ex);
                Misc.Utils.ErrorBox("Could not convert the image.", ex);
                return 1;
            }

        }


        public static void WriteToLog(StreamReader std_out_reader, string level)
        {
            try
            {
                while (!std_out_reader.EndOfStream)
                {
                    // the point is that the stream does not end until the process has 
                    // finished all of its output.
                    var nextLine = std_out_reader.ReadLine();
                    string errorMsg = null;
                    if (level.Equals("e"))
                    {
                        Log.Error(TAG + nextLine);
                        ErrorFlag = true;
                        //Misc.Utils.ErrorBox("Failed to convert the disks.");
                        if ((nextLine.Contains("bad signature")) || (nextLine.Contains("Image not in")))
                        {
                            errorMsg = "The selected disk is not in the correct format.";
                        }
                        else
                            errorMsg = "Failed to convert the disks";

                        Misc.Utils.ErrorBox(errorMsg+"\nError message: " + nextLine);
                    }
                    else
                        Log.Debug(TAG + nextLine);

                }
            }
            catch (Exception ex)
            {
                Misc.Utils.ExceptionHandleMsg(TAG, "Failed to write to Log", ex);
                //Misc.Utils.ErrorBox("Could not convert the image.", ex);
            }

        }


    }

}
