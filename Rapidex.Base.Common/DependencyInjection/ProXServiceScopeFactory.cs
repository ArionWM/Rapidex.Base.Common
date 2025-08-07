//using Microsoft.Extensions.DependencyInjection;
//
//using System;
//using System.Collections.Generic;
//using System.Diagnostics.CodeAnalysis;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rapidex.DependencyInjection
//{
//    internal readonly struct ServiceCacheKey : IEquatable<ServiceCacheKey>
//    {
//        /// <summary>
//        /// Type of service being cached
//        /// </summary>
//        public ServiceIdentifier ServiceIdentifier { get; }

//        /// <summary>
//        /// Reverse index of the service when resolved in <c>IEnumerable&lt;Type&gt;</c> where default instance gets slot 0.
//        /// For example for service collection
//        ///  IService Impl1
//        ///  IService Impl2
//        ///  IService Impl3
//        /// We would get the following cache keys:
//        ///  Impl1 2
//        ///  Impl2 1
//        ///  Impl3 0
//        /// </summary>
//        public int Slot { get; }

//        public ServiceCacheKey(object key, Type type, int slot)
//        {
//            ServiceIdentifier = new ServiceIdentifier(key, type);
//            Slot = slot;
//        }

//        public ServiceCacheKey(ServiceIdentifier type, int slot)
//        {
//            ServiceIdentifier = type;
//            Slot = slot;
//        }

//        /// <summary>Indicates whether the current instance is equal to another instance of the same type.</summary>
//        /// <param name="other">An instance to compare with this instance.</param>
//        /// <returns>true if the current instance is equal to the other instance; otherwise, false.</returns>
//        public bool Equals(ServiceCacheKey other) =>
//            ServiceIdentifier.Equals(other.ServiceIdentifier) && Slot == other.Slot;

//        public override bool Equals([NotNullWhen(true)] object? obj) =>
//            obj is ServiceCacheKey other && Equals(other);

//        public override int GetHashCode()
//        {
//            unchecked
//            {
//                return (ServiceIdentifier.GetHashCode() * 397) ^ Slot;
//            }
//        }
//    }

//    internal readonly struct ServiceIdentifier : IEquatable<ServiceIdentifier>
//    {
//        public object? ServiceKey { get; }

//        public Type ServiceType { get; }

//        public ServiceIdentifier(Type serviceType)
//        {
//            ServiceType = serviceType;
//        }

//        public ServiceIdentifier(object? serviceKey, Type serviceType)
//        {
//            ServiceKey = serviceKey;
//            ServiceType = serviceType;
//        }

//        public static ServiceIdentifier FromDescriptor(ServiceDescriptor serviceDescriptor)
//            => new ServiceIdentifier(serviceDescriptor.ServiceKey, serviceDescriptor.ServiceType);

//        public static ServiceIdentifier FromServiceType(Type type) => new ServiceIdentifier(null, type);

//        public bool Equals(ServiceIdentifier other)
//        {
//            if (ServiceKey == null && other.ServiceKey == null)
//            {
//                return ServiceType == other.ServiceType;
//            }
//            else if (ServiceKey != null && other.ServiceKey != null)
//            {
//                return ServiceType == other.ServiceType && ServiceKey.Equals(other.ServiceKey);
//            }
//            return false;
//        }

//        public override bool Equals([NotNullWhen(true)] object? obj)
//        {
//            return obj is ServiceIdentifier && Equals((ServiceIdentifier)obj);
//        }

//        public override int GetHashCode()
//        {
//            if (ServiceKey == null)
//            {
//                return ServiceType.GetHashCode();
//            }
//            unchecked
//            {
//                return (ServiceType.GetHashCode() * 397) ^ ServiceKey.GetHashCode();
//            }
//        }

//        public bool IsConstructedGenericType => ServiceType.IsConstructedGenericType;

//        public ServiceIdentifier GetGenericTypeDefinition() => new ServiceIdentifier(ServiceKey, ServiceType.GetGenericTypeDefinition());

//        public override string? ToString()
//        {
//            if (ServiceKey == null)
//            {
//                return ServiceType.ToString();
//            }

//            return $"({ServiceKey}, {ServiceType})";
//        }
//    }

//    internal class RapidexServiceScopeFactory : IServiceScopeFactory //IAsyncDisposable, 
//    {
//        // For testing and debugging only

//        private bool _disposed;

//        public RapidexServiceScopeFactory(RapidexServiceProvider provider, bool isRootScope)
//        {
//            ResolvedServices = new Dictionary<ServiceCacheKey, object?>();
//            RootProvider = provider;
//            IsRootScope = isRootScope;
//        }

//        internal Dictionary<ServiceCacheKey, object?> ResolvedServices { get; }

//        internal bool Disposed => _disposed;


//        public bool IsRootScope { get; }

//        internal RapidexServiceProvider RootProvider { get; }

        

//        public IServiceScope CreateScope() => RootProvider.CreateScope();

        
//        public void Dispose()
//        {
//            //List<object>? toDispose = BeginDispose();

//            //if (toDispose != null)
//            //{
//            //    for (int i = toDispose.Count - 1; i >= 0; i--)
//            //    {
//            //        if (toDispose[i] is IDisposable disposable)
//            //        {
//            //            disposable.Dispose();
//            //        }
//            //        else
//            //        {
//            //            throw new InvalidOperationException(SR.Format(SR.AsyncDisposableServiceDispose, TypeNameHelper.GetTypeDisplayName(toDispose[i])));
//            //        }
//            //    }
//            //}
//        }

//        //public ValueTask DisposeAsync()
//        //{
            
//        //}
//    }
//}
