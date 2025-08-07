using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using FluentAssertions.Specialized;
using FluentAssertions.Primitives;
using System.Diagnostics.CodeAnalysis;


namespace Rapidex
{
    public static class AssertionHelper
    {
        //https://stackoverflow.com/questions/61384377/how-to-suppress-possible-null-reference-warnings
        public static T NotNull<T>([NotNull] this T obj, string message = null)
        {
            if (obj == null)
                throw new InvalidOperationException(message ?? "Object is null");

            //obj.Should().NotBeNull(message);
            return obj;
        }

        public static T NotNull<T, E>([NotNull] this T obj, string message = null) where E : Exception, new()
        {
            if (obj == null)
                throw new InvalidOperationException(message ?? "Object is null");

            //.Should().NotBeNull(message);

            //            if (obj == null)
            //            {
            //#pragma warning disable CS8597 // Thrown value may be null.
            //                throw Activator.CreateInstance(typeof(E), message) as E;
            //#pragma warning restore CS8597 // Thrown value may be null.
            //            }

            return obj;
        }

        public static T NotEmpty<T>([NotNull] this T obj, string message = null)
        {
            obj.NotNull(message);
            //Should().NotBeNull(message);
            if (obj.IsNullOrEmpty())
                throw new InvalidOperationException(message ?? "Object is empty");

            //.Should().BeFalse(message);

            return obj;
        }

        public static T ShouldBeSuccess<T>(this T obj, string message = null) where T : IResult
        {
            obj.NotNull(message);

            if (!obj.Success)
            {
                throw new BaseValidationException(message ?? "Result fail");
            }

            return obj;
        }

        //public static TObj ShouldSupportTo<TObj>(this object obj, Type type, string message = null)
        //{
        //    obj.IsSupportTo(type).Should().BeTrue(message);

        //    return (TObj)obj;
        //}

        public static TObj ShouldSupportTo<TObj>(this object obj, string message = null)
        {
            message = message ?? $"Object does not support to {typeof(TObj).Name}, obj type is '{obj?.GetType().Name}'";

            if (!obj.IsSupportTo(typeof(TObj)))
                throw new InvalidOperationException(message ?? $"Object '{obj}' is not support to {typeof(TObj).Name}");

            //.Should().BeTrue(message);
            return (TObj)obj;
        }

        public static void ShouldSupportTo(this Type type, Type desiredType, string message = null)
        {
            message = message ?? $"Type does not support to {desiredType.Name}, type is '{type.Name}'";

            if (!type.IsSupportTo(desiredType))
                throw new InvalidOperationException(message ?? $"Type '{type.Name}' is not support to {desiredType.Name}");

            //.Should().BeTrue(message);
        }

        public static void ShouldSupportTo<TType>(this Type type, string message = null)
        {
            message = message ?? $"Type does not support to {typeof(TType).Name}, type is '{type.Name}'";

            type.ShouldSupportTo(typeof(TType), message);
        }

        public static void ShouldNotSupportTo<TObj>(this object obj, string message = null)
        {
            message = message ?? $"Object does support to {typeof(TObj).Name}, obj type is '{obj?.GetType().Name}'";

            if (obj.IsSupportTo(typeof(TObj)))
                throw new InvalidOperationException(message);

        }
    }
}
