namespace Infrastructure.OpenApi;


[AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
public class BaseHeaderAttribute : Attribute
{
    public string HeaderName { get; set; }
    public string Description { get; set; }
    public string DefaultValue { get; set; }
    public bool IsRequired { get; set; }
}
