using System;
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

    }
    
}
