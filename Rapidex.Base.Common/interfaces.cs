using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Text.Json.Serialization;
using System.Threading;
using YamlDotNet.Serialization;

namespace Rapidex
{
    /// <summary>
    /// ThreadStatic context yığınları için
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IStack<T>
    {
        internal Stack<T> Stack { get; }
        void Enter(T item);
        T GetCurrent();
        T Exit();
    }

    public interface IBaseConverter
    {
        Type FromType { get; }
        Type ToType { get; }

        [Obsolete("Use TryConvert() instead.")]
        object Convert(object from, Type toType);

        bool TryConvert(object from, Type toType, out object to);
    }

    public interface IBaseConverter<TFrom, TTo> : IBaseConverter
    {
        new Type FromType => typeof(TFrom);
        new Type ToType => typeof(TTo);

        TTo Convert(TFrom from, TTo to);
    }


    public interface IPaging
    {
        long? PageSize { get; set; }
        long? StartIndex { get; set; }
        long? PageIndex { get; set; }
        long? PageCount { get; set; }

        bool IncludeTotalItemCount { get; set; }

    }

    public interface ILoadResult<T> : IReadOnlyList<T>, IPaging, IEmptyCheckObject
    {
        long TotalItemCount { get; set; }
        long ItemCount { get; }
    }

    /// <summary>
    /// Rapidex library mark. 
    /// IRapidexModule contained libraries are scanned and loaded by Rapidex framework. 
    /// Other libraries are ignored.
    /// </summary>
    public interface IRapidexAssemblyDefinition : IOrderedComponent
    {
        string TablePrefix { get; }
        void SetupServices(IServiceCollection services);

        void SetupMetadata(IServiceCollection services);

        void Start(IServiceProvider serviceProvider);
    }

    /// <summary>
    /// Setup zamanında çalışan servislerdir.
    /// </summary>
    public interface IPreSetupManager
    {
        //void Start(IServiceCollection services);
    }

    /// <summary>
    /// Setup zamanında çalışmayan, start ile çalıştırılan servislerdir.
    /// </summary>
    public interface IManager
    {
        //TODO async
        void Setup(IServiceCollection services);

        //TODO async
        void Start(IServiceProvider serviceProvider);
    }


    public interface IEmptyCheckObject
    {
        bool IsEmpty { get; }
    }

    public interface ILoggingHelper
    {
        void EnterCategory(string category);
        void ExitCategory();

        void Info(string category, string message);
        void Info(string message);

        void Info(string category, Exception ex, string message = null);

        void Info(Exception ex, string message = null);

        void Debug(string category, string message);

        void Debug(string category, string format, params object[] args);

        void Debug(string format, params object[] args);

        void Debug(string message);

        void Verbose(string category, string message);

        void Verbose(string category, string format, params object[] args);

        void Verbose(string format, params object[] args);

        void Verbose(string message);

        void Warn(string category, string message);

        void Warn(string message);

        void Error(string category, string message);

        void Error(string message);

        void Error(string category, Exception ex, string message = null);

        void Error(Exception ex, string message = null);

        void Flush();
    }

    public interface IResult
    {
        [JsonPropertyOrder(-9999)]
        bool Success { get; set; }

        [JsonPropertyOrder(-9998)]

        string Description { get; set; }
    }

    public interface IResult<T> : IResult
    {
        T Content { get; set; }
    }



    public interface IValidationResultItem //From ProCore
    {
        bool MarkProblem { get; set; }
        string ParentName { get; set; }
        string MemberName { get; set; }
        string Description { get; set; }
    }

    public interface IValidationResult : IResult
    {
        //Geliştirilecek

        List<IValidationResultItem> Errors { get; }
        List<IValidationResultItem> Warnings { get; }
        List<IValidationResultItem> Infos { get; }
    }


    public interface IUpdateResult<T> : IResult
    {
        IReadOnlyList<T> ModifiedItems { get; }
        IReadOnlyList<T> AddedItems { get; }
        IReadOnlyList<T> DeletedItems { get; }

        void MergeWith(IUpdateResult<T> with);

        void Modified(T item);
        void Added(T item);
        void Deleted(T item);
    }

    public interface IUpdateResult : IUpdateResult<object>
    {

    }

    public class UpdateResult<T> : IUpdateResult<T>
    {
        List<T> _modifiedItems { get; } = new List<T>();
        List<T> _addedItems { get; } = new List<T>();
        List<T> _deletedItems { get; } = new List<T>();



        public bool Success { get; set; }
        public string Description { get; set; }
        public IReadOnlyList<T> ModifiedItems => this._modifiedItems.AsReadOnly();
        public IReadOnlyList<T> AddedItems => this._addedItems.AsReadOnly();
        public IReadOnlyList<T> DeletedItems => this._deletedItems.AsReadOnly();


        public void Added(T item)
        {
            this._addedItems.Add(item);
        }

        public void Deleted(T item)
        {
            this._deletedItems.Add(item);
        }

        public void Modified(T item)
        {
            this._modifiedItems.Add(item);
        }

        public void MergeWith(IUpdateResult<T> with)
        {
            this._modifiedItems.AddRange(with.ModifiedItems);
            this._addedItems.AddRange(with.AddedItems);
            this._deletedItems.AddRange(with.DeletedItems);
        }
    }

    public class UpdateResult : UpdateResult<object>, IUpdateResult
    {
        public static UpdateResult AddedMany(params object[] items)
        {
            var result = new UpdateResult();
            result.Added(items);
            return result;
        }

        public static UpdateResult ModifiedMany(params object[] items)
        {
            var result = new UpdateResult();
            result.Modified(items);
            return result;
        }

        public static UpdateResult DeletedMany(params object[] items)
        {
            var result = new UpdateResult();
            result.Deleted(items);
            return result;
        }
    }

    public interface IExceptionManager
    {
        Exception Translate(Exception ex);
    }

    public interface IExceptionTranslator
    {
        Exception Translate(Exception ex);

    }

    public interface IComponent
    {
        [YamlMember(Order = -9999)]
        [JsonPropertyOrder(-9999)]
        string Name { get; }

        [YamlMember(Order = -9998)]
        [JsonPropertyOrder(-9998)]
        string NavigationName { get; }
    }

    public interface IOrderedComponent : IComponent
    {
        int Index { get; }
    }

    public delegate void TimerEventDelegate(string name, DateTimeOffset time, object state);

    public interface ITimeProvider
    {
        DateTimeOffset Now { get; }

        event TimerEventDelegate OnEvent;

        void Setup();
        void CallAfter(int msLater, Action<DateTimeOffset> callback);
        void StopCall();
    }

}
