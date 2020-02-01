using System.Collections.Generic;

namespace DataGenies.Core.Converters
{
    public interface IMappingConverter : IConverter
    {
        IDictionary<string, string> Mapping { get; }
    }
}