namespace DataGenies.AspNetCore.DataGeniesCore.Converters
{
    public interface IConverter
    {
        ConverterType Type { get; set; }

        byte[] Convert(byte[] data);
    }
}