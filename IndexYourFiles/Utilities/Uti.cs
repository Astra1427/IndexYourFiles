using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace IndexYourFiles.Utilities
{
    class Uti
    {
        #region File Helper
        public static string BasePath = Directory.GetCurrentDirectory();
        public static bool WriteFile(string file, string content)
        {
            try
            {

                if (!File.Exists($"{BasePath}/{file}"))
                {
                    File.WriteAllText($"{BasePath}/{file}", content);
                }
                else
                {
                    File.AppendAllText($"{BasePath}/{file}", content);
                }
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString(), "Log Error Failed!", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        public static string ReadFile(string file)
        {
            try
            {
                if (!File.Exists($"{BasePath}/{file}"))
                {
                    return null;
                }
                return File.ReadAllText($"{BasePath}/{file}");
            }
            catch (Exception e)
            {
                LogHelper.LogError(e);
                return null;
            }
        }

        #endregion
    }
}
