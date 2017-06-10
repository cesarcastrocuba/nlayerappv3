namespace NLayerApp.Infrastructure.Crosscutting.NetFramework.Adapter
{
    using AutoMapper;
    using Infrastructure.Crosscutting.Adapter;
    using System;
    using System.Linq;
    using System.Reflection;

    public class AutomapperTypeAdapterFactory
        : ITypeAdapterFactory
    {
        #region Constructor

        /// <summary>
        /// Create a new Automapper type adapter factory
        /// </summary>
        public AutomapperTypeAdapterFactory()
        {
            //scan all assemblies finding Automapper Profile
            var profiles = AppDomain.CurrentDomain
                                    .GetAssemblies()
                                    .Where(x => x.GetName().FullName.Contains("NLayerApp"))
                                    .SelectMany(a => a.GetTypes())
                                    .Where(t => t.GetTypeInfo().BaseType == typeof(Profile));


            Mapper.Initialize(cfg =>
            {
                foreach (var item in profiles)
                {
                    if (item.FullName != "AutoMapper.SelfProfiler`2" &&
                        item.FullName != "AutoMapper.MapperConfiguration+NamedProfile")

                        cfg.AddProfile(Activator.CreateInstance(item) as Profile);
                }
            });

        }

        #endregion

        #region ITypeAdapterFactory Members

        public ITypeAdapter Create()
        {
            return new AutomapperTypeAdapter();
        }

        #endregion
    }
}
