using System.Reflection;
using System.Transactions;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Middlewares.UnitOfWork;

public class UnitOfWorkFilter : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var attribute = GetAttribute(context);
        if (attribute == null)
        {
            await next();
            return;
        }

        using TransactionScope scope = new (TransactionScopeAsyncFlowOption.Enabled);
        List<DbContext> contexts = new ();

        // dbcontexts listed in the attribute
        // should be compared to the ones injected into the controller
        foreach (var ctx in attribute.DbContextTypes)
        {
            var services = context.HttpContext.RequestServices;
            var targetContext = (DbContext) services.GetRequiredService(ctx);
            
            contexts.Add(targetContext);
        }
        var result =  await next();
        if (result.Exception == null)
        {
            foreach (var ctx in contexts)
            {
                await ctx.SaveChangesAsync();
            }
            scope.Complete();
        }
    }

    private UnitOfWorkAttribute? GetAttribute(ActionExecutingContext context)
    {
        if (context.ActionDescriptor is not ControllerActionDescriptor descriptor)
        {
            return null;
        }

        // find the attribute on class first, then on method
        var classAttribute = descriptor.ControllerTypeInfo.GetCustomAttribute<UnitOfWorkAttribute>();
        if (classAttribute != null)
        {
            return classAttribute;
        }
        
        var methodAttribute = descriptor.MethodInfo.GetCustomAttribute<UnitOfWorkAttribute>();
        return methodAttribute;
    }
}