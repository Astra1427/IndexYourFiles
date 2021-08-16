using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;


namespace IndexYourFiles.Models
{
    public class UFile<T>
    {
        public UFile(T item)
        {
            Item = item;
        }

        public T Item { get; set; }

        public string Name { 
            get {
                var type = Item.GetType();
                var assembly = Assembly.GetAssembly(type);
                var name = assembly.GetName();

                var sc = new ServiceCollection();

                //sc.AddScoped(typeof());

                return "";
            } 
        }
    }
}
