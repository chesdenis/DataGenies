using System;
using DataGenies.Core.Containers;

namespace DataGenies.Core.Wrappers
{
    public delegate void WrappedActionWithContainer(Action<IContainer> action, IContainer arg);

    public delegate void WrappedTypedAction<T>(Action<T> action, T arg) where T : class;
}