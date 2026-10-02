namespace Web.Shared.DataTests;

public record DataTest(string Key, object? Value = null)
{
    public const string DataTestAttributeKey = "data-test";
    public const string DataTestAttributeValue = "data-test-value";
    public DataTest For(object value) => this with
    {
        Value = value
    };
    public override string ToString() => Key;
    public static implicit operator DataTest(string key) => new(key);
    public static implicit operator Dictionary<string, object?>(DataTest dataTest)
    {
        Dictionary<string, object?> result = new()
        {
            {
                DataTestAttributeKey, dataTest.Key
            }
        };

        if (dataTest.Value is not null)
        {
            result[DataTestAttributeValue] = dataTest.Value.ToString();
        }

        return result;
    }
}
