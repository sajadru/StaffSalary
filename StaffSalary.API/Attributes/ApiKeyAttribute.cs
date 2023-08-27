using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using StaffSalary.API.Attributes.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

[AttributeUsage(validOn: AttributeTargets.Class)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string ApiKeyName = "Api-Key";

    public ApiKeyAttribute()
    {
    }
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue(ApiKeyName, out var extractedApiKey))
            throw new UnauthorizedAccessException();

        var apiKey = context.HttpContext.RequestServices.GetService<APIKey>()?.Key;

        if (apiKey != extractedApiKey)
            throw new UnauthorizedAccessException();

        await next();
    }
}

