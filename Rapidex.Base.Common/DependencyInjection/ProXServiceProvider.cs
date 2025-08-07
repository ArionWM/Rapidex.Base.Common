//using System;
//using System.Collections.Generic;
//using System.Runtime.CompilerServices;
//using System.Text;

//namespace Rapidex.DependencyInjection
//{
//    internal class RapidexServiceProvider : IServiceProvider, IKeyedServiceProvider
//    {
//        IKeyedServiceProvider DefaultProvider { get; }

//        public RapidexServiceProvider(IServiceProvider defaultProvider)
//        {
//            DefaultProvider = (IKeyedServiceProvider)defaultProvider;
//        }

//        public object GetRapidexService(Type type, object? serviceKey)
//        {
//            if (serviceKey == null || (serviceKey is string serviceCode && serviceCode.IsNullOrEmpty()))
//                serviceKey = Common.EnviromentCode;

//            object service = this.DefaultProvider.GetKeyedService(type, serviceKey);

//            if (service == null && serviceKey.IsNOTNullOrEmpty())
//            {
//                switch (serviceKey)
//                {
//                    case CommonConstants.ENV_UNITTEST: //ENV_UNITTEST ise ve bulunamaz ise ENV_DEVELOPMENT a bakarız
//                        service = this.GetRapidexService(type, CommonConstants.ENV_DEVELOPMENT);
//                        break;

//                    case CommonConstants.ENV_DEVELOPMENT: // ENV_DEVELOPMENT ise ve bulunamaz ise ENV_PRODUCTION a bakarız
//                        service = this.GetRapidexService(type, CommonConstants.ENV_PRODUCTION);
//                        break;
                        
//                }
//            }

//            if (service == null)
//                service = this.DefaultProvider.GetService(type);

//            return service;
//        }

//        public object GetService(Type serviceType)
//        {
//            return this.GetRapidexService(serviceType, null);
//        }

//        public object? GetKeyedService(Type serviceType, object? serviceKey)
//        {
//            return this.GetRapidexService(serviceType, serviceKey?.ToString());

//        }

//        public object GetRequiredKeyedService(Type serviceType, object? serviceKey)
//        {
//            object? service = GetKeyedService(serviceType, serviceKey);
//            if (service == null)
//            {
//                throw new InvalidOperationException($"service '{serviceType.Name}' not found with '{serviceKey}'");
//            }
//            return service;
//        }
//    }
//}
