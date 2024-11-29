namespace Infrastructure.Identity.Constants;

using System.Collections.ObjectModel;

public static class RoleConstants
{
    public const string SuperAdmin = nameof(SuperAdmin);
    public const string Admin = nameof(Admin);
    public const string Basic = nameof(Basic);

    public static IReadOnlyList<string> DefaultRoles { get; } = new ReadOnlyCollection<string>(
        [
            Admin,
            Basic
        ]);

    public static bool IsDefault(string roleName) => 
        DefaultRoles.Any(role => role == roleName);
} 
 //The RoleConstants class contains the role names and a list of default roles.The IsDefault method checks if a role is a default role.
 //The next step is to create a class that will seed the default roles.
 //TODO: Create a new class named IdentityDataSeeder in the Infrastructure/Identity/Data/Seeders folder.