using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DSoft.Portable.WebClient.Rest.Encryption;

namespace DSoft.Portable.WebClient.Rest;

/// <summary>
/// Convenience extensions on <see cref="RestServiceSecureClientBase"/> for building and executing
/// encrypted ("secure") POST requests, so callers can send and receive <see cref="SecurePayload"/>-wrapped
/// data without assembling the request envelope by hand.
/// </summary>
public static class RestServiceClientBaseExtensions
{
    #region Rest Request Builders

    /// <summary>
    /// Builds a POST whose body is a <see cref="SecureRequest"/> carrying <paramref name="data"/> encrypted into its payload.
    /// </summary>
    /// <param name="target">The secure client the request is built for.</param>
    /// <param name="action">The action/method name to call.</param>
    /// <param name="data">The object to encrypt and send as the payload.</param>
    /// <param name="tokenId">The token/request identifier placed on the envelope.</param>
    /// <param name="encryptionToken">The pass phrase used to encrypt the payload.</param>
    /// <param name="parameterString">Optional query/route string appended to the URL.</param>
    /// <param name="controllerOverride">Optional controller name to use instead of the default.</param>
    /// <param name="headers">Optional custom headers to add to the request.</param>
    /// <param name="serviceOverride">Optional service segment to use instead of the default.</param>
    /// <returns>The composed POST request.</returns>
    public static HttpRequestMessage BuildUserPostRequest(this RestServiceSecureClientBase target, string action, object data, string tokenId, string encryptionToken, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null, string serviceOverride = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, target.CalculateUrlForMethod(action, parameterString: parameterString, controllerOverride: controllerOverride, serviceOverride: serviceOverride));

        target.ApplyHeaders(request, headers);

        request.Content = new StringContent(JsonSerializer.Serialize(target.CreateUserRequest(data, tokenId, encryptionToken), target.Options.JsonSerializerOptions), Encoding.UTF8, "application/json");

