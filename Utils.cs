using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
    using System.Windows.Forms;
using log4net;

namespace Misc
{
    class Utils
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string TAG = "Utils: ";


        public static void ExceptionHandleMsg(String TAGSent, String ErrorMessage, Exception exception)
        {
            try
            {
                Log.Error(TAGSent + ErrorMessage);
                Log.Error("Exception message: " + exception.Message);
                Log.Error("Exception Source: " + exception.Message);
                Log.Error("Exception StackTrace: " + exception.StackTrace);
            }
            catch (Exception ex)
            {
                Log.Error(TAG + "Exception handling :" + ErrorMessage + exception.Message);
                Log.Error(TAGSent + "Exception message: " + ex.Message);
                Log.Error("Exception Source: " + ex.Message);
                Log.Error("Exception StackTrace: " + ex.StackTrace);
            }
        }

        public static void ErrorBox(string errormsg)
        {
            MessageBox.Show(errormsg, AnyConvertVM.Properties.Resources.AppTitle , MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        public static void ErrorBox(string errormsg,Exception ex)
        {
            MessageBox.Show(errormsg+"\nException messaage: "+ex, AnyConvertVM.Properties.Resources.AppTitle,
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }


        public static void StartProcess(string args,string ProcessLocationFile,string workingDir)
        {
            try
            {

                Process BGprocess = new Process();
                BGprocess.StartInfo.CreateNoWindow = true;
                BGprocess.EnableRaisingEvents = true;
                BGprocess.StartInfo.Arguments = args;
                BGprocess.StartInfo.FileName = ProcessLocationFile; //@"./OpenVPN/openvpn.exe";
                BGprocess.StartInfo.WorkingDirectory = workingDir; //@"./OpenVPN/";


                BGprocess.StartInfo.UseShellExecute = false;
                BGprocess.StartInfo.RedirectStandardOutput = true;
                BGprocess.StartInfo.RedirectStandardError = true;

                BGprocess.Start();
                var std_out_error_reader = BGprocess.StandardError;
                var std_out_data_reader = BGprocess.StandardOutput;
               


                BGprocess.WaitForExit();
            }
            catch (Exception ex)
            {
                ExceptionHandleMsg(TAG, "Failed to load the Disk image.", ex);
                
            }

        }

        

    }
    
}
