using Mapster;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rapidex
{
    internal class MappingHelper
    {
        public static void Setup()
        {
            Type[] mapsterRegisterTypes = Common.Assembly.FindDerivedClassTypes<IRegister>();
            IEnumerable<IRegister> mapsterRegisters = mapsterRegisterTypes.Select(x => (IRegister)Activator.CreateInstance(x));
            TypeAdapterConfig.GlobalSettings.Apply(mapsterRegisters);
            TypeAdapterConfig.GlobalSettings.Default.NameMatchingStrategy(NameMatchingStrategy.Flexible);
            //TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);
            //// register the mapper as Singleton service for my application
            //var mapperConfig = new Mapper(typeAdapterConfig);
            //services.AddSingleton<IMapper>(mapperConfig);

        }
    }
}
