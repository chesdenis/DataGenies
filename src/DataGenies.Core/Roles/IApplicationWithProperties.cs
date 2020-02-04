using System.Collections;
using System.Collections.Generic;

namespace DataGenies.Core.Roles
{
    public interface IApplicationWithProperties
    {
        IApplicationProperties Properties { get; }
    }
}