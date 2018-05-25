using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnyConvertVM
{
    static class ConvertClassActions
    {

        //qemu-img convert -f raw -O qcow2 image.img image.qcow2

        public enum FormatType { VDI,VHDX,RAW,QCOW2,VMDK,VHD };

        public static void ConvertQEMU(FormatType fromFormat, FormatType toFormat)
        {

            try
            {

            }
            catch (Exception ex)
            {

            }

        }
    }

}
