namespace Application.Interfaces.Tenancy.Models;

public class TenantDto
{
    public string Id { get; set; }
    public string Identifier { get; set; }
    public string Name { get; set; }
    public string ConnectionString { get; set; }
    public string AdminEmail { get; set; }
    public DateTime ValidUpTo { get; set; }
    public bool IsActive { get; set; }
}
