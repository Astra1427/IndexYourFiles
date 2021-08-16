using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IndexYourFiles.Utilities
{
    public static class LogHelper
    {
        private static string LogFile = "Log.txt";
        public static bool LogError(Exception e)
        {
            return Uti.WriteFile(LogFile,e.ToString());
        }

    }
}
