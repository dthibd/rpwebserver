namespace WebComponentServer.Services.ReverseProxy;

public interface IReverseProxyChangesMonitor
{
    void Update();
    
    IObservable<IReverseProxyChangesMonitor> UpdateObservable { get; }
}
