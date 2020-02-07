using System.Collections;
using System.Collections.Generic;
using DataGenies.Core.Containers;

namespace DataGenies.Core.Roles
{
    public interface IApplicationWithStateContainer
    {
        IStateContainer StateContainer { get; }
    }
}