using Web.Shared.DataTests;

namespace AcceptanceTests.Extensions;

public class DataTestSelector(DataTest dataTest)
{
    public override string ToString() => $"[{DataTest.DataTestAttributeKey}=\"{dataTest.Key}\"]{(dataTest.Value is null ? string.Empty : $"[{DataTest.DataTestAttributeValue}=\"{dataTest.Value}\"]")}";
    public static implicit operator DataTestSelector(DataTest dataTest) => new(dataTest);
    public static implicit operator string(DataTestSelector dataTest) => dataTest.ToString();
}
