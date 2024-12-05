using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Threading.Tasks;

namespace TPBlog.Data.SeedWorks
{
    public static class AssemblyHelper
    {
        public static IEnumerable<Assembly> GetAllAssemblies(SearchOption searchOption = SearchOption.TopDirectoryOnly)
        {
            var assemblies = new List<Assembly>();

            foreach (var assemblyPath in
                     Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll", searchOption))
            {
                try
                {
                    var assembly = AssemblyLoadContext.Default.LoadFromAssemblyPath(assemblyPath);

                    if (assemblies.Find(a => a == assembly) != null)
                        continue;

                    assemblies.Add(assembly);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }

            return assemblies;
        }
    }


}
