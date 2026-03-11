using System.Net.Http;

namespace DSoft.Portable.WebClient.Rest;

public class PortableRestHttpClient : HttpClient
{
    private readonly HttpClient _httpClient;

    internal HttpClient HttpClient => _httpClient;

    public PortableRestHttpClient(HttpClient httpClient) : base()
    {
        _httpClient = httpClient;
    }

}
