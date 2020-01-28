using System.Collections.Generic;

namespace DataGenies.AspNetCore.DataGeniesCore.Converters
{
    public interface IMappingConverter : IConverter
    {
        IDictionary<string, string> Mapping { get; }
    }
}