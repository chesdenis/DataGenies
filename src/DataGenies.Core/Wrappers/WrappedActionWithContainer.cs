using System;
using DataGenies.Core.Containers;

namespace DataGenies.Core.Wrappers
{
    public delegate void WrappedActionWithContainer(Action<IContainer> action, IContainer arg);
}