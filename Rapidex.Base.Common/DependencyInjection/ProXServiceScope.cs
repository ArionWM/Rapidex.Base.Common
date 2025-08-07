//using System;
//using System.Collections.Generic;
//using System.Text;
//using System.Threading.Tasks;

//namespace Rapidex.DependencyInjection;

////See autofac code

///// <summary>
///// Autofac implementation of the ASP.NET Core <see cref="IServiceScope"/>.
///// </summary>
///// <seealso cref="IServiceScope" />
//internal class RapidexServiceScope : IServiceScope, IAsyncDisposable
//{
//    private bool _disposed;
//    private readonly RapidexServiceProvider _serviceProvider;

//    /// <summary>
//    /// Initializes a new instance of the <see cref="RapidexServiceScope"/> class.
//    /// </summary>
//    /// <param name="lifetimeScope">
//    /// The lifetime scope from which services should be resolved for this service scope.
//    /// </param>
//    public RapidexServiceScope(RapidexServiceProvider provider)
//    {
//        _serviceProvider = provider;
//    }

//    /// <summary>
//    /// Gets an <see cref="IServiceProvider" /> corresponding to this service scope.
//    /// </summary>
//    /// <value>
//    /// An <see cref="IServiceProvider" /> that can be used to resolve dependencies from the scope.
//    /// </value>
//    public IServiceProvider ServiceProvider
//    {
//        get
//        {
//            return _serviceProvider;
//        }
//    }

//    /// <summary>
//    /// Disposes of the lifetime scope and resolved disposable services.
//    /// </summary>
//    public void Dispose()
//    {
//        Dispose(true);
//        GC.SuppressFinalize(this);
//    }

//    /// <summary>
//    /// Releases unmanaged and - optionally - managed resources.
//    /// </summary>
//    /// <param name="disposing">
//    /// <see langword="true" /> to release both managed and unmanaged resources; <see langword="false" /> to release only unmanaged resources.
//    /// </param>
//    protected virtual void Dispose(bool disposing)
//    {
//        if (!_disposed)
//        {
//            _disposed = true;

//            if (disposing)
//            {
//                //_serviceProvider.Dispose();
//            }
//        }
//    }

//    /// <inheritdoc/>
//    public async ValueTask DisposeAsync()
//    {
//        if (!_disposed)
//        {
//            _disposed = true;
//            //await _serviceProvider.DisposeAsync().ConfigureAwait(false);
//        }
//    }
//}


