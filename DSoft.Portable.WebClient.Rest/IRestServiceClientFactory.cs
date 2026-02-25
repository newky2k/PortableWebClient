
using System;

namespace DSoft.Portable.WebClient.Rest;

/// <summary>
/// Defines a factory for creating instances of REST service clients with a specified base address.
/// </summary>
/// <remarks>Implementations of this interface enable the creation of different types of REST service clients,
/// allowing for flexibility in client configuration and instantiation. The created clients are expected to be
/// configured to communicate with the provided base address. This interface is typically used to abstract the
/// construction and setup of REST clients in applications that interact with multiple RESTful services.</remarks>
public interface IRestServiceClientFactory
{
    /// <summary>
    /// Get services client
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="uniqueId">Unique id or connection key</param>
    /// <returns></returns>
    T GetClient<T>(string uniqueId = null) where T : IRestServiceClient;

    /// <summary>
    /// Get services client
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="baseAddress">Base Url for the service client</param>
    /// <param name="uniqueId">Unique id or connection key</param>
    /// <returns></returns>
    T GetClient<T>(Uri baseAddress, string uniqueId = null) where T : IRestServiceClient;
}
