using System.IO.Abstractions;
using Microsoft.Win32.SafeHandles;
using Moq;
using WebComponentServer.Core;
using WebComponentServer.Services;

namespace WebComponentServerTest;

public class TestFileStream : FileSystemStream
{
    public TestFileStream(Stream stream, string path, bool isAsync) : base(stream, path, isAsync)
    {
    }
}

public class ComponentFileProviderTest
{
    private IComponentProviderFactory ComponentProviderFactory { get; }
    private Mock<IFileSystem> FileSystemMock { get; }
    private Mock<IFile> FileMock { get; }
    
    private Mock<TestFileStream> OpenReadStreamMock { get; }

    public ComponentFileProviderTest()
    {
        FileSystemMock = new Mock<IFileSystem>();

        OpenReadStreamMock = new Mock<TestFileStream>(new FileStream(new SafeFileHandle(new IntPtr(1), true), FileAccess.Read), "test", true);
        
        FileMock = new Mock<IFile>();
        FileMock
            .Setup<FileSystemStream>(mk => mk.OpenRead(It.IsAny<string>()))
            .Returns(OpenReadStreamMock.Object);
        
        FileSystemMock
            .SetupGet<IFile>(mk => mk.File).Returns(FileMock.Object);

        ComponentProviderFactory = new ComponentProviderFactory(FileSystemMock.Object);
    }
    
    [Fact]
    public void BaseUrlEndsWithSlash()
    {
        var provider = ComponentProviderFactory.CreateFileProvider("testid", "path/", "/dev/home");
        
        Assert.Equal("path/", provider.BaseUrl);
        Assert.Equal("testid", provider.Id);
    }

    [Fact]
    public void BaseUrlNotEndingWithSlash()
    {
        var provider = ComponentProviderFactory.CreateFileProvider("testid", "path", "/dev/home");
        
        Assert.Equal("path/", provider.BaseUrl);
    }

    [Fact]
    public void MatchUrlAllLowercaseEqual()
    {
        var provider = ComponentProviderFactory.CreateFileProvider("testid", "path/", "/dev/home");
        var result = provider.MatchUrl("path/");

        Assert.True(result);
    }
    
    [Fact]
    public void MatchUrlDifferentCaseEqual()
    {
        var provider = ComponentProviderFactory.CreateFileProvider("testid", "path/", "/dev/home");
        var result = provider.MatchUrl("Path/");

        Assert.True(result);
    }

    [Fact]
    public void MatchUrlAllLowercaseNotEqual()
    {
        var provider = ComponentProviderFactory.CreateFileProvider("testid", "path/", "/dev/home");
        var result = provider.MatchUrl("another/path/");

        Assert.False(result);
    }

    [Fact]
    public void ContentResultThrowIfInvalidUrl()
    {
        var provider = ComponentProviderFactory.CreateFileProvider("testid", "path/", "/dev/home");
        
        Assert.Throws<Exception>(() => provider.GetContentForUrl("test/file.ts"));
    }

    [Fact]
    public void ContentResultValid()
    {
        var provider = ComponentProviderFactory.CreateFileProvider("testid", "path/", "/dev/home");
        var result = provider.GetContentForUrl("path/main.js");

        Assert.NotNull(result.Content);
    }
}