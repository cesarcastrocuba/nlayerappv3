using AutoMapper;
using NLayerApp.Infrastructure.Crosscutting.Tests.Classes;
using System;

namespace NLayerApp.Infrastructure.Crosscutting.Tests
{
    public class AutomapperInitializer : IDisposable
    {
        public AutomapperInitializer()
        {
            //Arrange
            Mapper.Initialize(cfg => cfg.AddProfile(new TypeAdapterProfile()));
        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}
