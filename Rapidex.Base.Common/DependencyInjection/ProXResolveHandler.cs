//using System;
//using System.Collections.Generic;
//using System.Text;

//namespace Rapidex.DependencyInjection
//{
//    public class RapidexResolveHandler
//    {
//        IKeyedServiceProvider DefaultProvider { get; set; }

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

//        public object GetService(IServiceProvider provider, Type type)
//        {
//            this.DefaultProvider = (IKeyedServiceProvider)provider;

//            return this.GetRapidexService(type, null);

//        }
//    }
//}
