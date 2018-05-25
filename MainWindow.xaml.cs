using System;
using System.IO;
using System.Windows;
using System.Windows.Forms;
using log4net;
namespace AnyConvertVM
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
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
                //selectFromDisk_TB.Text = result.ToString;
            }
            catch (Exception ex)
            {

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
            }
            catch (Exception ex)
            {

            }

        }

        private void Convert_BTN_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                Enum FromFormat=null;
                Enum ToFormat = null; 


                String SelectedDiskFileNameWithExt = Path.GetFileName(selectFromDisk_TB.Text);
                Console.WriteLine("Selected File Name with ext:" + SelectedDiskFileNameWithExt);

                String SelectedDiskFileName = Path.GetFileNameWithoutExtension(selectFromDisk_TB.Text);
                Console.WriteLine("Selected File Name:" + SelectedDiskFileName);

                String SelectedDiskFormatName= Path.GetExtension(selectFromDisk_TB.Text);
                Console.WriteLine("Selected File Format:" + SelectedDiskFormatName);

                String toExt = null;
               
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
                #endregion
                // Console.WriteLine("Selected File Format:"+ SelectedDiskFormatName);

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
                else
                {
                    System.Windows.MessageBox.Show(Properties.Resources.WrongFileFormat_msg);
                }
                #endregion

                String SaveFileName = null;
                SaveFileName = SelectedDiskFileName + toExt;
                Console.WriteLine("Saved File Name will be: " + SaveFileName);

                if ( (!ToFormat.Equals(null)) ||
                     (!FromFormat.Equals(null)) ||
                     (!SaveFileName.Equals(null))
                   )
                {
                    Console.WriteLine("Let's Convert");
                    //ConvertClassActions.ConvertQEMU(FromFormat ,ToFormat);
                }
                

               

            }
            catch (Exception ex)
                {

                }
            }
    }
}
