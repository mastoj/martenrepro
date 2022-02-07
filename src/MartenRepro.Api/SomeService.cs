namespace MartenRepro.Api;

public interface ISomeService
{
    string GetSomeValue(string value);
}

public class SomeService : ISomeService
{
    public string GetSomeValue(string value)
    {
        return $"Some {value}";
    }
}
