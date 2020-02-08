using System.Collections.Generic;
using DataGenies.Core.Converters;
using DataGenies.Core.Models;

namespace DataGenies.Core.Scanners
{
    public interface IApplicationConvertersScanner
    {
        IEnumerable<ConverterEntity> ScanConverters();

        IEnumerable<IConverter> GetConvertersInstances(IEnumerable<ConverterEntity> converters);
    }
}