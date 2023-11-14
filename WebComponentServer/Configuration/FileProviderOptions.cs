namespace WebComponentServer.Configuration;

public sealed class FileProviderOptions
{
    public string FilePath { get; set; }
    public string BaseUrl { get; set; }

    public FileProviderOptions(string filepath, string baseUrl)
    {
        FilePath = filepath;
        BaseUrl = baseUrl;
    }
}
