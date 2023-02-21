namespace Infrastructure.Services.Abstractions
{
    public interface IJsonSerializer
    {
        string Serialize<T>(T data);

        T Deserialize<T>(string value);
    }
}
