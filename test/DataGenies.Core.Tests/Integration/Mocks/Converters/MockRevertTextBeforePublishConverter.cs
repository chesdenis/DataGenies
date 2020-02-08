using System.Linq;
using System.Text;
using DataGenies.Core.Attributes;
using DataGenies.Core.Converters;

namespace DataGenies.Core.Tests.Integration.Mocks.Converters
{
    [ConverterTemplate]
    public class MockRevertTextBeforePublishConverter : IConverter
    {
        public ConverterType Type { get; set; } = ConverterType.BeforePublish;

        public byte[] Convert(byte[] data)
        {
            var testString = Encoding.UTF8.GetString(data);
            var convertedString = new string(testString.Reverse().ToArray());
            return Encoding.UTF8.GetBytes(convertedString);
        }
    }
}