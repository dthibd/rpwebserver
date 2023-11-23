using Yarp.ReverseProxy.Configuration;

namespace RPWebServer.Services.ReverseProxy;

public interface ICustomMemoryConfig : IProxyConfig
{
    void SignalChange();
}