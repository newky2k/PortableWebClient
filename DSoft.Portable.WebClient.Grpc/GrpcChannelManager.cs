using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Grpc.Net.Client;
using Grpc.Net.Client.Web;

namespace DSoft.Portable.WebClient.Grpc;

/// <summary>
/// Default <see cref="IGrpcChannelManager"/>. Caches one channel per address (keyed with the HTTP mode it
/// was built for) and rebuilds it if a later call requests a different mode.
/// </summary>
/// <seealso cref="DSoft.Portable.WebClient.Grpc.IGrpcChannelManager" />
public class GrpcChannelManager : IGrpcChannelManager
{
    private readonly Dictionary<string, (GrpcChannel Channel, HttpMode Mode)> channels = new Dictionary<string, (GrpcChannel channel, HttpMode mode)>();

    /// <summary>
    /// Shuts down and removes the cached channel for the given address, if one exists.
    /// </summary>
    /// <param name="address">The server address whose channel should be cleared.</param>
    public async Task ClearAsync(string address)
    {
        if (channels.ContainsKey(address))
        {
            var data = channels[address];

            await data.Channel.ShutdownAsync();

            channels.Remove(address);

        }
    }

    /// <summary>
    /// Returns the cached channel for the address when its HTTP mode matches; otherwise builds a new channel
    /// (replacing any cached one built for a different mode) and caches it.
    /// </summary>
    /// <param name="address">The server address the channel connects to.</param>
    /// <param name="options">The options controlling how the channel is created.</param>
    /// <returns>A shared gRPC channel for the address.</returns>
    /// <exception cref="System.Exception">Thrown when the options specify an unsupported HTTP mode.</exception>
    public GrpcChannel ForAddress(string address, GrpcClientOptions options)
    {
        if (channels.ContainsKey(address))
        {
            var data = channels[address];

            if (options.GrpcMode == data.Mode)
                return channels[address].Channel;
            else
                channels.Remove(address);
        }

        GrpcChannelOptions grpcChannelOptions = BuildOptions(options);

        var channel = GrpcChannel.ForAddress(address, grpcChannelOptions);
        channels.Add(address, new(channel, options.GrpcMode));

        return channel;
    }

    /// <summary>
    /// Translates <see cref="GrpcClientOptions"/> into the gRPC channel options for the selected HTTP mode,
    /// wiring up the gRPC-Web handler for HTTP/1.1 and applying any custom or disabled certificate validation.
    /// </summary>
    /// <param name="options">The client options to translate.</param>
    /// <returns>The channel options to create the channel with.</returns>
    /// <exception cref="System.Exception">Thrown when the options specify an unsupported HTTP mode.</exception>
    private GrpcChannelOptions BuildOptions(GrpcClientOptions options)
    {
        GrpcChannelOptions grpcChannelOptions = null;

        switch (options.GrpcMode)
        {
            case HttpMode.Http_1_1:
            {
                if (options.HttpMessageHandler != null)
                {
                    return new GrpcChannelOptions
                    {
                        HttpHandler = new GrpcWebHandler(options.HttpMessageHandler)
                    };
                }

                //if a custom validator has be provided use that
                if (options.ServerCertificateCustomValidationCallback != null)
                {
                    var httpClientHandlerCustom = new HttpClientHandler();
                    httpClientHandlerCustom.ServerCertificateCustomValidationCallback = options.ServerCertificateCustomValidationCallback;

                    grpcChannelOptions = new GrpcChannelOptions
                    {
                        HttpHandler = new GrpcWebHandler(httpClientHandlerCustom)
                    };

                }
                else if (options.DisableSSLCertValidation)
                {
                    //return channel with SSL cert validation disabled
                    var httpClientHandler = new HttpClientHandler();
#if NET6_0_OR_GREATER
                    httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
#else
                        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
#endif
                    grpcChannelOptions = new GrpcChannelOptions
                    {
                        HttpHandler = new GrpcWebHandler(httpClientHandler)
                    };

                }
                else
                {
                    //if disable SSL certifiate validation has not been set them return standard channel generator
                    grpcChannelOptions = new GrpcChannelOptions
                    {
                        HttpHandler = new GrpcWebHandler(new HttpClientHandler())
                    };

                }
                break;
            }
            case HttpMode.Http_2_0:
            {
                if (options.HttpMessageHandler != null)
                {
                    return new GrpcChannelOptions()
                    {
                        HttpClient = new HttpClient(options.HttpMessageHandler)
                    };
                }

                //if a custom validator has be provided use that
                if (options.ServerCertificateCustomValidationCallback != null)
                {
                    var httpClientHandlerCustom = new HttpClientHandler();

                    httpClientHandlerCustom.ServerCertificateCustomValidationCallback = options.ServerCertificateCustomValidationCallback;

                    grpcChannelOptions = new GrpcChannelOptions()
                    {
                        HttpClient = new HttpClient(httpClientHandlerCustom)
                    };

                }
                else if (options.DisableSSLCertValidation)
                {
                    //return channel with SSL cert validation disabled
                    var httpClientHandler = new HttpClientHandler();
#if NET6_0_OR_GREATER
                    httpClientHandler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
#else
                        httpClientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => { return true; };
#endif
                    grpcChannelOptions = new GrpcChannelOptions()
                    {
                        HttpClient = new HttpClient(httpClientHandler)
                    };
                }
                else
                {
                    //if disable SSL certifiate validation has not been set them return standard channel generator
                    grpcChannelOptions = new GrpcChannelOptions();
                }
                break;
            }
            default:
                throw new Exception("Unexpected HTTP mode for Grpc Channel");
        }

        return grpcChannelOptions;
    }

}
