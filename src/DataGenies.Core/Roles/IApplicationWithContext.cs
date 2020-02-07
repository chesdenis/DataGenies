using System.Collections;
using System.Collections.Generic;
using DataGenies.Core.Containers;

namespace DataGenies.Core.Roles
{
    public interface IApplicationWithContext
    {
        IContainer ContextContainer { get; }
    }
}