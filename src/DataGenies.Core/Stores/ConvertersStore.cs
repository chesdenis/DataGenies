using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DataGenies.Core.Converters;

namespace DataGenies.Core.Stores
{
    public class ConvertersStore : Dictionary<ConverterType, IEnumerable<IConverter>>
    {
        public ConvertersStore(IEnumerable<IConverter> converters)
        {
            var storeItems = converters.ToList();
            
            this.Add(ConverterType.AfterReceive,  storeItems.Where(w => w.Type == ConverterType.AfterReceive));
            this.Add(ConverterType.BeforePublish,  storeItems.Where(w => w.Type == ConverterType.BeforePublish));
        }
    }
}