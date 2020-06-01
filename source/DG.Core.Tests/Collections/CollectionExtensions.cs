using FluentAssertions.Common;
using Moq;
using System;

namespace DG.Core.Tests.Collections
{
    public static class CollectionExtensions
    {
        public static T GetOrMock<T>(this object[] collection, bool isExplicit = false)
            where T : class
        {
            foreach (var item in collection)
            {
                if (item.GetType().IsSameOrInherits(typeof(T)))
                {
                    return (T)item;
                }
            }

            if (isExplicit)
            {
                throw new ArgumentException($"This dependency is explicit {typeof(T)}. Please define and pass it from test arrange", nameof(T));
            }

            return new Mock<T>().Object;
        }
    }
}
