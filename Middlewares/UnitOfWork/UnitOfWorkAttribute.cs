using Microsoft.EntityFrameworkCore;

namespace Middlewares.UnitOfWork;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple =  false)]
public class UnitOfWorkAttribute : Attribute
{
    public Type[] DbContextTypes { get; init; }
    
    public UnitOfWorkAttribute(params Type[] dbContextTypes)
    {
        DbContextTypes = dbContextTypes;

        foreach (var type in DbContextTypes)
        {
            if (!typeof(DbContext).IsAssignableFrom(type))
            {
                throw new ArgumentException($"{type} is not a DbContext");
            }
        }
    }
}