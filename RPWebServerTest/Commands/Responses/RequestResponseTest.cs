using RPWebServer.Commands.Responses;

namespace RPWebServerTest.Commands.Responses;

public class RequestResponseTest
{
    [Fact]
    public void SuccessResponse()
    {
        var response = new RequestResponse();

        Assert.True(response.Succeeded);
        Assert.Null(response.Error);
    }

    [Fact]
    public void SuccessResponseUsingValue()
    {
        var response = new RequestResponse(true);

        Assert.True(response.Succeeded);
        Assert.Null(response.Error);
    }

    [Fact]
    public void ErrorResponse()
    {
        var response = new RequestResponse(false, "error");

        Assert.False(response.Succeeded);
        Assert.Equal("error", response.Error);
    }

    [Fact]
    public void SuccessResponseWithType()
    {
        var response = new RequestResponse<string>("value");

        Assert.True(response.Succeeded);
        Assert.Null(response.Error);
        Assert.Equal("value", response.Value);
    }

    [Fact]
    public void SuccessResponseWithTypeAndValue()
    {
        var response = new RequestResponse<string>(true, "value");

        Assert.True(response.Succeeded);
        Assert.Null(response.Error);
    }
    
    [Fact]
    public void FailureResponseWithType()
    {
        var response = new RequestResponse<string>(false, "error");

        Assert.False(response.Succeeded);
        Assert.Equal("error", response.Error);
        Assert.Null(response.Value);
    }
}