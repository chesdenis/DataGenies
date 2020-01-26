namespace DataGenies.AspNetCore.DataGeniesCore.Abstractions
{
    public interface IConverter
    {
        ConverterType Type { get; set; }

        byte[] Convert(byte[] data);
    }
}