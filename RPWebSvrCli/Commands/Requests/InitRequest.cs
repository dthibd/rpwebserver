using MediatR;

namespace RPWebSvrCli.Commands.Requests;

public class InitRequest(string path) : IRequest
{
    public string Path { get; } = path;
}
