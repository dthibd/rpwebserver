using Ardalis.GuardClauses;

namespace WebComponentServer.Guards;

public static class DictionaryGuards
{
    public static IGuardClause DictionaryContainsKey<T, TV>(this IGuardClause guardClause,
        Dictionary<T, TV> dictionary,
        T key,
        string message = "") 
        where T : notnull
    {
        if (!dictionary.ContainsKey(key)) return guardClause;
        
        if (string.IsNullOrEmpty(message))
        {
            message = $"key {key} not found in dictionary";
        }
            
        throw new ArgumentException(message);

    }
    public static IGuardClause DictionaryDoesNotContainsKey<T, TV>(this IGuardClause guardClause,
        Dictionary<T, TV> dictionary,
        T key,
        string message = "") 
        where T : notnull
    {
        if (dictionary.ContainsKey(key)) return guardClause;
        
        if (string.IsNullOrEmpty(message))
        {
            message = $"key {key} already present in dictionary";
        }
            
        throw new ArgumentException(message);

    }    
}