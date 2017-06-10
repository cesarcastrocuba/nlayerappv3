namespace NLayerApp.Infrastructure.Crosscutting.NetFramework.Adapter
{
    using System.Collections.Generic;
    using System.Reflection;
    using Microsoft.Extensions.DependencyModel;
    using System.Linq;
    public class AppDomain
    {
        public static AppDomain CurrentDomain { get; private set; }

        static AppDomain()
        {
            CurrentDomain = new AppDomain();
        }

        public Assembly[] GetAssemblies()
        {
            var assemblies = new List<Assembly>();
            var dependencies = DependencyContext.Default.RuntimeLibraries;
            foreach (var library in dependencies)
            {
                if (IsCandidateCompilationLibrary(library))
                {
                    var assembly = Assembly.Load(new AssemblyName(library.Name));
                    assemblies.Add(assembly);
                }
            }
            return assemblies.ToArray();
        }

        private static bool IsCandidateCompilationLibrary(RuntimeLibrary compilationLibrary)
        {
            return compilationLibrary.Name == ("NLayerApp")
                || compilationLibrary.Dependencies.Any(d => d.Name.StartsWith("NLayerApp"));
        }
    }
}
