using System;
using System.Collections.Generic;

namespace DG.Core.Scanners
{
    public interface IApplicationTypesScanner
    {
        IEnumerable<Type> Scan();
    }
}