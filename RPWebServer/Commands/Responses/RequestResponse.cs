namespace RPWebServer.Commands.Responses;

public class RequestResponse
{
    public string? Error { get; }
    public bool Succeeded { get; }

    public RequestResponse()
    {
        Succeeded = true;
    }
    
    public RequestResponse(bool success = false, string? error = null)
    {
        Error = success ? null : error;
        Succeeded = success;
    }
}


public class RequestResponse<TYPE> : RequestResponse
{
    public TYPE? Value { get; }

    public RequestResponse(TYPE value) : base()
    {
        Value = value;
    }

    public RequestResponse(bool success, string error) : base(success, error)
    {

    }
}
