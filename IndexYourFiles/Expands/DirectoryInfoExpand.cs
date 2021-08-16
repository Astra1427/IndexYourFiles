using IndexYourFiles.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndexYourFiles.Expands
{
    public class DirectoryInfoExpand
    {
        public DirectoryInfoExpand(string SearchWord, DirectoryInfo di)
        {
            this.SearchWord = SearchWord;
            Directory = di;
            
        }
        public string SearchWord { get; set; }
        public DirectoryInfo Directory { get; set; }
        /// <summary>
        /// Get Directories by SearchWord filter name
        /// </summary>
        /// <returns></returns>
        public List<DirectoryInfoExpand> GetDirectories()
        {
            return SearchHelper.SearchDirectories(Directory.FullName, SearchWord);
        }

        public IEnumerable<FileInfo> GetFilesBySearchWord()
        {
            return SearchHelper.SearchFiles(Directory.FullName, SearchWord);
        }


        public List<DirectoryInfoExpand> GetAllDirectories()
        {
            try
            {
                var ds = Directory.GetDirectories();
                var dies = new List<DirectoryInfoExpand>();
                if (ds.Length == 0)
                {
                    return dies;
                }
                foreach (var item in ds)
                {
                    dies.Add(new DirectoryInfoExpand(SearchWord, item));
                }
                return dies;
            }
            catch (Exception ex)
            {
                LogHelper.LogError(ex);
                return null;
            }
        }
        

    }
}
