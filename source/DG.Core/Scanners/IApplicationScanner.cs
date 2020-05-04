using System;
using System.Collections.Generic;

namespace DG.Core.Scanners
{
    public interface IApplicationScanner
    {
        IEnumerable<Type> Scan();
    }
}