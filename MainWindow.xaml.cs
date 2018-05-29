using System;
using System.IO;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Navigation;
using log4net;
namespace AnyConvertVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string TAG = "MainWindow: ";

        public MainWindow()
        {
            log4net.Config.XmlConfigurator.Configure();
            InitializeComponent();
            LoadComponent();
        }

        private void LoadComponent()
        {
            //Load components
            Log.Debug(TAG+"Starting "+Properties.Resources.AppTitle);
            Title = Properties.Resources.AppTitle + " V1.0";
           
        }
      
        private void selectFromDisk_BTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var fileDialog = new OpenFileDialog();
                var result = fileDialog.ShowDialog();

                switch (result)
                {
                    case System.Windows.Forms.DialogResult.OK:
                        var file = fileDialog.FileName;
                        selectFromDisk_TB.Text = file;
                        selectFromDisk_TB.ToolTip = file;
                        break;
                    case System.Windows.Forms.DialogResult.Cancel:
                    default:
                        selectFromDisk_TB.Text = null;
                        selectFromDisk_TB.ToolTip = null;
                        break;
                }

                saveToDisk_TB.Text = Path.GetDirectoryName(selectFromDisk_TB.Text);
                ProgressBar_PB.Value += 9;
                //selectFromDisk_TB.Text = result.ToString;
            }
            catch (Exception ex)
            {
                Misc.Utils.ExceptionHandleMsg(TAG, "Failed to load the Disk image.", ex);
                ProgressBar_PB.Value = 0;
                // Misc.Utils.ErrorBox("Failed to load the Disk image.", ex);
            }
        }

        private void saveToDisk_BTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var folderDialog = new FolderBrowserDialog();
            var result = folderDialog.ShowDialog();

            switch (result)
            {
                case System.Windows.Forms.DialogResult.OK:
                    var folder = folderDialog.SelectedPath;
                    saveToDisk_TB.Text = folder;
                    saveToDisk_TB.ToolTip = folder;
                    break;
                case System.Windows.Forms.DialogResult.Cancel:
                default:
                    saveToDisk_TB.Text = null;
                    saveToDisk_TB.ToolTip = null;
                    break;
            }
                ProgressBar_PB.Value += 10;
            }
            catch (Exception ex)
            {
                Misc.Utils.ExceptionHandleMsg(TAG, "Failed to save the converted Disk image.", ex);
                if (ProgressBar_PB.Value <= 0) { ProgressBar_PB.Value = 0; }
                else ProgressBar_PB.Value -= 10;
                //Misc.Utils.ErrorBox("Failed to save the converted Disk image.", ex);
            }

        }

        private void Convert_BTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ProgressBar_PB.Value += 20;

                if ((selectFromDisk_TB.Text.Equals(null) )||(selectFromDisk_TB.Text.Equals(""))
                        || (saveToDisk_TB.Text.Equals(null))|| (saveToDisk_TB.Text.Equals("")))
                {
                    Misc.Utils.ErrorBox("Please select the disk file to convert , choose conversion format and disk location for converted file to save.");
                }
                else {

                    System.Windows.Forms.MessageBox.Show("Conversion will start now.Application may hang for few minutes before conversion is complete. Do not close/kill the app.",
                        Properties.Resources.AppTitle, MessageBoxButtons.OK, MessageBoxIcon.Information);
                    var FromFormat = new ConvertClassActions.FormatType();
                    var ToFormat = new ConvertClassActions.FormatType();


                    String SelectedDiskFileNameWithExt = Path.GetFileName(selectFromDisk_TB.Text);
                    Log.Debug(TAG + "Selected File Name with ext:" + SelectedDiskFileNameWithExt);

                    String SelectedDiskFileName = Path.GetFileNameWithoutExtension(selectFromDisk_TB.Text);
                    Log.Debug(TAG + "Selected File Name:" + SelectedDiskFileName);

                    String SelectedDiskFormatName = Path.GetExtension(selectFromDisk_TB.Text);
                    Log.Debug(TAG + "Selected File Format:" + SelectedDiskFormatName);



                    if ((selectFromDisk_TB.Text.Equals("Select the virtual hard disk to convert")) ||
                        (selectFromDisk_TB.Text.Equals(null)) ||
                        !File.Exists(selectFromDisk_TB.Text))
                    {
                        System.Windows.MessageBox.Show(Properties.Resources.SelectFile_msg);
                    }

                    if ((saveToDisk_TB.Text.Equals("Select the location the convert file to be saved")) ||
                        (saveToDisk_TB.Text.Equals(null)) ||
                        !Directory.Exists(saveToDisk_TB.Text))
                    {
                        System.Windows.MessageBox.Show(Properties.Resources.SelectFolder_msg);
                    }

                    #region ToFormatCheck
                    String toExt = null;
                    if (VDI_RB.IsChecked.Equals(true))
                    {
                        ToFormat = ConvertClassActions.FormatType.VDI;
                        toExt = ".vdi";

                    }
                    else if (VHDX_RB.IsChecked.Equals(true))
                    {
                        ToFormat = ConvertClassActions.FormatType.VHDX;
                        toExt = ".vhdx";
                    }
                    else if (RAW_RB.IsChecked.Equals(true))
                    {
                        ToFormat = ConvertClassActions.FormatType.RAW;
                        toExt = ".raw";

                    }
                    else if (QCOW2_RB.IsChecked.Equals(true))
                    {
                        ToFormat = ConvertClassActions.FormatType.QCOW2;
                        toExt = ".qcow2";

                    }
                    else if (VMDK_rb.IsChecked.Equals(true))
                    {
                        ToFormat = ConvertClassActions.FormatType.VMDK;
                        toExt = ".vmdk";
                    }
                    else if (VHD_RB.IsChecked.Equals(true))
                    {
                        ToFormat = ConvertClassActions.FormatType.VHD;
                        toExt = ".vhd";
                    }
                    else if (QED_RB.IsChecked.Equals(true))
                    {
                        ToFormat = ConvertClassActions.FormatType.QED;
                        toExt = ".qed";
                    }
                    #endregion

                    Log.Debug(TAG + "Selected File Format:" + SelectedDiskFormatName);

                    #region FromFormatCheck
                    if (SelectedDiskFormatName.Equals(".vdi"))
                    {
                        FromFormat = ConvertClassActions.FormatType.VDI;
                    }
                    else if (SelectedDiskFormatName.Equals(".vhdx"))
                    {
                        FromFormat = ConvertClassActions.FormatType.VHDX;

                    }
                    else if (SelectedDiskFormatName.Equals(".raw"))
                    {
                        FromFormat = ConvertClassActions.FormatType.RAW;


                    }
                    else if (SelectedDiskFormatName.Equals(".qcow2"))
                    {
                        FromFormat = ConvertClassActions.FormatType.QCOW2;


                    }
                    else if (SelectedDiskFormatName.Equals(".vmdk"))
                    {
                        FromFormat = ConvertClassActions.FormatType.VMDK;

                    }
                    else if (SelectedDiskFormatName.Equals(".vhd"))
                    {
                        FromFormat = ConvertClassActions.FormatType.VHD;

                    }
                    else if (SelectedDiskFormatName.Equals(".qed"))
                    {
                        FromFormat = ConvertClassActions.FormatType.QED;
                    }
                    else
                    {
                        System.Windows.MessageBox.Show(Properties.Resources.WrongFileFormat_msg);
                    }
                    #endregion



                    if ((!ToFormat.Equals(null)) || (!FromFormat.Equals(null)))
                    {
                        Log.Debug(TAG + "Starting conversion...");
                        //MessageBox.Show()
                        if (ConvertClassActions.ConvertQEMU(FromFormat, ToFormat, selectFromDisk_TB.Text, SelectedDiskFileName, saveToDisk_TB.Text).Equals(0))
                        {
                            ProgressBar_PB.Value = 100;
                        }
                    }
                }
            }
            catch (Exception ex)
                {
                Misc.Utils.ExceptionHandleMsg(TAG, "Failed to convert the disk image.", ex);
                Misc.Utils.ErrorBox("Failed to convert the disk image.", ex);
                if (ProgressBar_PB.Value <= 0) { ProgressBar_PB.Value = 0; }
                else ProgressBar_PB.Value -= 10;
            }

        }

        #region Copyright
        private void Hyperlink_RequestNavigate1(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start("https://www.linkedin.com/in/srinivasamaringanti/");
        }

        private void Hyperlink_RequestNavigate2(object sender, RequestNavigateEventArgs e)
        {
            System.Diagnostics.Process.Start("http://shyzon.com/");
        }

        #endregion Sunny Maringanti

        private void saveToDisk_TB_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            try
            {
                ProgressBar_PB.Value += 10;
            }
            catch (Exception ex)
            {
                Misc.Utils.ExceptionHandleMsg(TAG, " ", ex);
                //Misc.Utils.ErrorBox("Failed to convert the disk image.", ex);
                if (ProgressBar_PB.Value <= 0) { ProgressBar_PB.Value = 0; }
                else ProgressBar_PB.Value = ProgressBar_PB.Value - 10;
            }
        }
    }
}
