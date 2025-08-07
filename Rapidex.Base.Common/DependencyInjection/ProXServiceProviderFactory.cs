//using Microsoft.Extensions.DependencyInjection;
//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Rapidex.DependencyInjection
//{
//    //https://stackoverflow.com/questions/65198987/custom-iserviceproviderfactory-in-asp-net-core
//    //[Obsolete("", true)]
//    public class RapidexServiceProviderFactory : IServiceProviderFactory<IServiceCollection>
//    {
//        private readonly ServiceProviderOptions _options;

//        public RapidexServiceProviderFactory() : this(new ServiceProviderOptions())
//        {

//        }

//        /// <summary>
//        /// Initializes a new instance of the <see cref="RapidexServiceProviderFactory"/> class
//        /// with the specified <paramref name="options"/>.
//        /// </summary>
//        /// <param name="options">The options to use for this instance.</param>
//        public RapidexServiceProviderFactory(ServiceProviderOptions options)
//        {
//            _options = options ?? throw new ArgumentNullException(nameof(options));
//        }

//        public IServiceCollection CreateBuilder(IServiceCollection services)
//        {
//            return services;
//        }

//        public IServiceProvider CreateServiceProvider(IServiceCollection containerBuilder)
//        {
//            ServiceProvider defaultSP = containerBuilder.BuildServiceProvider(_options);
//            RapidexServiceProvider proXSP = new RapidexServiceProvider(defaultSP);
//            return proXSP;
//        }
//    }
//}
