using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexYourFiles.Expands
{
    public class DriveInfoExpand
    {
        public DriveInfo Drive { get; set; }

        public string DisAvailableFreeSpace => (Drive?.AvailableFreeSpace / 1024 / 1024 / 1024)?.ToString("0.000")+"G";

        public string DisTotalFreeSpace => (Drive?.TotalSize / 1024 / 1024 / 1024)?.ToString("0.000") + "G";
    }
}