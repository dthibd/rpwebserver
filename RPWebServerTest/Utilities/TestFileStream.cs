using System.IO.Abstractions;

namespace WebComponentServerTest.Utilities;

public class TestFileStream : FileSystemStream
{
    public TestFileStream(Stream stream, string path, bool isAsync) : base(stream, path, isAsync)
    {
    }
}