        return request;
    }

    /// <summary>
    /// Builds a POST whose body is a <see cref="SecureRequest"/> with an encrypted empty payload, for calls that send no data.
    /// </summary>
    /// <param name="target">The secure client the request is built for.</param>
    /// <param name="action">The action/method name to call.</param>
    /// <param name="tokenId">The token/request identifier placed on the envelope.</param>
    /// <param name="encryptionToken">The pass phrase used to encrypt the payload.</param>
    /// <param name="parameterString">Optional query/route string appended to the URL.</param>
    /// <param name="controllerOverride">Optional controller name to use instead of the default.</param>
    /// <param name="headers">Optional custom headers to add to the request.</param>
    /// <param name="serviceOverride">Optional service segment to use instead of the default.</param>
    /// <returns>The composed POST request.</returns>
    public static HttpRequestMessage BuildEmptyUserPostRequest(this RestServiceSecureClientBase target, string action, string tokenId, string encryptionToken, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null, string serviceOverride = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, target.CalculateUrlForMethod(action, parameterString: parameterString, controllerOverride: controllerOverride, serviceOverride: serviceOverride));

        target.ApplyHeaders(request, headers);

        request.Content = new StringContent(JsonSerializer.Serialize(target.CreateEmptyUserRequest(tokenId, encryptionToken), target.Options.JsonSerializerOptions), Encoding.UTF8, "application/json");

        return request;
    }

    /// <summary>
    /// Builds a POST whose body is a <see cref="SecureBinaryRequest"/> carrying both an encrypted payload and an encrypted binary blob.
    /// </summary>
    /// <param name="target">The secure client the request is built for.</param>
    /// <param name="action">The action/method name to call.</param>
    /// <param name="data">The object to encrypt and send as the payload.</param>
    /// <param name="binary">The raw bytes to encrypt and attach to the request.</param>
    /// <param name="tokenId">The token/request identifier placed on the envelope.</param>
    /// <param name="encryptionToken">The pass phrase used to encrypt the payload and binary content.</param>
    /// <param name="parameterString">Optional query/route string appended to the URL.</param>
    /// <param name="controllerOverride">Optional controller name to use instead of the default.</param>
    /// <param name="headers">Optional custom headers to add to the request.</param>
    /// <param name="serviceOverride">Optional service segment to use instead of the default.</param>
    /// <returns>The composed POST request.</returns>
    public static HttpRequestMessage BuildUserBinaryPostRequest(this RestServiceSecureClientBase target, string action, object data, byte[] binary, string tokenId, string encryptionToken, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null, string serviceOverride = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, target.CalculateUrlForMethod(action, parameterString: parameterString, controllerOverride: controllerOverride, serviceOverride: serviceOverride));

        target.ApplyHeaders(request, headers);

        request.Content = new StringContent(JsonSerializer.Serialize(target.CreateUserBinaryRequest(data, binary, tokenId, encryptionToken), target.Options.JsonSerializerOptions), Encoding.UTF8, "application/json");

        return request;
    }

    /// <summary>
    /// Builds a POST with an inline <see cref="SecureRequest"/> envelope (stamped with the client version) carrying <paramref name="data"/> encrypted into its payload.
    /// </summary>
    /// <param name="target">The secure client the request is built for.</param>
    /// <param name="action">The action/method name to call.</param>
    /// <param name="data">The object to encrypt and send as the payload.</param>
    /// <param name="tokenId">The token/request identifier placed on the envelope.</param>
    /// <param name="encryptionToken">The pass phrase used to encrypt the payload.</param>
    /// <param name="parameterString">Optional query/route string appended to the URL.</param>
    /// <param name="controllerOverride">Optional controller name to use instead of the default.</param>
    /// <param name="headers">Optional custom headers to add to the request.</param>
    /// <param name="serviceOverride">Optional service segment to use instead of the default.</param>
    /// <returns>The composed POST request.</returns>
    public static HttpRequestMessage BuildSecurePostRequest(this RestServiceSecureClientBase target, string action, object data, string tokenId, string encryptionToken, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null, string serviceOverride = null)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, target.CalculateUrlForMethod(action, parameterString: parameterString, controllerOverride: controllerOverride, serviceOverride: serviceOverride));

        target.ApplyHeaders(request, headers);

        var fReq = new SecureRequest()
        {
            ClientVersionNo = target.ClientVersionNo,
            Id = tokenId,
            Payload = new SecurePayload(data, encryptionToken, target.InitVector, target.KeySize),
        };

        request.Content = new StringContent(JsonSerializer.Serialize(fReq, target.Options.JsonSerializerOptions), Encoding.UTF8, "application/json");

        return request;
    }

    #endregion

    #region SecureRequest Builders

    /// <summary>
    /// Creates a <see cref="SecureRequest"/> with an encrypted empty payload, stamped with the client version.
    /// </summary>
    /// <param name="target">The secure client supplying the version, IV, and key size.</param>
    /// <param name="tokenId">The token/request identifier placed on the envelope.</param>
    /// <param name="encryptionToken">The pass phrase used to encrypt the payload.</param>
    /// <returns>The populated secure request.</returns>
    public static SecureRequest CreateEmptyUserRequest(this RestServiceSecureClientBase target, string tokenId, string encryptionToken)
    {
        return new SecureRequest()
        {
            ClientVersionNo = target.ClientVersionNo,
            Id = tokenId,
            Payload = new SecurePayload(encryptionToken, target.InitVector, target.KeySize),
        };
    }

    /// <summary>
    /// Creates a <see cref="SecureRequest"/> carrying <paramref name="data"/> encrypted into its payload, stamped with the client version.
    /// </summary>
    /// <param name="target">The secure client supplying the version, IV, and key size.</param>
    /// <param name="data">The object to encrypt into the payload.</param>
    /// <param name="tokenId">The token/request identifier placed on the envelope.</param>
    /// <param name="encryptionToken">The pass phrase used to encrypt the payload.</param>
    /// <returns>The populated secure request.</returns>
    public static SecureRequest CreateUserRequest(this RestServiceSecureClientBase target, object data, string tokenId, string encryptionToken)
    {
        return new SecureRequest()
        {
            ClientVersionNo = target.ClientVersionNo,
            Id = tokenId,
            Payload = new SecurePayload(data, encryptionToken, target.InitVector, target.KeySize),
        };
    }

    /// <summary>
    /// Creates a <see cref="SecureBinaryRequest"/> carrying both an encrypted payload and an encrypted binary blob, stamped with the client version.
    /// </summary>
    /// <param name="target">The secure client supplying the version, IV, and key size.</param>
    /// <param name="data">The object to encrypt into the payload.</param>
    /// <param name="binary">The raw bytes to encrypt and attach.</param>
    /// <param name="tokenId">The token/request identifier placed on the envelope.</param>
    /// <param name="encryptionToken">The pass phrase used to encrypt the payload and binary content.</param>
    /// <returns>The populated secure binary request.</returns>
    public static SecureBinaryRequest CreateUserBinaryRequest(this RestServiceSecureClientBase target, object data, byte[] binary, string tokenId, string encryptionToken)
    {
        var request = new SecureBinaryRequest()
        {
            ClientVersionNo = target.ClientVersionNo,
            Id = tokenId,
            Payload = new SecurePayload(data, encryptionToken, target.InitVector, target.KeySize),
        };

        request.SetBinaryObject(binary, encryptionToken, target.InitVector, target.KeySize);

        return request;
    }

    #endregion

    #region Request execution methods

    /// <summary>
    /// Sends an encrypted payload and awaits a <see cref="SecureResponse"/>, throwing if the server reports failure. Use for calls that return no data.
    /// </summary>
    /// <param name="target">The secure client to send through.</param>
    /// <param name="methodName">The action/method name to call.</param>
    /// <param name="data">The object to encrypt and send as the payload.</param>
    /// <param name="tokenId">The token/request identifier placed on the envelope.</param>
    /// <param name="encryptionToken">The pass phrase used to encrypt the payload.</param>
    /// <param name="parameterString">Optional query/route string appended to the URL.</param>
    /// <param name="controllerOverride">Optional controller name to use instead of the default.</param>
    /// <param name="headers">Optional custom headers to add to the request.</param>
    /// <returns>A task that completes when the call succeeds.</returns>
    /// <exception cref="System.Exception">Thrown when the server reports the call as unsuccessful.</exception>
    public static async Task ExecuteSecureCallAsync(this RestServiceSecureClientBase target, string methodName, object data, string tokenId, string encryptionToken, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null)
    {
        var request = target.BuildUserPostRequest(methodName, data, tokenId, encryptionToken, parameterString, controllerOverride, headers);

        target.ApplyHeaders(request);

        var result = await target.ExecuteAsync<SecureResponse>(request);

        if (result.Success == false)
            throw new Exception(result.Message);
    }

    /// <summary>
    /// Sends an empty encrypted request and returns the decrypted response payload as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type the encrypted response payload decrypts to.</typeparam>
    /// <param name="target">The secure client to send through.</param>
    /// <param name="methodName">The action/method name to call.</param>
    /// <param name="tokenId">The token/request identifier placed on the envelope.</param>
    /// <param name="encryptionToken">The pass phrase used to encrypt the request and decrypt the response.</param>
    /// <param name="parameterString">Optional query/route string appended to the URL.</param>
    /// <param name="controllerOverride">Optional controller name to use instead of the default.</param>
    /// <param name="headers">Optional custom headers to add to the request.</param>
    /// <returns>The decrypted response payload.</returns>
    /// <exception cref="System.Exception">Thrown when the server reports the call as unsuccessful.</exception>
    public static async Task<T> ExecuteSecureCallAsync<T>(this RestServiceSecureClientBase target, string methodName, string tokenId, string encryptionToken, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null)
    {
        var request = target.BuildEmptyUserPostRequest(methodName, tokenId, encryptionToken, parameterString, controllerOverride, headers);

        target.ApplyHeaders(request);

        var result = await target.ExecuteAsync<SecureResponse>(request);

        if (result.Success == false)
            throw new Exception(result.Message);

        var payload = result.Payload.Extract<T>(encryptionToken, target.InitVector, target.KeySize);

        return payload;
    }

    /// <summary>
    /// Sends an encrypted payload and returns the decrypted response payload as <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type the encrypted response payload decrypts to.</typeparam>
    /// <param name="target">The secure client to send through.</param>
    /// <param name="methodName">The action/method name to call.</param>
    /// <param name="data">The object to encrypt and send as the payload.</param>
    /// <param name="tokenId">The token/request identifier placed on the envelope.</param>
    /// <param name="encryptionToken">The pass phrase used to encrypt the request and decrypt the response.</param>
    /// <param name="parameterString">Optional query/route string appended to the URL.</param>
    /// <param name="controllerOverride">Optional controller name to use instead of the default.</param>
    /// <param name="headers">Optional custom headers to add to the request.</param>
    /// <returns>The decrypted response payload.</returns>
    /// <exception cref="System.Exception">Thrown when the server reports the call as unsuccessful.</exception>
    public static async Task<T> ExecuteSecureCallAsync<T>(this RestServiceSecureClientBase target, string methodName, object data, string tokenId, string encryptionToken, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null)
    {
        var request = target.BuildUserPostRequest(methodName, data, tokenId, encryptionToken, parameterString, controllerOverride, headers);

        target.ApplyHeaders(request);

        var result = await target.ExecuteAsync<SecureResponse>(request);

        if (result.Success == false)
            throw new Exception(result.Message);

        var payload = result.Payload.Extract<T>(encryptionToken, target.InitVector, target.KeySize);

        return payload;
    }

    /// <summary>
    /// Sends an encrypted payload together with an encrypted binary blob and awaits a <see cref="SecureResponse"/>, throwing if the server reports failure.
    /// </summary>
    /// <param name="target">The secure client to send through.</param>
    /// <param name="methodName">The action/method name to call.</param>
    /// <param name="data">The object to encrypt and send as the payload.</param>
    /// <param name="binary">The raw bytes to encrypt and attach.</param>
    /// <param name="tokenId">The token/request identifier placed on the envelope.</param>
    /// <param name="encryptionToken">The pass phrase used to encrypt the payload and binary content.</param>
    /// <param name="parameterString">Optional query/route string appended to the URL.</param>
    /// <param name="controllerOverride">Optional controller name to use instead of the default.</param>
    /// <param name="headers">Optional custom headers to add to the request.</param>
    /// <returns>A task that completes when the call succeeds.</returns>
    /// <exception cref="System.Exception">Thrown when the server reports the call as unsuccessful.</exception>
    public static async Task ExecuteSecureBinaryCallAsync(this RestServiceSecureClientBase target, string methodName, object data, byte[] binary, string tokenId, string encryptionToken, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null)
    {
        var request = target.BuildUserBinaryPostRequest(methodName, data, binary, tokenId, encryptionToken, parameterString, controllerOverride, headers);

        target.ApplyHeaders(request);

        var result = await target.ExecuteAsync<SecureResponse>(request);

        if (result.Success == false)
            throw new Exception(result.Message);
    }

    /// <summary>
    /// Sends an encrypted payload and returns a downloaded file: its name, MIME type, and (still-encrypted) bytes from the <see cref="SecureBinaryResponse"/>.
    /// </summary>
    /// <param name="target">The secure client to send through.</param>
    /// <param name="methodName">The action/method name to call.</param>
    /// <param name="data">The object to encrypt and send as the payload.</param>
    /// <param name="tokenId">The token/request identifier placed on the envelope.</param>
    /// <param name="encryptionToken">The pass phrase used to encrypt the payload.</param>
    /// <param name="parameterString">Optional query/route string appended to the URL.</param>
    /// <param name="controllerOverride">Optional controller name to use instead of the default.</param>
    /// <param name="headers">Optional custom headers to add to the request.</param>
    /// <returns>A tuple of the file name, MIME type, and binary content returned by the server.</returns>
    /// <exception cref="System.Exception">Thrown when the server reports the call as unsuccessful.</exception>
    public static async Task<(string FileName, string MimeType, byte[] Binary)> ExecuteSecureDownloadServiceCallAsync(this RestServiceSecureClientBase target, string methodName, object data, string tokenId, string encryptionToken, string parameterString = null, string controllerOverride = null, Dictionary<string, string> headers = null)
    {
        var request = target.BuildUserPostRequest(methodName, data, tokenId, encryptionToken, parameterString, controllerOverride, headers);

        target.ApplyHeaders(request);

        var result = await target.ExecuteAsync<SecureBinaryResponse>(request);

        if (result.Success == false)
            throw new Exception(result.Message);

        return (result.FileName, result.MimeType, result.Data);
    }

    #endregion
}
