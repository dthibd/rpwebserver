using Ardalis.GuardClauses;
using WebComponentServer.Guards;

namespace WebComponentServerTest.Guards
{
    public class DictionaryGuardsTest
    {
        [Fact]
        public void DictionaryContainsKey_KeyPresent()
        {
            var dict = new Dictionary<string, string>() 
            {
                {"test-key", "test-value"}
            };

            Assert.Throws<ArgumentException>(() => Guard.Against.DictionaryContainsKey(dict, "test-key"));
        }
        
        [Fact]
        public void DictionaryContainsKey_KeyPresent_WithMessage()
        {
            var dict = new Dictionary<string, string>() 
            {
                {"test-key", "test-value"}
            };

            var exception = Record.Exception(() => Guard.Against.DictionaryContainsKey(dict, "test-key", "test-message"));
            Assert.NotNull(exception);
            Assert.Equal("test-message", exception.Message);
        }

        [Fact]
        public void DictionaryContainsKey_KeyNotPresent()
        {
            var dict = new Dictionary<string, string>();

            var exception = Record.Exception(() => Guard.Against.DictionaryContainsKey(dict, "test-key"));
            Assert.Null(exception);
        }
        
        [Fact]
        public void DictionaryDoesNotContainsKey_KeyNotPresent()
        {
            var dict = new Dictionary<string, string>();

            var exception = Record.Exception(() => Guard.Against.DictionaryDoesNotContainsKey(dict, "test-key"));
            Assert.NotNull(exception);
        }
        
        [Fact]
        public void DictionaryDoesNotContainsKey_KeyNotPresent_WithMessage()
        {
            var dict = new Dictionary<string, string>();

            var exception = Record.Exception(() => Guard.Against.DictionaryDoesNotContainsKey(dict, "test-key", "test-message"));
            Assert.NotNull(exception);
            Assert.Equal("test-message", exception.Message);
        }
        
        [Fact]
        public void DictionaryDoesNotContainsKey_KeyPresent()
        {
            var dict = new Dictionary<string, string>() 
            {
                {"test-key", "test-value"}
            };

            var exception = Record.Exception(() => Guard.Against.DictionaryDoesNotContainsKey(dict, "test-key"));
            Assert.Null(exception);
        }
    }
}