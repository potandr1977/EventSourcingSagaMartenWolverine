namespace Infrastructure;

public record MartenSettings
{
    public string WriteSchema { get; set; }
    public string ReadSchema { get; set; }
}
