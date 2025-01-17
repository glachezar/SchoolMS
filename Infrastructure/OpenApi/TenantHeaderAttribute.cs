﻿namespace Infrastructure.OpenApi;

using Infrastructure.Tenancy;

public class TenantHeaderAttribute : SwaggerHeaderAttribute
{
    public TenantHeaderAttribute() 
        : base(
            TenancyConstants.TenantIdName, 
            "Innput your tenant name to access this API.", 
            string.Empty, 
            true)
    {
    }
}
