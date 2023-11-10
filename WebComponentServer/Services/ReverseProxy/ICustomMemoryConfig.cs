using Yarp.ReverseProxy.Configuration;

namespace WebComponentServer.Services.ReverseProxy;

public interface ICustomMemoryConfig : IProxyConfig
{
    void SignalChange();
}